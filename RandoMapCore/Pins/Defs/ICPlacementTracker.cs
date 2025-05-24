using System.Collections.ObjectModel;
using ItemChanger;
using RandoMapCore.Settings;
using SM = ConnectionMetadataInjector.SupplementalMetadata;

namespace RandoMapCore.Pins;

internal class ICPlacementTracker(AbstractPlacement placement)
{
    // Items that can be fully previewed without spoilers even if the placement isn't visited yet.
    private readonly List<AbstractItem> _neverObtainedForcePreviewItems = [];

    // Items of which name/sprite can be displayed without spoilers after visiting the placement.
    private readonly List<AbstractItem> _neverObtainedPreviewableItems = [];

    // Items of which name/sprite cannot be displayed without spoilers after visiting the placement.
    private readonly List<AbstractItem> _neverObtainedUnpreviewableItems = [];
    private readonly List<AbstractItem> _everObtainedPersistentItems = [];
    private readonly List<AbstractItem> _everObtainedNonPersistentItems = [];

    private readonly List<AbstractItem> _activeItems = [];

    private bool _enqueueUpdateItems;

    internal PlacementState State { get; private set; }

    // The following are used for text generation
    internal IEnumerable<AbstractItem> ForcePreviewedItems => _neverObtainedForcePreviewItems;
    internal IEnumerable<AbstractItem> NeverObtainedItems =>
        _neverObtainedForcePreviewItems.Concat(_neverObtainedPreviewableItems).Concat(_neverObtainedUnpreviewableItems);
    internal IEnumerable<AbstractItem> EverObtainedItems =>
        _everObtainedNonPersistentItems.Concat(_everObtainedPersistentItems);
    internal IEnumerable<AbstractItem> EverObtainedPersistentItems => _everObtainedPersistentItems;

    // This is used primarily for getting the sprites
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
        _neverObtainedForcePreviewItems.Clear();
        _neverObtainedPreviewableItems.Clear();
        _neverObtainedUnpreviewableItems.Clear();
        _everObtainedPersistentItems.Clear();
        _everObtainedNonPersistentItems.Clear();

        var placementPreviewable = placement.CanPreviewName();

        foreach (var i in placement.Items)
        {
            if (!i.WasEverObtained())
            {
                if (SM.Of(i).Get(InteropProperties.ForceEnablePreview))
                {
                    _neverObtainedForcePreviewItems.Add(i);
                }
                else if (placementPreviewable && i.CanPreviewName())
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

        if (placement.CheckVisitedAny(VisitState.Previewed) && NeverObtainedItems.Any())
        {
            State = PlacementState.Previewed;
            _activeItems.AddRange(_neverObtainedForcePreviewItems);
            _activeItems.AddRange(_neverObtainedPreviewableItems);
            if (RandoMapCoreMod.Data.EnableSpoilerToggle && RandoMapCoreMod.LS.SpoilerOn)
            {
                _activeItems.AddRange(_neverObtainedUnpreviewableItems);
            }
        }
        else if (_neverObtainedForcePreviewItems.Any())
        {
            State = PlacementState.ForcePreviewed;
            _activeItems.AddRange(_neverObtainedForcePreviewItems);
            if (RandoMapCoreMod.Data.EnableSpoilerToggle && RandoMapCoreMod.LS.SpoilerOn)
            {
                _activeItems.AddRange(_neverObtainedPreviewableItems);
                _activeItems.AddRange(_neverObtainedUnpreviewableItems);
            }
        }
        else if (_neverObtainedPreviewableItems.Any() || _neverObtainedUnpreviewableItems.Any())
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
                _activeItems.AddRange(_everObtainedPersistentItems);
            }
        }
        else
        {
            State = PlacementState.Cleared;
            if (RandoMapCoreMod.GS.ShowClearedPins is ClearedPinsSetting.All)
            {
                _activeItems.AddRange(_everObtainedNonPersistentItems);
            }
        }
    }
}

internal enum PlacementState
{
    // The placement has been visited, and there are items with name/sprite that can be displayed
    Previewed,

    // The placement hasn't been visited but items can be force shown with name/sprite/cost
    ForcePreviewed,

    // The placement has items that haven't ever been obtained, and hasn't ever been previewed
    NotCleared,

    // The placement is cleared, but has persistent items that are currently obtainable
    ClearedPersistent,

    // The placement is cleared with no persistent items currently obtainable
    Cleared,
}
