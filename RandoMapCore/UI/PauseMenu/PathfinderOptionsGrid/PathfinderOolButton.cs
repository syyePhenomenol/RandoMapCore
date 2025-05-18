using MapChanger.UI;
using RandoMapCore.Pathfinder;

namespace RandoMapCore.UI;

internal class PathfinderOolButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            RandoMapCoreMod.GS.TogglePathfinderOutOfLogic();
            RmcPathfinder.RM.ResetRoute();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Pathfinder\nOOL".L()}: ";

        if (!RandoMapCoreMod.Data.EnableRoomSelection || !RandoMapCoreMod.Data.EnablePathfinder)
        {
            return (text + "Disabled".L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return UIExtensions.GetBoolTextFormat(text, RandoMapCoreMod.GS.PathfinderOutOfLogic);
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder
                ? "Allow sequence breaks in pathfinder search.".L()
                : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
