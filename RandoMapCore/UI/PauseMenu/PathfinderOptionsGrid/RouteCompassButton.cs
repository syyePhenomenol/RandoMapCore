using MapChanger.UI;

namespace RandoMapCore.UI;

internal class RouteCompassButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            RandoMapCoreMod.GS.ToggleRouteCompassEnabled();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Route Compass".L()}:\n";

        if (!RandoMapCoreMod.Data.EnableRoomSelection || !RandoMapCoreMod.Data.EnablePathfinder)
        {
            return (text + "Disabled".L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return UIExtensions.GetBoolTextFormat(text, RandoMapCoreMod.GS.ShowRouteCompass);
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder
                ? "Point compass to next transition in the pathfinder route.".L()
                : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
