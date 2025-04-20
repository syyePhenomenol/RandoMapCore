using RandoMapCore.Input;
using RandoMapCore.Localization;
using UnityEngine;

namespace RandoMapCore.UI;

internal class MapKeyText : ControlPanelText
{
    private protected override string Name => "Map Key";

    private protected override bool ActiveCondition()
    {
        return RandoMapCoreMod.GS.ControlPanelOn;
    }

    private protected override Vector4 GetColor()
    {
        return RandoMapCoreMod.GS.MapKeyOn
            ? RmcColors.GetColor(RmcColorSetting.UI_On)
            : RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }

    private protected override string GetText()
    {
        return $"{"Toggle map key".L()} {MapKeyInput.Instance.GetBindingsText()}: {(RandoMapCoreMod.GS.MapKeyOn ? "On" : "Off").L()}";
    }
}
