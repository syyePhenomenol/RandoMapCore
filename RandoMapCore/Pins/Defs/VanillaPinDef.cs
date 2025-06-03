using ConnectionMetadataInjector.Util;
using MagicUI.Elements;
using MapChanger;
using MapChanger.Defs;
using RandoMapCore.Data;
using RandoMapCore.Settings;
using RandomizerCore.Logic;
using UnityEngine;

namespace RandoMapCore.Pins;

internal sealed class VanillaPinDef : PinDef, ILogicPinDef
{
    private readonly string _locationPoolGroup;

    internal VanillaPinDef(RmcLocationDef vanillaLocation)
        : base()
    {
        Name = vanillaLocation.Name;
        SceneName = vanillaLocation.SceneName ?? ItemChanger.Finder.GetLocation(vanillaLocation.Name)?.sceneName;

        _locationPoolGroup = SubcategoryFinder.GetLocationPoolGroup(vanillaLocation.Name).FriendlyName();

        LocationPoolGroups = [_locationPoolGroup];
        // TODO: SUPPORT MULTIPLE ITEMS FOR DEFAULT SHOPS
        // For now, all placements at the same location are represented by one pin.
        ItemPoolGroups = [_locationPoolGroup];

        if (RmcPinManager.Dpm.GetDefaultMapPosition(Name) is MapRoomPosition mrp)
        {
            MapPosition = mrp;
            MapZone = mrp.MapZone;
        }

        if (RandoMapCoreMod.Data.PM.lm.LogicLookup.TryGetValue(Name, out var logic))
        {
            Logic = new(logic);
            TextBuilders.Add(Logic.GetLogicText);
        }
        else
        {
            RandoMapCoreMod.Instance.LogWarn($"No logic def found for vanilla placement {Name}");
        }

        if (RmcPinManager.Dpm.GetDefaultLocationHints(Name) is RawLogicDef[] hints)
        {
            Hint = new(hints);
            if (RandoMapCoreMod.Data.EnableLocationHints)
            {
                TextBuilders.Add(Hint.GetHintText);
            }
        }

        Persistent =
            _locationPoolGroup == PoolGroup.LifebloodCocoons.FriendlyName()
            || _locationPoolGroup == PoolGroup.SoulTotems.FriendlyName()
            || _locationPoolGroup == PoolGroup.LoreTablets.FriendlyName();
    }

    public LogicInfo Logic { get; init; }
    public HintInfo Hint { get; init; }
    internal bool Persistent { get; }

    internal override bool ActiveByProgress()
    {
        return RandoMapCoreMod.LS.IsActivePoolGroup(_locationPoolGroup, "Vanilla");
    }

    internal override bool ActiveBySettings()
    {
        return (!Tracker.HasClearedLocation(Name) || RandoMapCoreMod.GS.ShowClearedPins is ClearedPinsSetting.All)
            && (
                Logic?.State is LogicState.Reachable or LogicState.ReachableSequenceBreak
                || RandoMapCoreMod.GS.ShowReachablePins is not ReachablePinsSetting.HideUnreachable
            );
    }

    internal override IEnumerable<ScaledPinSprite> GetPinSprites()
    {
        return [RmcPinManager.Psm.GetLocationSprite(_locationPoolGroup)];
    }

    internal override bool ShrinkPin()
    {
        return Logic?.IndicateUnreachable() ?? false;
    }

    internal override bool DarkenPin()
    {
        return !Tracker.HasClearedLocation(Name) && (Logic?.IndicateUnreachable() ?? false);
    }

    internal override Color GetBorderColor()
    {
        if (Tracker.HasClearedLocation(Name))
        {
            return RmcColors.GetColor(RmcColorSetting.Pin_Cleared);
        }

        if (Logic?.State is LogicState.ReachableSequenceBreak)
        {
            return RmcColors.GetColor(RmcColorSetting.Pin_Out_of_logic);
        }

        if (Persistent)
        {
            return RmcColors.GetColor(RmcColorSetting.Pin_Persistent);
        }

        return RmcColors.GetColor(RmcColorSetting.Pin_Normal);
    }

    internal override PinShape GetMixedPinShape()
    {
        if (Tracker.HasClearedLocation(Name))
        {
            return PinShape.Circle;
        }

        if (Logic?.State is LogicState.ReachableSequenceBreak)
        {
            return PinShape.Pentagon;
        }

        if (Persistent)
        {
            return PinShape.Hexagon;
        }

        return base.GetMixedPinShape();
    }

    private protected override RunCollection GetStatusText()
    {
        List<string> statuses = ["Vanilla".L()];

        if (Tracker.HasClearedLocation(Name))
        {
            statuses.Add("cleared".L());
        }
        else
        {
            if (Persistent)
            {
                statuses.Add("persistent".L());
            }
            else
            {
                statuses.Add("not cleared".L());
            }

            statuses.Add(Logic?.GetStatusTextFragment() ?? "unknown logic".L());
        }

        return [new Run($"{"Status".L()}: "), .. RunCollection.Join(", ", statuses.Select(s => new Run(s)))];
    }
}
