using RandoMapCore.Input;
using RandoMapCore.Localization;
using UnityEngine;

namespace RandoMapCore.UI;

internal class PinSelectionText : ControlPanelText
{
    private protected override string Name => "Pin Selection";

    private protected override bool ActiveCondition()
    {
        return RandoMapCoreMod.GS.ControlPanelOn;
    }

    private protected override Vector4 GetColor()
    {
        return RandoMapCoreMod.GS.PinSelectionOn
            ? RmcColors.GetColor(RmcColorSetting.UI_On)
            : RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }

    private protected override string GetText()
    {
        return $"{"Toggle pin selection".L()} {PinPanelInput.Instance.GetBindingsText()}: {(RandoMapCoreMod.GS.PinSelectionOn ? "On" : "Off").L()}";
    }
}
