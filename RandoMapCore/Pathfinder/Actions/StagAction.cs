using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;
using RCPathfinder;

namespace RandoMapCore.Pathfinder.Actions;

internal class StagAction(
    Term start,
    Term destination,
    DNFLogicDef logic,
    string text,
    Dictionary<string, string> compassObjects
) : WaypointAction(start, destination, logic, text, compassObjects)
{
    public override bool TryDo(Node node, ProgressionManager pm, out StateUnion satisfiableStates)
    {
        if (RandoMapCoreMod.GS.PathfinderStags)
        {
            return base.TryDo(node, pm, out satisfiableStates);
        }

        satisfiableStates = default;
        return false;
    }

    public override bool TryDoStateless(Node node, ProgressionManager pm)
    {
        return RandoMapCoreMod.GS.PathfinderStags && base.TryDoStateless(node, pm);
    }
}
