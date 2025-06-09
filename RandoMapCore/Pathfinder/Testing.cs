using System.Diagnostics;
using RandoMapCore.Transition;
using RandomizerCore.Logic;
using RCPathfinder;

namespace RandoMapCore.Pathfinder;

internal static class Testing
{
    private static readonly Random _rng = new(0);

    internal static void LogProgressionData()
    {
        RandoMapCoreMod.Instance?.LogDebug($"  Logging PMs");

        foreach (var term in RmcPathfinder.LE.LocalLM.Terms)
        {
            RandoMapCoreMod.Instance.LogDebug($"    {term.Name}");

            if (RmcPathfinder.LE.ReferenceLM.Terms.IsTerm(term.Name))
            {
                RandoMapCoreMod.Instance.LogDebug($"      Reference:");

                if (RmcPathfinder.LE.ReferenceLM.LogicLookup.TryGetValue(term.Name, out var ld1))
                {
                    RandoMapCoreMod.Instance.LogDebug($"        Logic: {ld1.InfixSource}");
                }

                RandoMapCoreMod.Instance.LogDebug($"        PM Value: {RmcPathfinder.PS.ReferencePM.Has(term)}");
                RandoMapCoreMod.Instance.LogDebug(
                    $"        PMNoSequenceBreak Value: {RmcPathfinder.PSNoSequenceBreak.ReferencePM.Has(term)}"
                );
            }

            RandoMapCoreMod.Instance.LogDebug($"      Local:");

            if (RmcPathfinder.LE.LocalLM.LogicLookup.TryGetValue(term.Name, out var ld2))
            {
                RandoMapCoreMod.Instance.LogDebug($"        Logic: {ld2.InfixSource}");
            }

            RandoMapCoreMod.Instance.LogDebug($"        PM Value: {RmcPathfinder.PS.LocalPM.Has(term)}");
            RandoMapCoreMod.Instance.LogDebug(
                $"        PMNoSequenceBreak Value: {RmcPathfinder.PSNoSequenceBreak.LocalPM.Has(term)}"
            );
        }
    }

    internal static void DebugActions()
    {
        RandoMapCoreMod.Instance?.LogDebug($"  Debug OneToOneActions");

        RmcPathfinder.PS.LocalPM.StartTemp();

        foreach (var kvp in RmcPathfinder.SD.StandardActionLookup)
        {
            foreach (var action in kvp.Value)
            {
                var start = kvp.Key.ToStartPosition(0f);
                RmcPathfinder.PS.LocalPM.SetState(start);
                RandoMapCoreMod.Instance?.LogDebug(
                    $"    {action.DebugString}, {action.Cost}: {action.TryDo(new Node(start), RmcPathfinder.PS.LocalPM, out var _)}"
                );
            }
        }

        RmcPathfinder.PS.LocalPM.RemoveTempItems();
    }

    internal static void SingleStartDestinationTest()
    {
        var inLogicTransitions = RmcPathfinder
            .SD.GetAllStateTerms()
            .Where(p => TransitionData.GetTransitionDef(p.Name) is not null && RmcPathfinder.PS.LocalPM.Has(p))
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

        var pm = RmcPathfinder.PS.LocalPM;
        var sd = RmcPathfinder.SD;

        for (var i = 0; i < testCount; i++)
        {
            var start = GetRandomTerm(inLogicTransitions);
            var destination = GetRandomTerm(inLogicTransitions);

            sp.StartPositions = [start.ToStartPosition(0f), PositionHelper.GetArbitraryPosition(-0.5f)];
            sp.Destinations = [destination];
            SearchState ss = new(sp);

            RandoMapCoreMod.Instance?.LogDebug($"Trying {start} -> ? -> {destination}");

            sw.Restart();
            if (DoTest(pm, sd, sp, ss))
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

    internal static void SceneToSceneTest()
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

        var pm = RmcPathfinder.PS.LocalPM;
        var sd = RmcPathfinder.SD;

        for (var i = 0; i < testCount; i++)
        {
            HashSet<Route> routes = [];

            var startScene = RmcPathfinder.Slt.InLogicScenes.ElementAt(
                _rng.Next(RmcPathfinder.Slt.InLogicScenes.Count)
            );
            var destScene = RmcPathfinder.Slt.InLogicScenes.ElementAt(_rng.Next(RmcPathfinder.Slt.InLogicScenes.Count));

            sp.StartPositions = PositionHelper
                .GetNonEquivalentTermsInScene(startScene)
                .Select(t => t.ToStartPosition(0f));
            sp.Destinations = PositionHelper.GetTermsInScene(destScene);

            if (!sp.StartPositions.Any() || !sp.Destinations.Any())
            {
                RandoMapCoreMod.Instance?.LogDebug($"Invalid scenes {startScene} or {destScene}. Skipping");
                continue;
            }

            SearchState ss = new(sp);

            RandoMapCoreMod.Instance?.LogDebug($"Trying {startScene} -> ? -> {destScene}");

            sw.Restart();
            while (DoTest(pm, sd, sp, ss))
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

    private static bool DoTest(ProgressionManager pm, SearchData sd, SearchParams sp, SearchState search)
    {
        if (Algorithms.DijkstraSearch(pm, sd, sp, search))
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
