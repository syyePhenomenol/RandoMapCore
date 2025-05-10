using ConnectionMetadataInjector;
using ItemChanger;
using MapChanger.Defs;
using RandoMapCore.Localization;
using RandoMapCore.Settings;
using UnityEngine;
using SM = ConnectionMetadataInjector.SupplementalMetadata;

namespace RandoMapCore.Pins;

internal abstract class ICPinDef : PinDef
{
    internal ICPinDef(AbstractPlacement placement, string poolsCollection)
        : base()
    {
        Placement = placement;
        ICPlacementTracker = new(placement);

        Name = placement.Name;

        LocationPoolGroups = [SM.Of(placement).Get(InjectedProps.LocationPoolGroup)];

        HashSet<string> itemPoolGroups = [];
        foreach (var item in placement.Items)
        {
            _ = itemPoolGroups.Add(SM.Of(item).Get(InjectedProps.ItemPoolGroup));
        }

        ItemPoolGroups = itemPoolGroups;

        SceneName = placement.GetScene();

        if (
            SM.Of(placement).Get(InteropProperties.WorldMapLocation) is (string, float, float) worldMapLocation
            && MapChanger.Finder.IsMappedScene(worldMapLocation.scene)
        )
        {
            MapPosition = new WorldMapPosition(worldMapLocation);
        }
        else if (
            SM.Of(placement).Get(InteropProperties.WorldMapLocations) is (string, float, float)[] worldMapLocations
            && worldMapLocations.FirstOrDefault() is (string scene, float x, float y) obseleteWorldMapLocation
            && MapChanger.Finder.IsMappedScene(obseleteWorldMapLocation.Item1)
        )
        {
            MapPosition = new WorldMapPosition(obseleteWorldMapLocation);
        }
        else if (
            SM.Of(placement).Get(InteropProperties.MapLocations) is (string, float, float)[] mapLocations
            && mapLocations.Where(l => MapChanger.Finder.IsMappedScene(l.Item1)).FirstOrDefault()
                is
                (string, float, float) mapLocation
            && mapLocation != default
        )
        {
            MapPosition = new MapRoomPosition(mapLocation);
        }
        else if (SM.Of(placement).Get(InteropProperties.AbsMapLocation) is (float, float) absMapLocation)
        {
            MapPosition = new AbsMapPosition(absMapLocation);
        }
        else if (RmcPinManager.Dpm.GetDefaultMapPosition(placement.Name) is MapRoomPosition defaultPosition)
        {
            MapPosition = defaultPosition;
        }
        else
        {
            RandoMapCoreMod.Instance.LogFine(
                $"Placement {placement.Name} is missing a valid map location. It will be placed into the grid"
            );
        }

        if (MapPosition is MapRoomPosition mrp)
        {
            MapZone = mrp.MapZone;
        }

        ModSource = SM.Of(placement).Get(InteropProperties.ModSource);
        GridIndex = SM.Of(placement).Get(InteropProperties.PinGridIndex);

        if (SM.Of(placement).Get(InteropProperties.HighlightScenes) is string[] highlightScenes)
        {
            HighlightScenes = new HashSet<string>(highlightScenes);
        }

        PoolsCollection = poolsCollection;

        TextBuilders.AddRange([GetPreviewText, GetObtainedText, GetPersistentText, GetUnobtainedText]);
    }

    internal string ModSource { get; }
    internal int GridIndex { get; }
    internal IReadOnlyCollection<string> HighlightScenes { get; }

    internal string PoolsCollection { get; }

    internal AbstractPlacement Placement { get; }
    internal ICPlacementTracker ICPlacementTracker { get; }
    internal PlacementState State => ICPlacementTracker.State;

    internal override void Update()
    {
        ICPlacementTracker.Update();
    }

    internal override bool ActiveBySettings()
    {
        if (RandoMapCoreMod.Data.EnableSpoilerToggle && RandoMapCoreMod.LS.GroupBy is GroupBySetting.Item)
        {
            return ItemPoolGroups.Any(p => RandoMapCoreMod.LS.IsActivePoolGroup(p, PoolsCollection));
        }

        return LocationPoolGroups.Any(p => RandoMapCoreMod.LS.IsActivePoolGroup(p, PoolsCollection));
    }

    internal override bool ActiveByProgress()
    {
        return State is PlacementState.Previewable or PlacementState.NotCleared
            || (
                State is PlacementState.ClearedPersistent
                && RandoMapCoreMod.GS.ShowClearedPins is ClearedPinsSetting.Persistent or ClearedPinsSetting.All
            )
            || RandoMapCoreMod.GS.ShowClearedPins is ClearedPinsSetting.All;
    }

    internal override IEnumerable<ScaledPinSprite> GetPinSprites()
    {
        if (ICPlacementTracker.ActiveItems.Any())
        {
            return ICPlacementTracker
                .ActiveItems.Select(RmcPinManager.Psm.GetSprite)
                .GroupBy(s => s.Value)
                .Select(g => g.First());
        }

        return [RmcPinManager.Psm.GetLocationSprite(Placement)];
    }

    internal override Color GetBorderColor()
    {
        return State switch
        {
            PlacementState.Previewable => RmcColors.GetColor(RmcColorSetting.Pin_Previewed),
            PlacementState.Cleared => RmcColors.GetColor(RmcColorSetting.Pin_Cleared),
            PlacementState.ClearedPersistent => RmcColors.GetColor(RmcColorSetting.Pin_Persistent),
            _ => RmcColors.GetColor(RmcColorSetting.Pin_Normal),
        };
    }

    internal override PinShape GetMixedPinShape()
    {
        if (State is PlacementState.Previewable)
        {
            return PinShape.Diamond;
        }

        if (State is PlacementState.ClearedPersistent)
        {
            return PinShape.Hexagon;
        }

        return base.GetMixedPinShape();
    }

    internal override float GetZPriority()
    {
        return (int)State;
    }

    private protected override string GetRoomText()
    {
        if (HighlightScenes is not null && HighlightScenes.Any())
        {
            return $"{"Rooms".L()}: {string.Join(", ", HighlightScenes)}";
        }

        return base.GetRoomText();
    }

    private protected override string GetStatusText()
    {
        return $"{"Status".L()}: {PoolsCollection}, "
            + State switch
            {
                PlacementState.Previewable => "previewed".L(),
                PlacementState.NotCleared => "unchecked".L(),
                PlacementState.ClearedPersistent or PlacementState.Cleared => "cleared".L(),
                _ => "",
            };
    }

    private protected virtual string GetPreviewText()
    {
        var text = $"{"Previewed item(s)".L()}: ";

        if (Placement.GetTagPreviewText() is var previewTexts && previewTexts.Any())
        {
            return text + string.Join(", ", previewTexts);
        }

        // Default handler if tag isn't found
        if (ICPlacementTracker.NeverObtainedPreviewableItems.Any())
        {
            return text + string.Join(", ", ToStringList(ICPlacementTracker.NeverObtainedPreviewableItems));
        }

        return null;
    }

    private protected virtual string GetObtainedText()
    {
        var obtainedItems = ICPlacementTracker.EverObtainedPersistentItems.Concat(
            ICPlacementTracker.EverObtainedNonPersistentItems
        );

        if (obtainedItems.Any())
        {
            return $"{"Obtained item(s)".L()}: {ToStringList(obtainedItems)}";
        }

        return null;
    }

    private protected virtual string GetPersistentText()
    {
        if (ICPlacementTracker.EverObtainedPersistentItems.Any())
        {
            return $"{"Available persistent item(s)".L()}: {ToStringList(ICPlacementTracker.EverObtainedPersistentItems)}";
        }

        return null;
    }

    private protected virtual string GetUnobtainedText()
    {
        if (
            RandoMapCoreMod.Data.EnableSpoilerToggle
            && RandoMapCoreMod.LS.SpoilerOn
            && ICPlacementTracker.NeverObtainedUnpreviewableItems.Any()
        )
        {
            return $"{"Spoiler item(s)".L()}: {ToStringList(ICPlacementTracker.NeverObtainedUnpreviewableItems)}";
        }

        return null;
    }

    private string ToStringList(IEnumerable<AbstractItem> items)
    {
        return string.Join(", ", items.Select(i => i.GetPreviewName().LC()));
    }
}
