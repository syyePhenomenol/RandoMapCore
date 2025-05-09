using MagicUI.Core;
using MagicUI.Elements;
using MagicUI.Graphics;
using MapChanger;
using RandoMapCore.Localization;
using RandoMapCore.Settings;
using RandomizerCore.Extensions;
using RandomizerCore.Logic;

namespace RandoMapCore.UI;

internal class ProgressHintPanel
{
    private readonly Panel _progressHintPanel;
    private readonly TextObject _progressHintText;
    private PlacementProgressHint _selectedHint;
    private bool _progressionUnlockingHint;

    internal ProgressHintPanel(LayoutRoot layout, StackLayout panelStack)
    {
        Instance = this;

        _progressHintPanel = new(
            layout,
            SpriteManager.Instance.GetTexture("GUI.PanelLeft").ToSlicedSprite(200f, 50f, 100f, 50f),
            "Progress Hint Panel"
        )
        {
            MinWidth = 0f,
            MinHeight = 0f,
            Borders = new(10f, 20f, 30f, 20f),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        panelStack.Children.Add(_progressHintPanel);

        ((Image)layout.GetElement("Progress Hint Panel Background")).Tint = RmcColors.GetColor(
            RmcColorSetting.UI_Borders
        );

        _progressHintText = new(layout, "Progress Hint Panel Text")
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = HorizontalAlignment.Left,
            Font = MagicUI.Core.UI.Perpetua,
            FontSize = 20,
            Padding = new(0f, 2f, 0f, 2f),
            MaxWidth = 450f,
        };

        _progressHintPanel.Child = _progressHintText;
    }

    internal static ProgressHintPanel Instance { get; private set; }

    internal void Update()
    {
        if (RandoMapCoreMod.GS.ProgressHint is not ProgressHintSetting.Off)
        {
            _progressHintPanel.Visibility = Visibility.Visible;
        }
        else
        {
            _progressHintPanel.Visibility = Visibility.Collapsed;
        }

        if (_selectedHint is not null && !_selectedHint.IsPlacementObtained())
        {
            UpdateHintText();
        }
        else
        {
            _selectedHint = null;
            _progressHintText.Text =
                $"{"Press".L()} {ProgressHintInput.Instance.GetBindingsText()} {"to reveal progress hint".L()}.";
        }
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
            _progressHintText.Text = "Current reachable locations/transitions will not unlock any more progress.".L();
            return;
        }

        var textFragment =
            $"{_selectedHint.GetTextFragment()}.\n\n{"Press".L()} {ProgressHintInput.Instance.GetBindingsText()} {"to refresh hint".L()}.";

        if (_progressionUnlockingHint)
        {
            _progressHintText.Text = $"{"You will find progress".L()}" + textFragment;
        }
        else
        {
            _progressHintText.Text = $"{"You might find progress".L()}" + textFragment;
        }
    }
}
