using MapChanger;
using MapChanger.UI;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class RouteTextButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            RandoMapCoreMod.GS.ToggleRouteTextInGame();
            RmcUIManager.Route.Update();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Route Text".L()}:\n";

        if (!RandoMapCoreMod.Data.EnableRoomSelection || !RandoMapCoreMod.Data.EnablePathfinder)
        {
            return (text + "Disabled".L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return (
            text + RandoMapCoreMod.GS.RouteTextInGame.ToString().ToWhitespaced().L(),
            RandoMapCoreMod.GS.RouteTextInGame switch
            {
                RouteTextInGame.Hide => RmcColorSetting.UI_Neutral,
                RouteTextInGame.NextTransition => RmcColorSetting.UI_On,
                RouteTextInGame.AllTransitions => RmcColorSetting.UI_On,
                _ => RmcColorSetting.UI_Neutral,
            }
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder
                ? "How the route text is displayed during gameplay.".L()
                : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
