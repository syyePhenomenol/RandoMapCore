using MapChanger;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class ReachablePinsButton : ExtraButton
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleShowReachablePins();
    }

    protected override TextFormat GetTextFormat()
    {
        return (
            $"{"Reachable Pins".L()}:\n" + RandoMapCoreMod.GS.ShowReachablePins.ToString().ToWhitespaced().L()
        ).ToNeutralTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "Toggle how reachable locations are displayed.".L().ToNeutralTextFormat();
    }
}
