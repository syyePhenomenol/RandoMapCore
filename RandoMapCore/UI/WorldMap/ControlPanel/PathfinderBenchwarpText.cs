using RandoMapCore.Input;
using RandoMapCore.Localization;
using RandoMapCore.Modes;
using UnityEngine;

namespace RandoMapCore.UI;

internal class PathfinderBenchwarpText : ControlPanelText
{
    private protected override string Name => "Pathfinder Benchwarp";

    private protected override bool ActiveCondition()
    {
        return RandoMapCoreMod.GS.ControlPanelOn && Conditions.TransitionRandoModeEnabled();
    }

    private protected override Vector4 GetColor()
    {
        if (Interop.HasBenchwarp)
        {
            return RandoMapCoreMod.GS.PathfinderBenchwarp
                ? RmcColors.GetColor(RmcColorSetting.UI_On)
                : RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        }

        return RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }

    private protected override string GetText()
    {
        if (Interop.HasBenchwarp)
        {
            return $"{"Pathfinder benchwarp".L()} {PathfinderBenchwarpInput.Instance.GetBindingsText()}: {(RandoMapCoreMod.GS.PathfinderBenchwarp ? "On" : "Off").L()}";
        }

        return "Benchwarp is not installed or outdated".L();
    }
}
