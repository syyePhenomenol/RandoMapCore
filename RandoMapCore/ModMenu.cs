using ItemChanger.Internal.Menu;
using MapChanger;
using RandoMapCore.UI;

namespace RandoMapCore;

internal static class ModMenu
{
    internal static MenuScreen GetMenuScreen(MenuScreen modListMenu)
    {
        ModMenuScreenBuilder builder = new("Randomizer Map Mod".L(), modListMenu);
        builder.AddHorizontalOption(
            new(
                "Show Pause Menu".L(),
                ["Off".L(), "On".L()],
                string.Empty,
                i =>
                {
                    MapChangerMod.GS.ToggleShowPauseMenu();
                    RmcUIManager.PauseMenu?.Update();
                },
                () => MapChangerMod.GS.ShowPauseMenu ? 1 : 0
            )
        );
        return builder.CreateMenuScreen();
    }
}
