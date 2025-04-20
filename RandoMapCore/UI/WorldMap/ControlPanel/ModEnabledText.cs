using MapChanger.Input;
using RandoMapCore.Localization;
using UnityEngine;

namespace RandoMapCore.UI;

internal class ModEnabledText : ControlPanelText
{
    private protected override string Name => "Mod Enabled";

    private protected override bool ActiveCondition()
    {
        return RandoMapCoreMod.GS.ControlPanelOn;
    }

    private protected override Vector4 GetColor()
    {
        return RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }

    private protected override string GetText()
    {
        return $"{ModEnabledInput.Instance.GetBindingsText()}: {"Disable mod".L()}";
    }
}
