using RandoMapCore.Input;
using RandoMapCore.Localization;

namespace RandoMapCore.Pins;

internal class PinCluster(List<RmcPin> selectables) : SelectableGroup<RmcPin>(selectables), IPinSelectable
{
    private int _selectionIndex;
    private RmcPin[] _sortedPins;
    private float[] _zValues;

    internal RmcPin SelectedPin => _sortedPins[_selectionIndex];

    internal void UpdateSelectablePins()
    {
        _sortedPins = [.. Selectables.Where(s => s.CanSelect()).OrderBy(p => p.Def.GetZPriority())];
        _zValues = [.. _sortedPins.Select(p => p.transform.localPosition.z)];
        ResetSelectionIndex();
    }

    internal void ToggleSelectedPin()
    {
        _selectionIndex = (_selectionIndex + 1) % _sortedPins.Length;
        SetShiftedZOffsets();
    }

    internal void ResetSelectionIndex()
    {
        _selectionIndex = 0;
        SetShiftedZOffsets();
    }

    private void SetShiftedZOffsets()
    {
        // Shift z position of sorted pins in a circular array
        for (var i = 0; i < _sortedPins.Length; i++)
        {
            var pinTransform = _sortedPins[(_selectionIndex + i) % _sortedPins.Length].gameObject.transform;
            var origLocalPosition = pinTransform.localPosition;
            pinTransform.localPosition = new(origLocalPosition.x, origLocalPosition.y, _zValues[i]);
        }
    }

    public string GetText()
    {
        if (!_sortedPins.Any())
        {
            RandoMapCoreMod.Instance.LogWarn($"Selected PinCluster {Key} has no active pins!");
            return null;
        }

        if (_sortedPins.Length is 1)
        {
            return SelectedPin.GetText();
        }

        var nextPin = _sortedPins[(_selectionIndex + 1) % _sortedPins.Length];

        var bindingsText = TogglePinClusterInput.Instance.GetBindingsText();
        return $"{SelectedPin.GetText()}\n\n{"Press".L()} {bindingsText} {"to toggle selected pin to".L()} {nextPin.Name.LC()}.";
    }
}
