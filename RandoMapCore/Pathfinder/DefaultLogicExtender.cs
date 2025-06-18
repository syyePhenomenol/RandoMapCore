using RandomizerCore.Logic;

namespace RandoMapCore.Pathfinder;

internal class DefaultLogicExtender(LogicManager referenceLM) : RmcLogicExtender(referenceLM)
{
    protected override LogicManagerBuilder ModifyReferenceLM(LogicManagerBuilder lmb)
    {
        PdBoolWaypoints = new(new Dictionary<string, string>());
        SceneBoolWaypoints = new(new Dictionary<(string, string), string>());
        return lmb;
    }
}
