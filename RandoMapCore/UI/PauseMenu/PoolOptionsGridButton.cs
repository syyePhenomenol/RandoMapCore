using MapChanger.UI;

namespace RandoMapCore.UI;

internal class PoolOptionsGridButton : RmcGridControlButton<PoolOptionsGrid>
{
    protected override TextFormat GetTextFormat()
    {
        return (
            "Customize\nPools".L(),
            GridOpen() ? RmcColorSetting.UI_Custom : RmcColorSetting.UI_Neutral
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "Customize which item/location pools to display.".L().ToNeutralTextFormat();
    }
}
