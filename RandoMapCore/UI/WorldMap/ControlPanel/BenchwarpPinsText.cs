using RandoMapCore.Input;
using RandoMapCore.Localization;
using UnityEngine;

namespace RandoMapCore.UI;

internal class BenchwarpPinsText : ControlPanelText
{
    private protected override string Name => "Benchwarp Pins";

    private protected override bool ActiveCondition()
    {
        return RandoMapCoreMod.GS.ControlPanelOn;
    }

    private protected override Vector4 GetColor()
    {
        if (Interop.HasBenchwarp)
        {
            return RandoMapCoreMod.GS.ShowBenchwarpPins
                ? RmcColors.GetColor(RmcColorSetting.UI_On)
                : RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        }

        return RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }

    private protected override string GetText()
    {
        if (Interop.HasBenchwarp)
        {
            return $"{"Benchwarp pins".L()} {ToggleBenchwarpPinsInput.Instance.GetBindingsText()}: {(RandoMapCoreMod.GS.ShowBenchwarpPins ? "On" : "Off").L()}";
        }

        return "Benchwarp is not installed or outdated".L();
    }
}
