using RandomizerCore.Logic;
using RCPathfinder.Actions;

namespace RandoMapCore.Pathfinder.Actions;

internal class WaypointAction(
    Term start,
    Term destination,
    DNFLogicDef logic,
    string text,
    Dictionary<string, string> compassObjects
) : StateLogicAction(start, destination, logic), IInstruction
{
    private readonly Dictionary<string, string> _compassObjects = compassObjects;

    public string SourceText => text;
    public string TargetText => null;

    public bool IsFinished(ItemChanger.Transition lastTransition)
    {
        return lastTransition.ToString() == Target.Name;
    }

    public string GetCompassObjectPath(string scene)
    {
        if (_compassObjects is not null && _compassObjects.TryGetValue(scene, out var path))
        {
            return path;
        }

        return null;
    }
}
