using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;

namespace RandoMapCore.UI;

internal class ProgressHintLogicUpdater : MainUpdater
{
    private readonly ProgressionManager _pm;
    private readonly bool _initialized;

    internal ProgressHintLogicUpdater(ProgressionManager pm)
        : base(pm.lm, pm)
    {
        _pm = pm;

        foreach (var w in pm.lm.Waypoints)
        {
            if (w.term.Type == TermType.State)
            {
                AddEntry(new TrackedStateUpdateEntry(this, w.term, w.logic));
            }
            else
            {
                AddEntry(new TrackedPreplacedItemUpdateEntry(this, w, w));
            }
        }

        foreach (var t in pm.lm.TransitionLookup.Values)
        {
            if (t.term.Type == TermType.State)
            {
                AddEntry(new TrackedStateUpdateEntry(this, t.term, t.logic));
            }
        }

        AddEntries(RandoMapCoreMod.Data.RandomizedPlacements.Select(p => new RandomizedPlacementUpdateEntry(this, p)));

        AddEntries(RandoMapCoreMod.Data.VanillaPlacements.Select(p => new VanillaPlacementUpdateEntry(this, p)));

        StartUpdating();

        _initialized = true;
    }

    internal bool NewReachablePlacement { get; private set; }
    internal bool NewStateTerms { get; private set; }
    internal bool NewNonStateTerms { get; private set; }

    internal void Test(PlacementProgressHint hint)
    {
        NewReachablePlacement = false;
        NewStateTerms = false;
        NewNonStateTerms = false;

        _pm.StartTemp();
        _pm.Add(hint.RandoPlacement.Item, hint.RandoPlacement.Location);
        _pm.RemoveTempItems();
    }

    private class TrackedStateUpdateEntry(ProgressHintLogicUpdater updater, Term term, StateLogicDef logic)
        : StateUpdateEntry(term, logic)
    {
        private readonly List<State> _stateAccumulator = [];

        public override void Update(ProgressionManager pm, int updateTerm)
        {
            if (!updater._initialized || updater.NewReachablePlacement)
            {
                return;
            }

            var state = pm.GetState(term);
            _stateAccumulator.Clear();
            if (logic.CheckForUpdatedState(pm, state, _stateAccumulator, updateTerm, out var newState))
            {
                stateSetter.value = newState;
                pm.Add(stateSetter);
                updater.NewStateTerms = true;
                // RandoMapCore.Instance.LogFine($"Adding state term: {term}");
            }
        }

        public override void Update(ProgressionManager pm)
        {
            if (!updater._initialized || updater.NewReachablePlacement)
            {
                return;
            }

            var state = pm.GetState(term);
            _stateAccumulator.Clear();
            if (logic.CheckForUpdatedState(pm, state, _stateAccumulator, out var newState))
            {
                stateSetter.value = newState;
                // RandoMapCore.Instance.LogFine($"Adding state term: {term}");
                pm.Add(stateSetter);
                updater.NewStateTerms = true;
            }
            else if (state is null && logic.CanGet(pm))
            {
                stateSetter.value = pm.lm.StateManager.Empty;
                // RandoMapCore.Instance.LogFine($"Adding state term: {term}");
                pm.Add(stateSetter);
                updater.NewStateTerms = true;
            }
        }
    }

    private class TrackedPreplacedItemUpdateEntry(ProgressHintLogicUpdater updater, ILogicItem item, ILogicDef location)
        : PrePlacedItemUpdateEntry(item, location)
    {
        public override void OnAdd(ProgressionManager pm)
        {
            if (!updater._initialized || updater.NewReachablePlacement)
            {
                return;
            }

            if (pm.lm.Terms.IsTerm(location.Name) && pm.Get(location.Name) is 0)
            {
                // RandoMapCore.Instance.LogFine($"Adding non-state term: {location.Name}");
                updater.NewNonStateTerms = true;
            }

            base.OnAdd(pm);
        }
    }

    public abstract class GeneralizedPlacementUpdateEntry(GeneralizedPlacement gp) : UpdateEntry
    {
        private protected GeneralizedPlacement GP { get; } = gp;

        public override bool CanGet(ProgressionManager pm)
        {
            return GP.Location.CanGet(pm);
        }

        public override IEnumerable<Term> GetTerms()
        {
            return GP.Location.GetTerms();
        }
    }

    public class RandomizedPlacementUpdateEntry(ProgressHintLogicUpdater updater, RandoPlacement rp)
        : GeneralizedPlacementUpdateEntry(rp)
    {
        public override void OnAdd(ProgressionManager pm)
        {
            if (!updater._initialized || updater.NewReachablePlacement)
            {
                return;
            }

            if (pm.lm.Terms.IsTerm(GP.Location.Name) && pm.Get(GP.Location.Name) is 0)
            {
                // RandoMapCore.Instance.LogFine($"New reachable placement {GP.Location.Name}");
                updater.NewReachablePlacement = true;
            }
        }
    }

    public class VanillaPlacementUpdateEntry(ProgressHintLogicUpdater updater, GeneralizedPlacement gp)
        : GeneralizedPlacementUpdateEntry(gp)
    {
        public override void OnAdd(ProgressionManager pm)
        {
            if (!updater._initialized || updater.NewReachablePlacement)
            {
                return;
            }

            // RandoMapCore.Instance.LogFine($"Adding vanilla placement: {GP.Location.Name}");
            pm.Add(GP.Item, GP.Location);

            if (GP.Location is ILocationWaypoint ilw)
            {
                // RandoMapCore.Instance.LogFine($"Adding reachable effect: {GP.Location.Name}");
                pm.Add(ilw.GetReachableEffect());
            }
        }
    }
}
