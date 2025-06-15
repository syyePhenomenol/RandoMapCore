using MapChanger;
using RandoMapCore.Settings;
using RandoMapCore.UI;
using RCPathfinder;
using RCPathfinder.Actions;
using JU = RandomizerCore.Json.JsonUtil;

namespace RandoMapCore.Pathfinder;

internal class RouteManager
{
    private readonly Dictionary<(string, string), RouteHint> _routeHints;

    private SearchParams _sp;
    private SearchState _ss;
    private HashSet<Route> _routes;
    private string _startScene;
    private string _finalScene;
    private bool _reevaluated;

    internal RouteManager()
    {
        _routeHints = JU.DeserializeFromEmbeddedResource<RouteHint[]>(
                RandoMapCoreMod.Assembly,
                "RandoMapCore.Resources.Pathfinder.routeHints.json"
            )
            .SelectMany(rh => rh.TermPairs.Select(tp => (tp, rh)))
            .ToDictionary(pair => pair.tp, pair => pair.rh);
    }

    internal Route CurrentRoute { get; private set; }

    internal bool CanCycleRoute(string scene)
    {
        return !_reevaluated
            && Utils.CurrentScene() == _startScene
            && scene == _finalScene
            && CurrentRoute is not null
            && CurrentRoute.NotStarted;
    }

    internal bool TryGetNextRouteTo(string scene)
    {
        // Reset
        if (!CanCycleRoute(scene))
        {
            _startScene = Utils.CurrentScene();
            _finalScene = scene;

            _sp = new()
            {
                StartPositions = PositionHelper.GetStartPositionsInCurrentScene(false),
                Destinations = PositionHelper.GetTermsInScene(scene),
                MaxCost = 100f,
                MaxTime = 1000f,
                TerminationCondition = TerminationConditionType.Any,
                Stateless = _ss is not null && _ss.HasTimedOut,
                DisallowBacktracking = false,
            };

            if (!_sp.StartPositions.Any() || !_sp.Destinations.Any())
            {
                ResetRoute();
                return false;
            }

            _ss = new(_sp);
            _routes = [];
        }

        _reevaluated = false;

        try
        {
            while (Algorithms.DijkstraSearch(RmcPathfinder.GetLocalPM(), RmcPathfinder.SD, _sp, _ss))
            {
                var route = GetRoute(_ss.NewResultNodes.First());

                if (
                    route.FinishedOrEmpty
                    || _routes.Any(r =>
                        r.FirstInstruction.Equals(route.FirstInstruction)
                        && r.LastInstruction.Equals(route.LastInstruction)
                    )
                )
                {
                    RandoMapCoreMod.Instance.LogFine($"Redundant route: {route.Node.DebugString}");
                    continue;
                }

                LogRouteResults(route);
                _ = _routes.Add(route);
                CurrentRoute = route;
                RouteCompass.Update();
                UpdateRouteUI();
                return true;
            }
        }
        catch (Exception e)
        {
            RandoMapCoreMod.Instance.LogError(e);
        }

        // Search exhausted, clear search state and reset
        ResetRoute();
        return false;
    }

    internal void CheckRoute(ItemChanger.Transition lastTransition)
    {
        // RandoMapCoreMod.Instance.LogFine($"Last transition: {lastTransition}");

        // if (!RmcPathfinder.LE.LocalLM.Terms.IsTerm(lastTransition.ToString()))
        // {
        //     RandoMapCoreMod.Instance.LogFine("Transition not in LM");
        // }

        if (CurrentRoute is null)
        {
            return;
        }

        if (CurrentRoute.CheckCurrentInstruction(lastTransition))
        {
            if (CurrentRoute.FinishedOrEmpty)
            {
                ResetRoute();
            }

            UpdateRouteUI();
            return;
        }

        // The transition doesn't match the route's instruction
        switch (RandoMapCoreMod.GS.WhenOffRoute)
        {
            case OffRouteBehaviour.CancelRoute:
                ResetRoute();
                break;
            case OffRouteBehaviour.Reevaluate:
                ReevaluateRoute(lastTransition);
                UpdateRouteUI();
                break;
            case OffRouteBehaviour.KeepRoute:
                break;
            default:
                break;
        }
    }

    private void ReevaluateRoute(ItemChanger.Transition transition)
    {
        _startScene = transition.SceneName;

        _sp = new()
        {
            StartPositions = PositionHelper.GetStartPositionsInCurrentScene(true),
            Destinations = [CurrentRoute.Node.Current.Term],
            MaxCost = 100f,
            MaxTime = 1000f,
            TerminationCondition = TerminationConditionType.Any,
            DisallowBacktracking = false,
        };

        if (!_sp.StartPositions.Any())
        {
            ResetRoute();
            return;
        }

        _ss = new(_sp);

        try
        {
            if (Algorithms.DijkstraSearch(RmcPathfinder.GetLocalPM(), RmcPathfinder.SD, _sp, _ss))
            {
                var route = GetRoute(_ss.NewResultNodes.First());

                if (route.FinishedOrEmpty)
                {
                    ResetRoute();
                    return;
                }

                LogRouteResults(route);

                CurrentRoute = route;
                _reevaluated = true;
                return;
            }
        }
        catch (Exception e)
        {
            RandoMapCoreMod.Instance.LogError(e);
        }

        ResetRoute();
    }

    internal void ResetRoute()
    {
        CurrentRoute = null;
        _sp = null;
        _ss = null;
        _routes = null;
        _startScene = null;
        _finalScene = null;
        _reevaluated = false;

        RouteCompass.Update();
        UpdateRouteUI();

        RandoMapCoreMod.Instance.LogFine("Reset route.");
    }

    private void UpdateRouteUI()
    {
        RmcUIManager.WorldMap?.Update();
        RmcUIManager.Route?.Update();
    }

    private Route GetRoute(Node node)
    {
        List<RouteHint> routeHints = [];
        foreach (var a in node.Actions.Where(a => a is StandardAction).Cast<StandardAction>())
        {
            if (_routeHints.TryGetValue((a.Source.Name, a.Target.Name), out var rh))
            {
                routeHints.Add(rh);
            }
        }

        return new Route(node, routeHints.Distinct());
    }

    private void LogRouteResults(Route route)
    {
        RandoMapCoreMod.Instance.LogFine($"Found a route from {route.Node.Start.Term} to {route.Node.Current.Term}:");
        RandoMapCoreMod.Instance.LogFine(route.Node.DebugString);
        RandoMapCoreMod.Instance.LogFine($"Node states count: {route.Node.Current.States.Count()}");
        RandoMapCoreMod.Instance.LogFine($"Stateless search: {_sp?.Stateless}");
        RandoMapCoreMod.Instance.LogFine($"Cumulative search time: {_ss?.SearchTime} ms");
    }
}
