using RandoMapCore.Input;
using RandoMapCore.Localization;
using RandoMapCore.Modes;
using UnityEngine;

namespace RandoMapCore.UI;

internal class RoomSelectionText : ControlPanelText
{
    private protected override string Name => "Room Selection";

    private protected override bool ActiveCondition()
    {
        return RandoMapCoreMod.GS.ControlPanelOn && Conditions.TransitionRandoModeEnabled();
    }

    private protected override Vector4 GetColor()
    {
        return RandoMapCoreMod.GS.RoomSelectionOn
            ? RmcColors.GetColor(RmcColorSetting.UI_On)
            : RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }

    private protected override string GetText()
    {
        return $"{"Toggle room selection".L()} {RoomPanelInput.Instance.GetBindingsText()}: {(RandoMapCoreMod.GS.RoomSelectionOn ? "On" : "Off").L()}";
    }
}
