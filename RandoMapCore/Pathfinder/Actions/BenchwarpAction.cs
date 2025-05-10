using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;
using RCPathfinder;
using RCPathfinder.Actions;

namespace RandoMapCore.Pathfinder.Actions;

internal class BenchwarpAction(Term term, RmcBenchKey benchKey) : JumpAction, IInstruction
{
    public override Term Target => term;
    public override float Cost => 1f;

    internal RmcBenchKey BenchKey { get; } = benchKey;

    public override bool TryDo(Node node, ProgressionManager pm, out StateUnion satisfiableStates)
    {
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

    public string SourceText { get; } = term.Name;
    public string TargetText => null;

    public bool IsFinished(ItemChanger.Transition lastTransition)
    {
        return lastTransition.ToString() == $"{BenchKey.SceneName}[]"
            && PlayerData.instance.GetString("respawnMarkerName") == BenchKey.RespawnMarkerName;
    }

    public string GetCompassObjectPath(string scene)
    {
        return null;
    }
}
