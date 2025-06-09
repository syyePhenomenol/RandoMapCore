using MapChanger;
using RandoMapCore.Settings;
using RandoMapCore.Transition;
using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;
using RCPathfinder;
using RCPathfinder.Actions;

namespace RandoMapCore.Pathfinder;

internal static class PositionHelper
{
    internal static IEnumerable<Term> GetTermsInScene(string scene)
    {
        if (RmcPathfinder.SD.StateTermsByScene.TryGetValue(scene, out var sceneTerms))
        {
            return sceneTerms;
        }

        return [];
    }

    internal static IEnumerable<Position> GetStartPositionsInCurrentScene(bool lastActionOnly)
    {
        try
        {
            var lastActionTerm = RmcPathfinder.IT.LastAction?.Target;
            List<Position> startPositions = lastActionTerm is not null ? [lastActionTerm.ToStartPosition(0f)] : [];

            // Prioritize direct backtrack through last transition for single action routes
            startPositions.Add(GetArbitraryPosition(0.5f));

            if (lastActionOnly && lastActionTerm is not null)
            {
                return startPositions;
            }

            // Make other terms in scene not logically equivalent to the last action much lower in priority
            // (search is exhausted from last action and start jumps before considering these other "currently unreachable" terms)
            startPositions.AddRange(
                GetNonEquivalentTermsInScene(Utils.CurrentScene(), lastActionTerm).Select(t => t.ToStartPosition(50f))
            );

            return startPositions;
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
                    StartPositions = GetRelevantStartTermsInScene(Utils.CurrentScene())
                        .Select(t => t.ToStartPosition(0f)),
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

    internal static Position ToStartPosition(this Term term, float cost)
    {
        return new Position(term, RmcPathfinder.StateSync.CurrentState, cost);
    }

    internal static Position GetArbitraryPosition(float cost)
    {
        return new ArbitraryPosition(RmcPathfinder.StateSync.CurrentState, cost);
    }

    // Returns all terms in the scene that are not logically equivalent to each other.
    // Does not include any terms that are logically equivalent to priorityTerm.
    internal static IEnumerable<Term> GetNonEquivalentTermsInScene(string scene, Term priorityTerm = null)
    {
        HashSet<Term> relevantTerms = priorityTerm is not null ? [priorityTerm] : [];

        relevantTerms.UnionWith(GetRelevantStartTermsInScene(scene));

        if (!relevantTerms.Any())
        {
            return [];
        }

        SearchParams sp =
            new()
            {
                StartPositions = relevantTerms.Select(t => t.ToStartPosition(0f)),
                Destinations = relevantTerms,
                MaxCost = 0f,
                MaxDepth = 10,
                MaxTime = 1000f,
                DisallowBacktracking = false,
            };

        SearchState ss = new(sp);
        _ = Algorithms.DijkstraSearch(RmcPathfinder.GetLocalPM(), RmcPathfinder.SD, sp, ss);
        var resultNodes = ss.ResultNodes.Where(n => n.Depth > 0 && n.Start.Term != n.Current.Term);

        HashSet<Term> newTerms = new(relevantTerms);
        HashSet<(Term, Term)> reachableTermPairs = [];

        foreach (var node in resultNodes)
        {
            if (!newTerms.Contains(node.Start.Term) || !newTerms.Contains(node.Current.Term))
            {
                continue;
            }

            if (StateUnion.IsProgressivelyLE(node.Start.States, node.Current.States))
            {
                reachableTermPairs.Add((node.Start.Term, node.Current.Term));

                // The start and destination of the node are logically equivalent
                if (reachableTermPairs.Contains((node.Current.Term, node.Start.Term)))
                {
                    if (node.Start.Term == priorityTerm)
                    {
                        newTerms.Remove(node.Current.Term);
                    }
                    else
                    {
                        newTerms.Remove(node.Start.Term);
                    }
                }
            }
        }

        newTerms.Remove(priorityTerm);
        RandoMapCoreMod.Instance.LogFine($"Remaining: {string.Join(", ", newTerms.Select(p => p.Name))}");
        return newTerms;
    }

    // Gather all relevant terms in scene depending on if OOL search is used or not
    private static IEnumerable<Term> GetRelevantStartTermsInScene(string scene)
    {
        var pm = RmcPathfinder.GetLocalPM();

        return
        [
            .. GetTermsInScene(scene)
                .Where(t =>
                    pm.Has(t)
                    || (
                        RandoMapCoreMod.GS.PathfinderSequenceBreaks is SequenceBreakSetting.SuperSequenceBreaks
                        && RandoMapCoreMod.LS.SuperSequenceBreaks.ContainsKey(t.Name)
                    )
                ),
        ];
    }
}
