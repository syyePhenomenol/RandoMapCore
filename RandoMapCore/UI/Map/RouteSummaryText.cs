using MagicUI.Core;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Modes;
using RandoMapCore.Pathfinder;

namespace RandoMapCore.UI;

internal class RouteSummaryText : TopEdgeText<RmcWorldMapLayout>
{
    internal override HorizontalAlignment Alignment => HorizontalAlignment.Right;

    public override void Update()
    {
        if (ModeManager.CurrentMode() is TransitionRandoMode)
        {
            Text.Visibility = Visibility.Visible;
            base.Update();
        }
        else
        {
            Text.Visibility = Visibility.Hidden;
        }
    }

    private protected override TextFormat GetTextFormat()
    {
        var text = $"{"Route".L()}: ";

        if (RmcPathfinder.RM.CurrentRoute is not Route route)
        {
            return (text + "None".L()).ToNeutralTextFormat();
        }

        var first = route.FirstInstruction;
        var last = route.LastInstruction;

        if (last.TargetText is not null)
        {
            text += $"{first.SourceText.LT().ToCleanName()} ->...-> {last.TargetText.LT().ToCleanName()}";
        }
        else
        {
            text += first.SourceText.LT().ToCleanName();

            if (first != last)
            {
                text += $" ->...-> {last.SourceText.LT().ToCleanName()}";
            }
        }

        return (text + $"\n\n{"Transitions".L()}: {route.TotalInstructionCount}").ToNeutralTextFormat();
    }
}
