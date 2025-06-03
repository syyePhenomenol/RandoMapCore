using MagicUI.Elements;
using MapChanger;
using RandomizerCore.Logic;

namespace RandoMapCore.Pins;

public class LogicInfo(LogicDef logic)
{
    internal LogicState State { get; private set; }

    internal void Update()
    {
        if (logic.CanGet(RandoMapCoreMod.Data.PMNoSequenceBreak))
        {
            State = LogicState.Reachable;
        }
        else if (logic.CanGet(RandoMapCoreMod.Data.PM))
        {
            State = LogicState.ReachableSequenceBreak;
        }
        else
        {
            State = LogicState.Unreachable;
        }
    }

    internal bool IndicateUnreachable()
    {
        return RandoMapCoreMod.GS.ShowReachablePins is not Settings.ReachablePinsSetting.ExpandAll
            && State is LogicState.Unreachable;
    }

    internal string GetStatusTextFragment()
    {
        return State switch
        {
            LogicState.Reachable => "reachable".L(),
            LogicState.ReachableSequenceBreak => "reachable through sequence break".L(),
            LogicState.Unreachable => "unreachable".L(),
            _ => null,
        };
    }

    internal RunCollection GetLogicText()
    {
        return [new Run($"{"Logic".L()}: "), new Run(logic.InfixSource)];
    }
}

internal enum LogicState
{
    Reachable,
    ReachableSequenceBreak,
    Unreachable,
}
