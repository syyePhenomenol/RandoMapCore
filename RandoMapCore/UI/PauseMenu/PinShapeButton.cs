using MapChanger;
using MapChanger.UI;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class PinShapeButton : RmcMainButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RandoMapCoreMod.GS.TogglePinShape();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Pin Shape".L()}:\n";

        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            return (
                text + PinShapeSetting.Mixed.ToString().ToWhitespaced().L(),
                RmcColorSetting.UI_Disabled
            ).ToTextFormat();
        }

        return (text + RandoMapCoreMod.GS.PinShapes.ToString().ToWhitespaced().L()).ToNeutralTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableVisualCustomization ? "Toggle the shape of the pins.".L() : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
