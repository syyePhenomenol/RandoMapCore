using MapChanger.UI;

namespace RandoMapCore.UI;

internal class PathfinderOptionsGrid : RmcOptionsGrid
{
    protected override IEnumerable<ExtraButton> GetButtons()
    {
        return [new PathfinderOolButton(), new RouteCompassButton(), new RouteTextButton(), new OffRouteButton()];
    }
}
