using RandoMapCore.Localization;
using UnityEngine;

namespace RandoMapCore.UI;

internal class ShiftPanText : ControlPanelText
{
    private protected override string Name => "Shift Pan";

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
        return $"{"Hold".L()} Shift: {"Pan faster".L()}";
    }
}
