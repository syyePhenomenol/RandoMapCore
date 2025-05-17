using MapChanger;
using MapChanger.UI;
using RandoMapCore.Input;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class ProgressHintText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return (
            $"{"Toggle progress hint".L()} {ProgressHintPanelInput.Instance.GetBindingsText()}: "
                + RandoMapCoreMod.GS.ProgressHint.ToString().ToWhitespaced().L(),
            RandoMapCoreMod.GS.ProgressHint switch
            {
                ProgressHintSetting.Off => RmcColorSetting.UI_Neutral,
                ProgressHintSetting.Area => RmcColorSetting.UI_On,
                ProgressHintSetting.Room => RmcColorSetting.UI_On,
                ProgressHintSetting.Location => RmcColorSetting.UI_On,
                _ => RmcColorSetting.UI_Neutral,
            }
        ).ToTextFormat();
    }
}
