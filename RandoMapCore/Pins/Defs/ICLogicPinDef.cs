using ItemChanger;
using MagicUI.Elements;
using MapChanger;
using RandoMapCore.Settings;
using RandomizerCore.Logic;
using UnityEngine;
using SM = ConnectionMetadataInjector.SupplementalMetadata;

namespace RandoMapCore.Pins;

internal abstract class ICLogicPinDef : ICPinDef, ILogicPinDef
{
    internal ICLogicPinDef(AbstractPlacement placement, string poolsCollection)
        : base(placement, poolsCollection)
    {
        if (SM.Of(placement).Get(InteropProperties.LogicInfix) is string logicInfix)
        {
            try
            {
                Logic = new(RandoMapCoreMod.Data.PM.lm.CreateDNFLogicDef(new(placement.Name, logicInfix)));
                TextBuilders.Add(Logic.GetLogicText);
            }
            catch
            {
                RandoMapCoreMod.Instance.LogWarn(
                    $"Failed to make LogicDef for placement {placement.Name}: {logicInfix}"
                );
            }
        }
        else if (RandoMapCoreMod.Data.PM.lm.LogicLookup.TryGetValue(placement.Name, out var logic))
        {
            Logic = new(logic);
            TextBuilders.Add(Logic.GetLogicText);
        }
        else
        {
            RandoMapCoreMod.Instance.LogDebug($"No logic def found for placement {placement.Name}");
        }

        if (SM.Of(placement).Get(InteropProperties.LocationHints) is RawLogicDef[] hints)
        {
            Hint = new(hints);
            if (RandoMapCoreMod.Data.EnableLocationHints)
            {
                TextBuilders.Add(Hint.GetHintText);
            }
        }
    }

    public LogicInfo Logic { get; init; }
    public HintInfo Hint { get; init; }

    internal override bool ActiveBySettings()
    {
        return base.ActiveBySettings()
            && (
                Logic?.State is LogicState.Reachable or LogicState.ReachableSequenceBreak
                || RandoMapCoreMod.GS.ShowReachablePins is not ReachablePinsSetting.HideUnreachable
            );
    }

    internal override bool ShrinkPin()
    {
        return Logic?.IndicateUnreachable() ?? false;
    }

    internal override bool DarkenPin()
    {
        return State is not PlacementState.Cleared && (Logic?.IndicateUnreachable() ?? false);
    }

    internal override Color GetBorderColor()
    {
        if (Logic?.State is LogicState.ReachableSequenceBreak)
        {
            return RmcColors.GetColor(RmcColorSetting.Pin_Out_of_logic);
        }

        return base.GetBorderColor();
    }

    internal override PinShape GetMixedPinShape()
    {
        if (Logic?.State is LogicState.ReachableSequenceBreak)
        {
            return PinShape.Pentagon;
        }

        return base.GetMixedPinShape();
    }

    internal override float GetZPriority()
    {
        return base.GetZPriority() + (10f * (int)(Logic?.State ?? LogicState.Unreachable));
    }

    private protected override RunCollection GetStatusText()
    {
        if (State is PlacementState.Cleared)
        {
            return base.GetStatusText();
        }

        return [.. base.GetStatusText(), new Run(", "), new Run(Logic?.GetStatusTextFragment() ?? "unknown logic".L())];
    }
}
