using ConnectionMetadataInjector;
using ItemChanger;
using MagicUI.Elements;
using MapChanger;
using MapChanger.Defs;
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

        SceneName = placement.GetSceneName();

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
        else if (RmcPinManager.Dpm.GetDefaultMapPosition(placement) is MapRoomPosition defaultPosition)
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

        TextBuilders.AddRange([GetNeverObtainedText, GetObtainedText, GetPersistentText]);
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
        return State is PlacementState.Previewed or PlacementState.ForcePreviewed or PlacementState.NotCleared
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

        // Placement is previewed without item previews enabled
        if (State is PlacementState.Previewed)
        {
            return [RmcPinManager.Psm.GetSprite("Unknown")];
        }

        return [RmcPinManager.Psm.GetLocationSprite(Placement)];
    }

    internal override Color GetBorderColor()
    {
        return State switch
        {
            PlacementState.Previewed or PlacementState.ForcePreviewed => RmcColors.GetColor(
                RmcColorSetting.Pin_Previewed
            ),
            PlacementState.Cleared => RmcColors.GetColor(RmcColorSetting.Pin_Cleared),
            PlacementState.ClearedPersistent => RmcColors.GetColor(RmcColorSetting.Pin_Persistent),
            _ => RmcColors.GetColor(RmcColorSetting.Pin_Normal),
        };
    }

    internal override PinShape GetMixedPinShape()
    {
        if (State is PlacementState.Previewed or PlacementState.ForcePreviewed)
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

    private protected override RunCollection GetRoomText()
    {
        if (HighlightScenes is not null && HighlightScenes.Any())
        {
            return [
                new Run($"{"Rooms".L()}: "),
                .. RunCollection.Join(", ", HighlightScenes.Select(s => new Run(s)))
            ];
        }

        return base.GetRoomText();
    }

    private protected override RunCollection GetStatusText()
    {
        var stateText = new Run(State switch
        {
            PlacementState.Previewed or PlacementState.ForcePreviewed => "previewed".L(),
            PlacementState.NotCleared => "unchecked".L(),
            PlacementState.ClearedPersistent or PlacementState.Cleared => "cleared".L(),
            _ => "unknown".L(),
        });
        return [
            new Run($"{"Status".L()}: "),
            new Run(PoolsCollection),
            new Run(", "),
            stateText
        ];
    }

    private protected virtual RunCollection GetNeverObtainedText()
    {
        var canPreviewPlacementName = Placement.CanPreviewName();
        var canPreviewPlacementCost = Placement.CanPreviewCost();
        var placementCosts = Placement.GetCosts();

        if (
            RandoMapCoreMod.Data.EnableSpoilerToggle
            && RandoMapCoreMod.LS.SpoilerOn
            && ICPlacementTracker.NeverObtainedItems.Any()
        )
        {
            return GetItemTextWithPlacementCosts(
                $"{"Spoiler item(s)".L()}: ",
                ICPlacementTracker.NeverObtainedItems,
                true
            );
        }

        if (State is PlacementState.Previewed)
        {
            return GetItemTextWithPlacementCosts(
                $"{"Previewed item(s)".L()}: ",
                ICPlacementTracker.NeverObtainedItems,
                false
            );
        }

        if (State is PlacementState.ForcePreviewed)
        {
            return GetItemTextWithPlacementCosts(
                $"{"Previewed item(s)".L()}: ",
                ICPlacementTracker.ForcePreviewedItems,
                false
            );
        }

        return null;

        RunCollection GetItemTextWithPlacementCosts(string heading, IEnumerable<AbstractItem> items, bool spoilers)
        {
            var itemText = items.ToTextWithCost(
                spoilers ? true
                    : canPreviewPlacementName ? null
                    : false,
                spoilers ? true
                    : canPreviewPlacementCost ? null
                    : false
            );

            if (placementCosts is not null)
            {
                var placementCostText = RunCollection.Join(
                    ", ",
                    placementCosts.Select(c => c.ToCostText(spoilers || canPreviewPlacementCost))
                );

                if (items.Count() is 1)
                {
                    return [new Run(heading), .. itemText, new Run(": "), .. placementCostText];
                }

                return [new Run(heading), new Run("("), .. itemText, new Run("): "), .. placementCostText];
            }

            return [new Run(heading), .. itemText];
        }
    }

    private protected virtual RunCollection GetObtainedText()
    {
        if (ICPlacementTracker.EverObtainedItems.Any())
        {
            return [
                new Run($"{"Obtained item(s)".L()}: "),
                .. ICPlacementTracker.EverObtainedItems.ToText(true)
            ];
        }

        return null;
    }

    private protected virtual RunCollection GetPersistentText()
    {
        if (ICPlacementTracker.EverObtainedPersistentItems.Any())
        {
            return [
                new Run($"{"Available persistent item(s)".L()}: "),
                .. ICPlacementTracker.EverObtainedPersistentItems.ToTextWithCost(true, true)
            ];
        }

        return null;
    }
}
