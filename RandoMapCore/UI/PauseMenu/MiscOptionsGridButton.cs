using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class MiscOptionsGridButton : RmcGridControlButton<MiscOptionsGrid>
{
    protected override TextFormat GetTextFormat()
    {
        return (
            "Misc.\nOptions".L(),
            GridOpen() ? RmcColorSetting.UI_Custom : RmcColorSetting.UI_Neutral
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "Some miscellaneous options.".L().ToNeutralTextFormat();
    }
}
