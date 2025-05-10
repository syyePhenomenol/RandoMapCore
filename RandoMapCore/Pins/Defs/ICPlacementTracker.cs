using System.Collections.ObjectModel;
using ItemChanger;
using RandoMapCore.Settings;
using SM = ConnectionMetadataInjector.SupplementalMetadata;

namespace RandoMapCore.Pins;

internal class ICPlacementTracker(AbstractPlacement placement)
{
    private readonly List<AbstractItem> _neverObtainedPreviewableItems = [];
    private readonly List<AbstractItem> _neverObtainedUnpreviewableItems = [];
    private readonly List<AbstractItem> _everObtainedPersistentItems = [];
    private readonly List<AbstractItem> _everObtainedNonPersistentItems = [];
    private readonly List<AbstractItem> _activeItems = [];

    private bool _enqueueUpdateItems;

    internal PlacementState State { get; private set; }

    // The following should all be mutually exclusive and sum to all items
    internal ReadOnlyCollection<AbstractItem> NeverObtainedPreviewableItems => new(_neverObtainedPreviewableItems);
    internal ReadOnlyCollection<AbstractItem> NeverObtainedUnpreviewableItems => new(_neverObtainedUnpreviewableItems);
    internal ReadOnlyCollection<AbstractItem> EverObtainedPersistentItems => new(_everObtainedPersistentItems);
    internal ReadOnlyCollection<AbstractItem> EverObtainedNonPersistentItems => new(_everObtainedNonPersistentItems);

    internal ReadOnlyCollection<AbstractItem> ActiveItems => new(_activeItems);

    internal void EnqueueUpdateItems()
    {
        _enqueueUpdateItems = true;
    }

    internal void Update()
    {
        if (_enqueueUpdateItems)
        {
            UpdateItems();
            _enqueueUpdateItems = false;
        }

        UpdateActiveItems();
    }

    private void UpdateItems()
    {
        _neverObtainedPreviewableItems.Clear();
        _neverObtainedUnpreviewableItems.Clear();
        _everObtainedPersistentItems.Clear();
        _everObtainedNonPersistentItems.Clear();

        var placementPreviewed = placement.CanPreview() && placement.CheckVisitedAny(VisitState.Previewed);

        foreach (var i in placement.Items)
        {
            if (!i.WasEverObtained())
            {
                if ((placementPreviewed && i.CanPreview()) || SM.Of(i).Get(InteropProperties.ForceEnablePreview))
                {
                    _neverObtainedPreviewableItems.Add(i);
                }
                else
                {
                    _neverObtainedUnpreviewableItems.Add(i);
                }
            }
            else
            {
                if (i.IsPersistent() && !i.IsObtained())
                {
                    _everObtainedPersistentItems.Add(i);
                }
                else
                {
                    _everObtainedNonPersistentItems.Add(i);
                }
            }
        }
    }

    private void UpdateActiveItems()
    {
        _activeItems.Clear();

        if (_neverObtainedPreviewableItems.Any())
        {
            State = PlacementState.Previewable;
            _activeItems.AddRange(_neverObtainedPreviewableItems);
        }
        else if (_neverObtainedUnpreviewableItems.Any())
        {
            State = PlacementState.NotCleared;
            if (RandoMapCoreMod.Data.EnableSpoilerToggle && RandoMapCoreMod.LS.SpoilerOn)
            {
                _activeItems.AddRange(_neverObtainedPreviewableItems);
                _activeItems.AddRange(_neverObtainedUnpreviewableItems);
            }
        }
        else if (_everObtainedPersistentItems.Any())
        {
            State = PlacementState.ClearedPersistent;
            if (RandoMapCoreMod.GS.ShowClearedPins is ClearedPinsSetting.Persistent or ClearedPinsSetting.All)
            {
                _activeItems.AddRange(EverObtainedPersistentItems);
            }
        }
        else
        {
            State = PlacementState.Cleared;
            if (RandoMapCoreMod.GS.ShowClearedPins is ClearedPinsSetting.All)
            {
                _activeItems.AddRange(EverObtainedPersistentItems);
                _activeItems.AddRange(EverObtainedNonPersistentItems);
            }
        }
    }
}

internal enum PlacementState
{
    Previewable,
    NotCleared,

    // The placement is cleared, but has persistent items that are currently obtainable
    ClearedPersistent,
    Cleared,
}
