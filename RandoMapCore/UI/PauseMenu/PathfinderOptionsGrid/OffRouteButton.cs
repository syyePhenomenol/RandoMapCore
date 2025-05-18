using MapChanger;
using MapChanger.UI;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class OffRouteButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            RandoMapCoreMod.GS.ToggleWhenOffRoute();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Off Route".L()}:\n";

        if (!RandoMapCoreMod.Data.EnableRoomSelection || !RandoMapCoreMod.Data.EnablePathfinder)
        {
            return (text + "Disabled".L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return (
            text + RandoMapCoreMod.GS.WhenOffRoute.ToString().ToWhitespaced().L(),
            RandoMapCoreMod.GS.WhenOffRoute switch
            {
                OffRouteBehaviour.CancelRoute => RmcColorSetting.UI_Neutral,
                OffRouteBehaviour.KeepRoute => RmcColorSetting.UI_Neutral,
                OffRouteBehaviour.Reevaluate => RmcColorSetting.UI_On,
                _ => RmcColorSetting.UI_Neutral,
            }
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder
                ? "When going off route, how the pathfinder route is updated.".L()
                : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
