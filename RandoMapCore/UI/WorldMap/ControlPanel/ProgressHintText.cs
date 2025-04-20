using RandoMapCore.Input;
using RandoMapCore.Localization;
using RandoMapCore.Settings;
using UnityEngine;

namespace RandoMapCore.UI;

internal class ProgressHintText : ControlPanelText
{
    private protected override string Name => "Progress Hint";

    private protected override bool ActiveCondition()
    {
        return RandoMapCoreMod.GS.ControlPanelOn;
    }

    private protected override Vector4 GetColor()
    {
        return RandoMapCoreMod.GS.ProgressHint is not ProgressHintSetting.Off
            ? RmcColors.GetColor(RmcColorSetting.UI_On)
            : RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }

    private protected override string GetText()
    {
        return $"{"Toggle progress hint".L()} {ProgressHintPanelInput.Instance.GetBindingsText()}: "
            + RandoMapCoreMod.GS.ProgressHint switch
            {
                ProgressHintSetting.Area => "Area".L(),
                ProgressHintSetting.Room => "Room".L(),
                ProgressHintSetting.Location => "Location".L(),
                _ => "Off".L(),
            };
    }
}
