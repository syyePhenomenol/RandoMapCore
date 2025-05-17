using MapChanger;
using MapChanger.MonoBehaviours;
using RandoMapCore.Modes;
using RandoMapCore.UI;

namespace RandoMapCore.Pins;

internal class PinSelector : Selector
{
    internal GridPinRoomHighlighter Highlighter { get; private set; }

    public override void Initialize(IEnumerable<ISelectable> pins)
    {
        base.Initialize(pins);

        Highlighter = Utils.MakeMonoBehaviour<GridPinRoomHighlighter>(gameObject, "Highlighter");
        Highlighter.gameObject.SetActive(true);

        ActiveModifiers.AddRange([ActiveByCurrentMode, ActiveByToggle]);
        SpriteObject.SetActive(true);
    }

    public override void OnMainUpdate(bool active)
    {
        base.OnMainUpdate(active);

        if (active)
        {
            Highlighter.StartAnimateHighlightedRooms();
        }
        else
        {
            Highlighter.StopAnimateHighlightedRooms();
        }
    }

    protected override void Select(ISelectable selectable)
    {
        selectable.Selected = true;

        if (selectable is GridPin gridPin)
        {
            Highlighter.SelectedGridPin = gridPin;
        }
        else if (selectable is PinCluster pinCluster)
        {
            pinCluster.UpdateSelectablePins();
        }
    }

    protected override void Deselect(ISelectable selectable)
    {
        selectable.Selected = false;

        Highlighter.SelectedGridPin = null;

        if (selectable is PinCluster pc)
        {
            pc.ResetSelectionIndex();
        }
    }

    protected override void OnSelectionChanged()
    {
        PinSelectionPanel.Instance?.HideHint();
        RmcUIManager.WorldMap?.Update();
    }

    internal string GetSelectionText()
    {
        return SelectedObject switch
        {
            PinCluster pinCluster => pinCluster.GetText(),
            GridPin gridPin => gridPin.GetText(LockSelection),
            RmcPin rmcPin => rmcPin.GetText(),
            _ => null,
        };
    }

    private bool ActiveByCurrentMode()
    {
        return Conditions.RandoCoreMapModEnabled();
    }

    private bool ActiveByToggle()
    {
        return RandoMapCoreMod.GS.PinSelectionOn;
    }

    internal bool VisitedBenchSelected()
    {
        return Interop.HasBenchwarp && BenchwarpInterop.IsVisitedBench(SelectedObject?.Key);
    }

    internal bool VisitedBenchNotSelected()
    {
        return Interop.HasBenchwarp && !BenchwarpInterop.IsVisitedBench(SelectedObject?.Key);
    }
}
