using MapChanger;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class ModEnabledButton : RmcMainButton
{
    protected override TextFormat GetTextFormat()
    {
        return ($"{"Map Mod".L()}\n{"Enabled".L()}", RmcColorSetting.UI_On).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "Toggle all map mod behavior on/off.".L().ToNeutralTextFormat();
    }

    protected override void OnClick()
    {
        ModeManager.ToggleModEnabled();
    }
}
