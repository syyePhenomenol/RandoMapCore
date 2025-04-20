using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;
using RCPathfinder;
using RCPathfinder.Actions;

namespace RandoMapCore.Pathfinder.Actions;

internal class BenchwarpAction(Term term, RmcBenchKey benchKey) : StartJumpAction, IInstruction
{
    private readonly string _benchText = term.Name;

    public override Term Target => term;
    public override float Cost => 1f;

    internal RmcBenchKey BenchKey { get; } = benchKey;

    public override bool TryDo(Node node, ProgressionManager pm, out StateUnion satisfiableStates)
    {
        if (!RandoMapCoreMod.GS.PathfinderBenchwarp || !BenchwarpInterop.GetVisitedBenchKeys().Contains(BenchKey))
        {
            satisfiableStates = null;
            return false;
        }

        List<State> states = [];
        if (RandoMapCoreMod.Data.WarpToBenchReset is StateModifier benchVariable)
        {
            foreach (var sb in node.Current.States.Select(s => new StateBuilder(s)))
            {
                states.AddRange(benchVariable.ModifyState(null, pm, new(sb)).Select(sb => sb.GetState()));
            }
        }

        satisfiableStates = new(states);
        return true;
    }

    string IInstruction.SourceText => _benchText;
    string IInstruction.TargetText => null;

    bool IInstruction.IsFinished(ItemChanger.Transition lastTransition)
    {
        return lastTransition.ToString() == $"{BenchKey.SceneName}[]"
            && PlayerData.instance.GetString("respawnMarkerName") == BenchKey.RespawnMarkerName;
    }

    string IInstruction.GetCompassObjectPath(string scene)
    {
        return null;
    }
}
