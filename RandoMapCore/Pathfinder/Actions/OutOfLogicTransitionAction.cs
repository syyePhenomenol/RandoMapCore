using RandomizerCore.Logic;
using RCPathfinder.Actions;

namespace RandoMapCore.Pathfinder.Actions;

/// <summary>
/// Short-circuits logic requirements to go from a position to a transition/waypoint jump source,
/// and the subsequent jump action as well.
/// </summary>
/// <param name="start"></param>
/// <param name="origAction"></param>
internal class OutOfLogicTransitionAction(Term start, TransitionAction origAction)
    : PlacementAction(start, origAction.Target),
        IInstruction
{
    public override string Prefix => "oolo";
    public override float Cost => 1f;
    public string SourceText => $"*{origAction.SourceText}";
    public string TargetText => origAction.TargetText;

    public bool IsFinished(ItemChanger.Transition lastTransition)
    {
        return origAction.IsFinished(lastTransition);
    }

    public string GetCompassObjectPath(string scene)
    {
        return origAction.GetCompassObjectPath(scene);
    }
}
