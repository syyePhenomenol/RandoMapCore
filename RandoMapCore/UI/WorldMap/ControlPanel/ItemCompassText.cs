using RandoMapCore.Input;
using RandoMapCore.Localization;
using UnityEngine;

namespace RandoMapCore.UI;

internal class ItemCompassText : ControlPanelText
{
    private protected override string Name => "Item Compass";

    private protected override bool ActiveCondition()
    {
        return RandoMapCoreMod.GS.ControlPanelOn;
    }

    private protected override Vector4 GetColor()
    {
        return RandoMapCoreMod.GS.ItemCompassOn
            ? RmcColors.GetColor(RmcColorSetting.UI_On)
            : RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }

    private protected override string GetText()
    {
        return $"{"Toggle item compass".L()} {ToggleItemCompassInput.Instance.GetBindingsText()}: {(RandoMapCoreMod.GS.ItemCompassOn ? "On" : "Off").L()}";
    }
}
