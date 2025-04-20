using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;
using RCPathfinder;

namespace RandoMapCore.Pathfinder.Actions;

internal class StartWarpAction(Term term) : BenchwarpAction(term, BenchwarpInterop.StartKey), IInstruction
{
    string IInstruction.SourceText => BenchwarpInterop.BENCH_WARP_START;

    public override bool TryDo(Node node, ProgressionManager pm, out StateUnion satisfiableStates)
    {
        if (!RandoMapCoreMod.GS.PathfinderBenchwarp)
        {
            satisfiableStates = null;
            return false;
        }

        List<State> states = [];

        if (RandoMapCoreMod.Data.WarpToStartReset is StateModifier startVariable)
        {
            foreach (var sb in node.Current.States.Select(s => new StateBuilder(s)))
            {
                states.AddRange(startVariable.ModifyState(null, pm, new(sb)).Select(sb => sb.GetState()));
            }
        }

        satisfiableStates = new(states);
        return true;
    }
}
