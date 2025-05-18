using MapChanger;
using MapChanger.Input;
using MapChanger.UI;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class ModeText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        var text =
            $"{"Mode".L()} {ToggleModeInput.Instance.GetBindingsText()}: {ModeManager.CurrentMode().ModeName.L()}";

        if (ModeManager.CurrentMode() is FullMapMode)
        {
            return (text, RmcColorSetting.UI_On).ToTextFormat();
        }

        if (Conditions.TransitionRandoModeEnabled())
        {
            return (text, RmcColorSetting.UI_Special).ToTextFormat();
        }

        return text.ToNeutralTextFormat();
    }
}
