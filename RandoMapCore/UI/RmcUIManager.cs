namespace RandoMapCore.UI;

internal static class RmcUIManager
{
    // Visible when the game is paused
    internal static RmcPauseMenuLayout PauseMenu { get; private set; }

    // Visible when the world map is open
    internal static RmcWorldMapLayout WorldMap { get; private set; }

    // Visible when the quick map is open
    internal static QuickMapLayout QuickMap { get; private set; }

    // Visible when either world map/quick map is open
    internal static RmcBottomRowLayout BottomRow { get; private set; }

    // Visible in world map/in gameplay depending on settings
    internal static RouteLayout Route { get; private set; }

    internal static void Build()
    {
        PauseMenu = new();
        WorldMap = new();
        QuickMap = new();
        BottomRow = new();

        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            Route = new();
        }
    }

    internal static void UpdateAll()
    {
        PauseMenu?.Update();
        WorldMap?.Update();
        QuickMap?.Update();
        BottomRow?.Update();
        Route?.Update();
    }

    internal static void Destroy()
    {
        PauseMenu?.Destroy();
        WorldMap?.Destroy();
        QuickMap?.Destroy();
        BottomRow?.Destroy();
        Route?.Destroy();

        PauseMenu = null;
        WorldMap = null;
        QuickMap = null;
        BottomRow = null;
        Route = null;
    }
}
