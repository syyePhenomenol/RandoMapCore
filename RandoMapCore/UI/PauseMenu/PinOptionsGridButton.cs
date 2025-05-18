using MapChanger.UI;

namespace RandoMapCore.UI;

internal class PinOptionsGridButton : RmcGridControlButton<PinOptionsGrid>
{
    protected override TextFormat GetTextFormat()
    {
        return (
            "More Pin\nOptions".L(),
            GridOpen() ? RmcColorSetting.UI_Custom : RmcColorSetting.UI_Neutral
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "More options for pin behavior.".L().ToNeutralTextFormat();
    }
}
