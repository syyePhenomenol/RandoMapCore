using MagicUI.Core;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Pathfinder;
using RandoMapCore.Pathfinder.Actions;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class RouteText : TopEdgeText<RouteLayout>
{
    internal override HorizontalAlignment Alignment => HorizontalAlignment.Left;

    private protected override TextFormat GetTextFormat()
    {
        var text = "";

        if (RmcPathfinder.RM.CurrentRoute is not Route route)
        {
            return text.ToNeutralTextFormat();
        }

        if (
            RandoMapCoreMod.GS.RouteTextInGame is RouteTextInGame.NextTransition
            && !States.QuickMapOpen
            && !States.WorldMapOpen
        )
        {
            return route.CurrentInstruction.ToArrowedText().ToNeutralTextFormat();
        }

        foreach (var instruction in route.RemainingInstructions)
        {
            if (text.Length > (int)(100f / Math.Pow(MapChangerMod.GS.UIScale, 3)))
            {
                text += " -> ..." + route.LastInstruction.SourceText;
                break;
            }

            text += instruction.ToArrowedText();
        }

        if (
            (States.WorldMapOpen || States.QuickMapOpen)
            && route.GetHintText() is string hints
            && hints != string.Empty
        )
        {
            text += $"\n\n{hints}";
        }

        return text.ToNeutralTextFormat();
    }
}
