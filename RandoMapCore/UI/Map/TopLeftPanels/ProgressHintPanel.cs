using MagicUI.Core;
using MagicUI.Elements;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Localization;
using RandoMapCore.Settings;
using RandomizerCore.Extensions;
using RandomizerCore.Logic;

namespace RandoMapCore.UI;

internal class ProgressHintPanel : WorldMapPanel
{
    private TextObject _text;
    private PlacementProgressHint _selectedHint;
    private bool _progressionUnlockingHint;

    protected override Panel Build(WorldMapLayout layout)
    {
        Instance = this;
        var panel = base.Build(layout);

        _text = new(layout, $"{Name} Text")
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = HorizontalAlignment.Left,
            Font = MagicUI.Core.UI.Perpetua,
            Padding = new(0f, 2f, 0f, 2f),
        };

        panel.Child = _text;

        return panel;
    }

    internal static ProgressHintPanel Instance { get; private set; }

    public override void Update()
    {
        base.Update();

        if (RandoMapCoreMod.GS.ProgressHint is not ProgressHintSetting.Off)
        {
            Panel.Visibility = Visibility.Visible;
        }
        else
        {
            Panel.Visibility = Visibility.Collapsed;
        }

        if (_selectedHint is not null && !_selectedHint.IsPlacementObtained())
        {
            UpdateHintText();
        }
        else
        {
            _selectedHint = null;
            _text.Text =
                $"{"Press".L()} {ProgressHintInput.Instance.GetBindingsText()} {"to reveal progress hint".L()}.";
        }

        _text.FontSize = (int)(20f * MapChangerMod.GS.UIScale);
        _text.MaxWidth = (int)(450f * MapChangerMod.GS.UIScale);
    }

    public override void Destroy()
    {
        _text?.Destroy();
        base.Destroy();
        Instance = null;
    }

    internal void UpdateNewHint()
    {
        UpdateLocation();
        UpdateHintText();
        _selectedHint?.DoPanning();
    }

    // Randomly selects a reachable location or transition that unlocks further progression.
    // Priority goes:
    // 1. Random placement that leads to new reachable placements
    // 2. Random placement that leads to new state terms
    // 3. Random placement that leads to new non-state terms
    // 4. Random placement of lowest inner sphere
    private void UpdateLocation()
    {
        RandoMapCoreMod.Instance.LogFine("Update progress hint location");

        var lm = RandoMapCoreMod.Data.PM.lm;
        ProgressionManager pm = new(lm, RandoMapCoreMod.Data.Context);
        foreach (var term in lm.Terms)
        {
            switch (term.Type)
            {
                case TermType.State:
                    pm.SetState(term, RandoMapCoreMod.Data.PMNoSequenceBreak.GetState(term));
                    break;
                default:
                    pm.Set(term, RandoMapCoreMod.Data.PMNoSequenceBreak.Get(term));
                    break;
            }
        }

        var updater = new ProgressHintLogicUpdater(pm);

        Random rng = new();

        // Item placements that are reachable and haven't been obtained yet (without breaking logic)
        var relevantItemPlacements = ItemChanger.Internal.Ref.Settings.Placements.Values.SelectMany(p =>
            p.Items.Select(i => new ItemPlacementHint(RandoMapCoreMod.Data.GetItemRandoPlacement(i), p, i))
                .Where(h =>
                    h.RandoPlacement != default && h.RandoPlacement.Location.CanGet(pm) && !h.IsPlacementObtained()
                )
        );

        // Transition placements that are unchecked and reachable (without breaking logic)
        var relevantTransitionPlacements = RandoMapCoreMod
            .Data.RandomizedPlacements.Where(tp =>
                RandoMapCoreMod.Data.UncheckedReachableTransitionsNoSequenceBreak.Contains(tp.Location.Name)
            )
            .Select(tp => new TransitionPlacementHint(tp, RandoMapCoreMod.Data.RandomizedTransitions[tp.Location.Name]))
            .Where(tp => tp.RandoPlacement.Item is not null && tp.RandoPlacement.Location is not null);

        // Make existing hint lowest in priority
        var relevantPlacements = rng.Permute(
                relevantItemPlacements.Select(ip => (PlacementProgressHint)ip).Concat(relevantTransitionPlacements)
            )
            .OrderBy(h => h.RandoPlacement.Location.Name == _selectedHint?.RandoPlacement.Location?.Name);

        PlacementProgressHint newStateTermHint = null;
        PlacementProgressHint newNonStateTermHint = null;

        foreach (var h in relevantPlacements)
        {
            updater.Test(h);

            if (updater.NewReachablePlacement)
            {
                updater.StopUpdating();
                RandoMapCoreMod.Instance.LogFine($"New reachable placements from {h.RandoPlacement.Location.Name}");
                _selectedHint = h;
                _progressionUnlockingHint = true;
                return;
            }

            if (updater.NewStateTerms && newStateTermHint is null)
            {
                newStateTermHint = h;
                continue;
            }

            if (updater.NewNonStateTerms && newNonStateTermHint is null)
            {
                newNonStateTermHint = h;
            }
        }

        updater.StopUpdating();

        if (newStateTermHint is not null)
        {
            RandoMapCoreMod.Instance.LogFine($"New state terms from {newStateTermHint.RandoPlacement.Location.Name}");
            _selectedHint = newStateTermHint;
            _progressionUnlockingHint = true;
            return;
        }
        else if (newNonStateTermHint is not null)
        {
            RandoMapCoreMod.Instance.LogFine(
                $"New non-state terms from {newNonStateTermHint.RandoPlacement.Location.Name}"
            );
            _selectedHint = newNonStateTermHint;
            _progressionUnlockingHint = true;
            return;
        }

        // Make lower sphere locations higher priority
        _selectedHint = relevantPlacements
            .OrderBy(h => h.RandoPlacement.Location.Name == _selectedHint?.RandoPlacement.Location?.Name)
            .ThenBy(h => h.RandoPlacement.Location.Sphere)
            .FirstOrDefault();
        _progressionUnlockingHint = false;
    }

    private void UpdateHintText()
    {
        if (_selectedHint is null)
        {
            _text.Text = "Current reachable locations/transitions will not unlock any more progress.".L();
            return;
        }

        var textFragment =
            $"{_selectedHint.GetTextFragment()}.\n\n{"Press".L()} {ProgressHintInput.Instance.GetBindingsText()} {"to refresh hint".L()}.";

        if (_progressionUnlockingHint)
        {
            _text.Text = $"{"You will find progress".L()}" + textFragment;
        }
        else
        {
            _text.Text = $"{"You might find progress".L()}" + textFragment;
        }
    }

    protected override UnityEngine.Color GetBackgroundTint()
    {
        return RmcColors.GetColor(RmcColorSetting.UI_Borders);
    }
}
