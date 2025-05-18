using HutongGames.PlayMaker.Actions;
using RandoMapCore.Pathfinder.Actions;
using RandomizerCore.Logic;
using RCPathfinder.Actions;

namespace RandoMapCore.Pathfinder;

internal class InstructionTracker
{
    private readonly RmcSearchData _sd;
    private readonly RouteManager _rm;

    private string _lastScene;

    internal InstructionTracker(RmcSearchData sd, RouteManager rm)
    {
        _sd = sd;
        _rm = rm;

        UpdateSequenceBreakActions();
    }

    internal AbstractAction LastAction { get; private set; }
    internal AbstractAction DreamgateLinkedAction { get; private set; } = null;

    internal void TrackDreamgateSet(
        On.HutongGames.PlayMaker.Actions.SetPlayerDataString.orig_OnEnter orig,
        SetPlayerDataString self
    )
    {
        orig(self);

        if (self.stringName.Value is "dreamGateScene")
        {
            DreamgateLinkedAction = LastAction;
            RandoMapCoreMod.Instance.LogFine($"Linking Dreamgate instruction to {LastAction}");
            _rm.ResetRoute();
        }
    }

    internal void TrackAction(ItemChanger.Transition target)
    {
        var action = GetAction(target);

        if (action is not null)
        {
            // RandoMapCoreMod.Instance.LogDebug($"Last action: {action.DebugString}");

            if (LastAction?.Target is Term lastTarget && action is StandardAction sa && IsOutOfLogic(sa.Source.Name))
            {
                RandoMapCoreMod.Instance.LogFine($"Adding OOL action: {sa.Source}");

                if (RandoMapCoreMod.LS.SequenceBreakActions.TryGetValue(lastTarget.ToString(), out var actionTargets))
                {
                    actionTargets.Add(sa.Source.ToString());
                }
                else
                {
                    RandoMapCoreMod.LS.SequenceBreakActions[lastTarget.ToString()] = new([sa.Source.ToString()]);
                }
            }
        }

        _lastScene = target.SceneName;
        LastAction = action;
    }

    internal void UpdateSequenceBreakActions()
    {
        List<string> emptySets = [];

        foreach (var kvp in RandoMapCoreMod.LS.SequenceBreakActions)
        {
            kvp.Value.RemoveWhere(s => !IsOutOfLogic(s));

            if (!kvp.Value.Any())
            {
                emptySets.Add(kvp.Key);
            }
        }

        foreach (var key in emptySets)
        {
            RandoMapCoreMod.LS.SequenceBreakActions.Remove(key);
        }
    }

    private bool IsOutOfLogic(string termName)
    {
        return RmcPathfinder.LE.LocalLM.LogicLookup.TryGetValue(termName, out var logic)
            && !logic.CanGet(RmcPathfinder.PSNoSequenceBreak.LocalPM);
    }

    private AbstractAction GetAction(ItemChanger.Transition target)
    {
        if (
            _lastScene is not null
            && _sd.StateTermsByScene.TryGetValue(_lastScene, out var lastScenePositions)
            && lastScenePositions
                .Where(_sd.StandardActionLookup.ContainsKey)
                .SelectMany(p => _sd.StandardActionLookup[p])
                .Cast<StandardAction>()
                .FirstOrDefault(a => a is IInstruction i && i.IsFinished(target))
                is StandardAction sa
        )
        {
            return sa;
        }

        if (_sd.BenchwarpActions.FirstOrDefault(ba => ba.IsFinished(target)) is BenchwarpAction ba)
        {
            return ba;
        }

        if (_sd.DreamgateAction.IsFinished(target))
        {
            return _sd.DreamgateAction;
        }

        return null;
    }
}
