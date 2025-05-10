using MapChanger.UI;

namespace RandoMapCore.UI;

internal class PathfinderOptionsPanel : RmcOptionsPanel
{
    internal PathfinderOptionsPanel()
        : base(nameof(PathfinderOptionsPanel), GetButtons())
    {
        Instance = this;
    }

    internal static PathfinderOptionsPanel Instance { get; private set; }

    private static IEnumerable<ExtraButton> GetButtons()
    {
        return [new PathfinderOolButton(), new RouteCompassButton(), new RouteTextButton(), new OffRouteButton()];
    }
}
