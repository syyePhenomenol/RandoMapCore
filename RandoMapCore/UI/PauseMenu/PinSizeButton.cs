using MapChanger;
using MapChanger.UI;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class PinSizeButton : RmcMainButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RandoMapCoreMod.GS.TogglePinSize();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Pin Size".L()}:\n";

        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            return (text + PinSize.Medium.ToString().ToWhitespaced().L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return (text + RandoMapCoreMod.GS.PinSize.ToString().ToWhitespaced().L()).ToNeutralTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableVisualCustomization ? "Toggle overall size of pins.".L() : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
