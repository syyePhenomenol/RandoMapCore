using MapChanger;
using MapChanger.UI;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class ClearedButton : ExtraButton
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleShowClearedPins();
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Show Cleared".L()}:\n";

        return (
            text + RandoMapCoreMod.GS.ShowClearedPins.ToString().ToWhitespaced().L(),
            RandoMapCoreMod.GS.ShowClearedPins switch
            {
                ClearedPinsSetting.Off => RmcColorSetting.UI_Neutral,
                ClearedPinsSetting.Persistent => RmcColorSetting.UI_On,
                ClearedPinsSetting.All => RmcColorSetting.UI_On,
                _ => RmcColorSetting.UI_Neutral,
            }
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "Show pins for persistent or all cleared locations.".L().ToNeutralTextFormat();
    }
}
