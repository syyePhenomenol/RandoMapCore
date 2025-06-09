using RandomizerCore.Logic;
using RCPathfinder.Actions;

namespace RandoMapCore.Pathfinder.Actions;

/// <summary>
/// Represents sequence breaks that are outside of what is normally tracked by the randomizer
/// (e.g. vanilla transitions, stags, waypoint jumps)
/// Short-circuits logic requirements to go from a position to a transition/waypoint jump source,
/// and the subsequent jump action as well.
/// </summary>
/// <param name="start"></param>
/// <param name="origAction"></param>
internal class SuperSequenceBreakAction(Term start, IInstruction origAction)
    : PlacementAction(start, ((StandardAction)origAction).Target),
        IInstruction
{
    public override string Prefix => "ssbr";
    public override float Cost => 1f;
    public string SourceText => $"*{OrigAction.SourceText}";
    public string TargetText => OrigAction.TargetText;

    internal IInstruction OrigAction { get; } = origAction;

    public bool IsFinished(string lastTransition)
    {
        return OrigAction.IsFinished(lastTransition);
    }

    public string GetCompassObjectPath(string scene)
    {
        return OrigAction.GetCompassObjectPath(scene);
    }

    public bool Equals(IInstruction other)
    {
        if (other is SuperSequenceBreakAction ssba)
        {
            return ReferenceEquals(OrigAction, ssba.OrigAction);
        }

        return ReferenceEquals(OrigAction, other);
    }
}
