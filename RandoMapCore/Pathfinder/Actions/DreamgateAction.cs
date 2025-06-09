using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;
using RCPathfinder;
using RCPathfinder.Actions;

namespace RandoMapCore.Pathfinder.Actions;

internal class DreamgateAction() : JumpAction, IInstruction
{
    // Used for figuring out where Dreamgate goes to during the pathfinder search
    public override Term Target => RmcPathfinder.LE.LocalLM.GetTerm(RandoMapCoreMod.LS.DreamgateLinkedTerm);
    public override float Cost => 1f;

    public override bool TryDo(Node node, ProgressionManager pm, out StateUnion satisfiableStates)
    {
        List<State> states = [];
        foreach (var sb in node.Current.States.Select(s => new StateBuilder(s)))
        {
            _ = sb.TrySetStateBool(pm.lm.StateManager, "NOFLOWER", true);
            states.Add(new(sb));
        }

        satisfiableStates = new(states);
        return true;
    }

    public string SourceText => "Dreamgate";
    public string TargetText => null;

    public bool IsFinished(string lastTransition)
    {
        return lastTransition.Contains("dreamGate");
    }

    public string GetCompassObjectPath(string scene)
    {
        return null;
    }

    public bool Equals(IInstruction other)
    {
        return ReferenceEquals(this, other);
    }
}
