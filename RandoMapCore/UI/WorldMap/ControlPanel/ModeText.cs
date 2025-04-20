using MapChanger.Input;
using RandoMapCore.Localization;
using RandoMapCore.Modes;
using UnityEngine;

namespace RandoMapCore.UI;

internal class ModeText : ControlPanelText
{
    private protected override string Name => "Mode";

    private protected override bool ActiveCondition()
    {
        return RandoMapCoreMod.GS.ControlPanelOn;
    }

    private protected override Vector4 GetColor()
    {
        if (MapChanger.Settings.CurrentMode() is FullMapMode)
        {
            return RmcColors.GetColor(RmcColorSetting.UI_On);
        }
        else if (Conditions.TransitionRandoModeEnabled())
        {
            return RmcColors.GetColor(RmcColorSetting.UI_Special);
        }
        else
        {
            return RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        }
    }

    private protected override string GetText()
    {
        return $"{"Mode".L()} {ToggleModeInput.Instance.GetBindingsText()}: {MapChanger.Settings.CurrentMode().ModeName.L()}";
    }
}
