using RandomizerCore.Logic;

namespace RandoMapCore.Pins;

public class LogicInfo
{
    private readonly LogicDef _logic;
    private readonly ProgressionManager _pm;
    private readonly ProgressionManager _pmNoSequenceBreak;

    internal LogicInfo(LogicDef logic, ProgressionManager pm, ProgressionManager pmNoSequenceBreak)
    {
        _logic = logic;
        _pm = pm;
        _pmNoSequenceBreak = pmNoSequenceBreak;
    }

    internal LogicState State { get; private set; }

    internal void Update()
    {
        if (_logic.CanGet(_pmNoSequenceBreak))
        {
            State = LogicState.Reachable;
        }
        else if (_logic.CanGet(_pm))
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

    internal string GetLogicText()
    {
        return $"{"Logic".L()}: {_logic.InfixSource}";
    }
}

internal enum LogicState
{
    Reachable,
    ReachableSequenceBreak,
    Unreachable,
}
