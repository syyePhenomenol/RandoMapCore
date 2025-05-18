using MapChanger.UI;

namespace RandoMapCore.UI;

internal class PathfinderOptionsGridButton : RmcGridControlButton<PathfinderOptionsGrid>
{
    protected override TextFormat GetTextFormat()
    {
        return (
            "Pathfinder\nOptions".L(),
            GridOpen() ? RmcColorSetting.UI_Custom : RmcColorSetting.UI_Neutral
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "Pathfinder options.".L().ToNeutralTextFormat();
    }
}
