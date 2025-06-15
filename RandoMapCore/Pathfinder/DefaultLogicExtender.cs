using RandomizerCore.Logic;

namespace RandoMapCore.Pathfinder;

internal class DefaultLogicExtender(LogicManager referenceLM) : RmcLogicExtender(referenceLM)
{
    protected override LogicManagerBuilder ModifyReferenceLM(LogicManagerBuilder lmb)
    {
        return lmb;
    }
}
