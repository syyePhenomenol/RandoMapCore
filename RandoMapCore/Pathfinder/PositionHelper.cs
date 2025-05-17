using MapChanger;
using RandoMapCore.Transition;
using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;
using RCPathfinder;
using RCPathfinder.Actions;

namespace RandoMapCore.Pathfinder;

internal static class PositionHelper
{
    internal static IEnumerable<Position> GetStartPositions(string scene, bool withArbitraryPosition)
    {
        try
        {
            if (RandoMapCoreMod.GS.PathfinderOutOfLogic)
            {
                return GetPrunedPositionsFromScene(RmcPathfinder.PS.LocalPM, scene)
                    .Select(GetNormalStartPosition)
                    .Concat(GetOutOfLogicStarts(scene))
                    .Distinct()
                    .Concat(withArbitraryPosition ? [GetArbitraryPosition()] : []);
            }

            return GetPrunedPositionsFromScene(RmcPathfinder.PSNoSequenceBreak.LocalPM, scene)
                .Select(GetNormalStartPosition)
                .Concat(withArbitraryPosition ? [GetArbitraryPosition()] : []);
        }
        catch (Exception e)
        {
            RandoMapCoreMod.Instance.LogError(e);
        }

        return [];
    }

    internal static IEnumerable<Position> GetStartPositionsFromLastTransition()
    {
        try
        {
            if (RmcPathfinder.IT.LastAction?.Target is Term targetTerm)
            {
                return
                [
                    GetNormalStartPosition(targetTerm),
                    // Make start jumps lower priority here
                    new ArbitraryPosition(RmcPathfinder.StateSync.CurrentState, 0.5f),
                ];
            }

            return GetStartPositions(Utils.CurrentScene(), true);
        }
        catch (Exception e)
        {
            RandoMapCoreMod.Instance.LogError(e);
        }

        return [];
    }

    internal static IEnumerable<Term> GetDestinations(string scene)
    {
        try
        {
            if (RandoMapCoreMod.GS.PathfinderOutOfLogic)
            {
                return GetPrunedPositionsFromScene(RmcPathfinder.PS.LocalPM, scene)
                    .Concat(GetOutOfLogicDestinations(scene))
                    .Distinct();
            }

            return GetPrunedPositionsFromScene(RmcPathfinder.PSNoSequenceBreak.LocalPM, scene);
        }
        catch (Exception e)
        {
            RandoMapCoreMod.Instance.LogError(e);
        }

        return [];
    }

    internal static IEnumerable<string> GetVisitedAdjacentScenes()
    {
        try
        {
            SearchParams sp =
                new()
                {
                    StartPositions = GetStartPositions(Utils.CurrentScene(), false),
                    Destinations = [],
                    MaxCost = 1f,
                    MaxTime = 1000f,
                    DisallowBacktracking = true,
                };

            SearchState ss = new(sp);
            _ = Algorithms.DijkstraSearch(RmcPathfinder.GetLocalPM(), RmcPathfinder.SD, sp, ss);

            HashSet<string> adjacentScenes = [];

            foreach (var node in ss.QueueNodes.Select(qn => qn.node).Concat(ss.TerminalNodes))
            {
                if (
                    node.Actions.FirstOrDefault(a => a.Cost == 1f) is StandardAction action
                    && TransitionData.TryGetScene(action.Target.Name, out var adjacentScene)
                )
                {
                    adjacentScenes.Add(adjacentScene);
                }
            }

            return adjacentScenes;
        }
        catch (Exception e)
        {
            RandoMapCoreMod.Instance.LogError(e);
        }

        return [];
    }

    internal static Position GetNormalStartPosition(Term term)
    {
        return new Position(term, RmcPathfinder.StateSync.CurrentState, 0f);
    }

    internal static ArbitraryPosition GetArbitraryPosition()
    {
        return new ArbitraryPosition(RmcPathfinder.StateSync.CurrentState, -0.5f);
    }

    internal static IEnumerable<Term> GetPrunedPositionsFromScene(ProgressionManager pm, string scene)
    {
        if (!TryGetInLogicPositionsFromScene(pm, scene, out var inLogicPositions) || !inLogicPositions.Any())
        {
            return [];
        }

        // Prune positions that are reachable from another in the same scene (two-way).
        SearchParams sp =
            new()
            {
                StartPositions = inLogicPositions.Select(GetNormalStartPosition),
                Destinations = inLogicPositions,
                MaxCost = 0f,
                MaxDepth = 10,
                MaxTime = 1000f,
                DisallowBacktracking = false,
            };

        SearchState ss = new(sp);
        _ = Algorithms.DijkstraSearch(pm, RmcPathfinder.SD, sp, ss);
        var resultNodes = ss.ResultNodes.Where(n => n.Depth > 0 && n.Start.Term != n.Current.Term);

        List<Term> newPositions = new(inLogicPositions);

        foreach (var node in resultNodes)
        {
            if (!newPositions.Contains(node.Start.Term) || !newPositions.Contains(node.Current.Term))
            {
                continue;
            }

            if (
                resultNodes.Any(n =>
                    node.Start.Term == n.Current.Term
                    && node.Current.Term == n.Start.Term
                    && StateUnion.IsProgressivelyLE(node.Start.States, node.Current.States)
                    && StateUnion.IsProgressivelyLE(n.Start.States, n.Current.States)
                )
            )
            {
                RandoMapCoreMod.Instance.LogFine(
                    $"Pruning {node.Current.Term} which is equivalent to {node.Start.Term}"
                );
                _ = newPositions.Remove(node.Current.Term);
            }
        }

        RandoMapCoreMod.Instance.LogFine($"Remaining: {string.Join(", ", newPositions.Select(p => p.Name))}");

        return newPositions;
    }

    private static bool TryGetInLogicPositionsFromScene(
        ProgressionManager pm,
        string scene,
        out IEnumerable<Term> inLogicPositions
    )
    {
        if (RmcPathfinder.SD.StateTermsByScene.TryGetValue(scene, out var positions))
        {
            inLogicPositions = positions.Where(p => pm.Has(p));
            return true;
        }

        inLogicPositions = default;
        return false;
    }

    private static IEnumerable<Position> GetOutOfLogicStarts(string scene)
    {
        if (!RmcPathfinder.SD.StateTermsByScene.TryGetValue(scene, out var terms))
        {
            return [];
        }

        var oolTerms = terms.Where(t => RandoMapCoreMod.LS.SequenceBreakActions.ContainsKey(t.Name));

        foreach (var t in oolTerms)
        {
            RandoMapCoreMod.Instance.LogFine($"OOL start: {t}");
        }

        return oolTerms.Select(GetNormalStartPosition);
    }

    private static IEnumerable<Term> GetOutOfLogicDestinations(string scene)
    {
        if (!RmcPathfinder.SD.StateTermsByScene.TryGetValue(scene, out var terms))
        {
            return [];
        }

        List<Term> destinations = [];

        foreach (var oolSource in RandoMapCoreMod.LS.SequenceBreakActions.Values.SelectMany(a => a))
        {
            if (RmcPathfinder.SD.TryGetSequenceBreakAction(oolSource, out var action) && terms.Contains(action.Target))
            {
                RandoMapCoreMod.Instance.LogFine($"OOL destination: {action.Target.Name}");
                destinations.Add(action.Target);
            }
        }

        return destinations;
    }
}
