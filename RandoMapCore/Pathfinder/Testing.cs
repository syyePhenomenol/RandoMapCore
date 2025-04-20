using System.Diagnostics;
using RandoMapCore.Transition;
using RandomizerCore.Logic;
using RCPathfinder;

namespace RandoMapCore.Pathfinder;

internal static class Testing
{
    private static readonly Random _rng = new(0);

    internal static void LogProgressionData(RmcSearchData sd)
    {
        RandoMapCoreMod.Instance?.LogDebug($"  Logging PMs");

        foreach (var term in sd.LocalPM.lm.Terms)
        {
            RandoMapCoreMod.Instance.LogDebug($"    {term.Name}");

            if (sd.ReferencePM.lm.Terms.IsTerm(term.Name))
            {
                RandoMapCoreMod.Instance.LogDebug($"      Reference:");

                if (sd.ReferencePM.lm.LogicLookup.TryGetValue(term.Name, out var ld1))
                {
                    RandoMapCoreMod.Instance.LogDebug($"        Logic: {ld1.InfixSource}");
                }

                RandoMapCoreMod.Instance.LogDebug($"        Value: {sd.ReferencePM.Has(term)}");
            }

            RandoMapCoreMod.Instance.LogDebug($"      Local:");

            if (sd.LocalPM.lm.LogicLookup.TryGetValue(term.Name, out var ld2))
            {
                RandoMapCoreMod.Instance.LogDebug($"        Logic: {ld2.InfixSource}");
            }

            RandoMapCoreMod.Instance.LogDebug($"        Value: {sd.LocalPM.Has(term)}");
        }
    }

    internal static void DebugActions(RmcSearchData sd)
    {
        RandoMapCoreMod.Instance?.LogDebug($"  Debug OneToOneActions");

        sd.LocalPM.StartTemp();

        foreach (var kvp in sd.StandardActionLookup)
        {
            foreach (var action in kvp.Value)
            {
                Position start = new(kvp.Key, sd.Updater.CurrentState, 0);
                sd.LocalPM.SetState(start);
                RandoMapCoreMod.Instance?.LogDebug(
                    $"    {action.DebugString}, {action.Cost}: {action.TryDo(new Node(start), sd.LocalPM, out var _)}"
                );
            }
        }

        sd.LocalPM.RemoveTempItems();
    }

    internal static void SingleStartDestinationTest(RmcSearchData sd)
    {
        var inLogicTransitions = sd.GetAllStateTerms()
            .Where(p => TransitionData.GetTransitionDef(p.Name) is not null && sd.LocalPM.Has(p))
            .ToArray();

        SearchParams sp =
            new()
            {
                StartPositions = null,
                Destinations = null,
                MaxCost = 1000f,
                MaxTime = 1000f,
                TerminationCondition = TerminationConditionType.Any,
                DisallowBacktracking = false,
            };

        RandoMapCoreMod.Instance?.LogDebug($"Starting SingleStartDestinationTest:");

        var globalSW = Stopwatch.StartNew();
        var sw = Stopwatch.StartNew();

        var testCount = 100;
        var successes = 0;

        for (var i = 0; i < testCount; i++)
        {
            var start = GetRandomTerm(inLogicTransitions);
            var destination = GetRandomTerm(inLogicTransitions);

            sp.StartPositions =
            [
                new(start, sd.Updater.CurrentState, 0f),
                new ArbitraryPosition(sd.Updater.CurrentState, -0.5f),
            ];
            sp.Destinations = [destination];
            SearchState ss = new(sp);

            RandoMapCoreMod.Instance?.LogDebug($"Trying {start} -> ? -> {destination}");

            sw.Restart();
            if (DoTest(sd, sp, ss))
            {
                RandoMapCoreMod.Instance?.LogDebug($"{ss.NewResultNodes[0].DebugString}");
                successes++;
            }
            else
            {
                RandoMapCoreMod.Instance?.LogDebug($"{start} to {destination} failed");
            }

            sw.Stop();

            var localElapsedMS = sw.ElapsedTicks * 1000f / Stopwatch.Frequency;
            RandoMapCoreMod.Instance?.LogDebug(
                $"Explored {ss.NodesPopped} nodes in {localElapsedMS} ms. Average nodes/ms: {ss.NodesPopped / localElapsedMS}"
            );
        }

        globalSW.Stop();

        var globalElapsedMS = globalSW.ElapsedTicks * 1000f / Stopwatch.Frequency;

        RandoMapCoreMod.Instance?.LogDebug($"Total computation time: {globalElapsedMS} ms");
        RandoMapCoreMod.Instance?.LogDebug($"Total successes: {successes}/{testCount}");
        RandoMapCoreMod.Instance?.LogDebug($"Average serarch time: {globalElapsedMS / testCount} ms");
    }

    internal static void SceneToSceneTest(RmcSearchData sd, SceneLogicTracker slt)
    {
        SearchParams sp =
            new()
            {
                StartPositions = null,
                Destinations = null,
                MaxCost = 1000f,
                MaxTime = 1000f,
                TerminationCondition = TerminationConditionType.Any,
                DisallowBacktracking = false,
            };

        RandoMapCoreMod.Instance?.LogDebug($"Starting SceneToSceneTest:");

        var globalSW = Stopwatch.StartNew();
        var sw = Stopwatch.StartNew();

        var testCount = 100;

        for (var i = 0; i < testCount; i++)
        {
            HashSet<Route> routes = [];

            var startScene = slt.InLogicScenes.ElementAt(_rng.Next(slt.InLogicScenes.Count));
            var destScene = slt.InLogicScenes.ElementAt(_rng.Next(slt.InLogicScenes.Count));

            sp.StartPositions = sd.GetPrunedPositionsFromScene(startScene).Select(sd.GetNormalStartPosition);
            sp.Destinations = sd.GetPrunedPositionsFromScene(destScene);

            if (!sp.StartPositions.Any() || !sp.Destinations.Any())
            {
                RandoMapCoreMod.Instance?.LogDebug($"Invalid scenes {startScene} or {destScene}. Skipping");
                continue;
            }

            SearchState ss = new(sp);

            RandoMapCoreMod.Instance?.LogDebug($"Trying {startScene} -> ? -> {destScene}");

            sw.Restart();
            while (DoTest(sd, sp, ss))
            {
                Route route = new(ss.NewResultNodes[0], null);
                _ = routes.Add(route);
                RandoMapCoreMod.Instance?.LogDebug(
                    $"    {string.Join(", ", route.RemainingInstructions.Select(i => i.SourceText))}"
                );
            }

            sw.Stop();

            var localElapsedMS = sw.ElapsedTicks * 1000f / Stopwatch.Frequency;
            RandoMapCoreMod.Instance?.LogDebug(
                $"Explored {ss.NodesPopped} nodes in {localElapsedMS} ms. Average nodes/ms: {ss.NodesPopped / localElapsedMS}"
            );
        }

        globalSW.Stop();

        var globalElapsedMS = globalSW.ElapsedTicks * 1000f / Stopwatch.Frequency;

        RandoMapCoreMod.Instance?.LogDebug($"Total computation time: {globalElapsedMS} ms");
        RandoMapCoreMod.Instance?.LogDebug($"Average serarch time: {globalElapsedMS / testCount} ms");
    }

    private static bool DoTest(SearchData sd, SearchParams sp, SearchState search)
    {
        if (Algorithms.DijkstraSearch(sd, sp, search))
        {
            RandoMapCoreMod.Instance?.LogDebug($"  Success!");
            return true;
        }

        if (search.QueueNodes.Count > 0)
        {
            RandoMapCoreMod.Instance?.LogDebug($"  Search terminated after reaching max cost.");
        }
        else
        {
            RandoMapCoreMod.Instance?.LogDebug($"  Search exhausted with no route found.");
        }

        return false;
    }

    private static Term GetRandomTerm(Term[] terms)
    {
        return terms[_rng.Next(terms.Length)];
    }
}
