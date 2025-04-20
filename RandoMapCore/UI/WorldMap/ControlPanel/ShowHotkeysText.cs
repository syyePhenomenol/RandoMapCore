using RandoMapCore.Input;
using RandoMapCore.Localization;
using UnityEngine;

namespace RandoMapCore.UI;

internal class ShowHotkeysText : ControlPanelText
{
    private protected override string Name => "Show Hotkeys";

    private protected override bool ActiveCondition()
    {
        return true;
    }

    private protected override Vector4 GetColor()
    {
        return RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }

    private protected override string GetText()
    {
        return $"{ControlPanelInput.Instance.GetBindingsText()}: {(RandoMapCoreMod.GS.ControlPanelOn ? "Hide hotkeys" : "More hotkeys").L()}";
    }
}
