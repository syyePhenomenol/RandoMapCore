using RandoMapCore.Pathfinder.Actions;
using RandoMapCore.Transition;
using RandomizerCore.Logic;
using RCPathfinder;
using RCPathfinder.Actions;

namespace RandoMapCore.Pathfinder;

internal class InstructionTracker
{
    private readonly RmcSearchData _sd;

    internal InstructionTracker(RmcSearchData sd)
    {
        _sd = sd;

        UpdateSequenceBreakActions();
    }

    internal AbstractAction LastAction { get; private set; }

    internal void TrackAction(ItemChanger.Transition target)
    {
        var action = GetAction(target.ToString());

        if (action is not null)
        {
            RandoMapCoreMod.Instance.LogFine($"Last action: {action.DebugString}");

            if (
                LastAction?.Target is Term lastTarget
                && action is StandardAction sa
                && IsSuperSequenceBreak(lastTarget, sa)
            )
            {
                RandoMapCoreMod.Instance.LogFine($"Adding super sequence break: {((IInstruction)sa).SourceText}");

                if (RandoMapCoreMod.LS.SuperSequenceBreaks.TryGetValue(lastTarget.ToString(), out var actionTargets))
                {
                    actionTargets.Add(sa.Target.ToString());
                }
                else
                {
                    RandoMapCoreMod.LS.SuperSequenceBreaks[lastTarget.ToString()] = new([sa.Target.ToString()]);
                }
            }
        }

        LastAction = action;
    }

    internal void LinkDreamgate()
    {
        RandoMapCoreMod.Instance.LogFine($"Linking Dreamgate instruction to {LastAction}");
        RandoMapCoreMod.LS.SetDreamgateLinkedTerm(LastAction?.Target);
    }

    internal void UpdateSequenceBreakActions()
    {
        List<string> emptySets = [];

        foreach (var kvp in RandoMapCoreMod.LS.SuperSequenceBreaks)
        {
            if (RmcPathfinder.LE.LocalLM.GetTerm(kvp.Key) is not Term term)
            {
                emptySets.Add(kvp.Key);
                continue;
            }

            kvp.Value.RemoveWhere(target =>
                !_sd.TargetInstructionLookup.TryGetValue(target, out var sa)
                || !IsSuperSequenceBreak(term, (StandardAction)sa)
            );

            if (!kvp.Value.Any())
            {
                emptySets.Add(kvp.Key);
            }
        }

        foreach (var key in emptySets)
        {
            RandoMapCoreMod.LS.SuperSequenceBreaks.Remove(key);
        }
    }

    private AbstractAction GetAction(string target)
    {
        if (_sd.TargetInstructionLookup.TryGetValue(target, out var standardAction))
        {
            return (StandardAction)standardAction;
        }

        if (_sd.BenchwarpActions.FirstOrDefault(ba => ba.IsFinished(target)) is BenchwarpAction ba)
        {
            return ba;
        }

        if (_sd.DreamgateAction.IsFinished(target))
        {
            return _sd.DreamgateAction;
        }

        RandoMapCoreMod.Instance.LogDebug($"Could not find action that matches target {target}");

        return null;
    }

    // Checks if the action was not reachable nor performable from the term of the previous scene,
    // allowing sequence breaks.
    private static bool IsSuperSequenceBreak(Term lastSceneTerm, StandardAction action)
    {
        try
        {
            // Avoid sequence breaks across different scenes (ie. door warps, disconnections caused by mid-save placement changes)
            if (
                TransitionData.TryGetScene(lastSceneTerm.ToString(), out var lastScene)
                && TransitionData.TryGetScene(action.Source.ToString(), out var sourceScene)
                && lastScene != sourceScene
            )
            {
                return false;
            }

            SearchParams sp =
                new()
                {
                    StartPositions = [lastSceneTerm.ToStartPosition(0f)],
                    Destinations = [action.Target],
                    MaxCost = 1f,
                    MaxTime = 1000f,
                    DisallowBacktracking = true,
                    TerminationCondition = TerminationConditionType.Any,
                };
            SearchState ss = new(sp);

            RmcPathfinder.SD.SuperSequenceBreakTempMode = true;
            var result = Algorithms.DijkstraSearch(RmcPathfinder.PS.LocalPM, RmcPathfinder.SD, sp, ss);
            RmcPathfinder.SD.SuperSequenceBreakTempMode = false;

            return !result;
        }
        catch (Exception e)
        {
            RandoMapCoreMod.Instance.LogError(e);
        }

        return false;
    }
}
