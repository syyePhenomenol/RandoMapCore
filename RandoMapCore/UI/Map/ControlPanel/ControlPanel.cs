using MagicUI.Core;
using MagicUI.Elements;
using MapChanger.UI;
using UnityEngine;

namespace RandoMapCore.UI;

internal class ControlPanel : WorldMapPanel
{
    private StackLayout _stack;

    private ShowHotkeysText _showHotkeysText;
    private List<ControlPanelText> _collapsableTexts;

    public override void Update()
    {
        base.Update();

        _showHotkeysText?.Update();

        foreach (var text in _collapsableTexts.Where(t => t is not null))
        {
            if (RandoMapCoreMod.GS.ControlPanelOn)
            {
                text.Text.Visibility = Visibility.Visible;
                text.Update();
            }
            else
            {
                text.Text.Visibility = Visibility.Collapsed;
            }
        }
    }

    protected override Panel Build(WorldMapLayout layout)
    {
        var panel = base.Build(layout);

        _stack = new(layout, $"{Name} Stack")
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Orientation = Orientation.Vertical,
        };

        panel.Child = _stack;

        _showHotkeysText = new ShowHotkeysText();
        _showHotkeysText.Initialize(layout);
        _stack.Children.Add(_showHotkeysText.Text);

        _collapsableTexts = [new ModEnabledText(), new ModeText(), new ShiftPanText(), new MapKeyText()];

        if (RandoMapCoreMod.Data.EnableProgressionHints)
        {
            _collapsableTexts.Add(new ProgressHintText());
        }

        if (RandoMapCoreMod.Data.EnableItemCompass)
        {
            _collapsableTexts.Add(new ItemCompassText());
        }

        if (RandoMapCoreMod.Data.EnablePinSelection)
        {
            _collapsableTexts.Add(new PinSelectionText());
        }

        if (Interop.HasBenchwarp)
        {
            _collapsableTexts.Add(new BenchwarpPinsText());
        }

        if (RandoMapCoreMod.Data.EnableRoomSelection)
        {
            _collapsableTexts.Add(new RoomSelectionText());
        }

        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder && Interop.HasBenchwarp)
        {
            _collapsableTexts.Add(new PathfinderBenchwarpText());
        }

        foreach (var text in _collapsableTexts)
        {
            text.Initialize(layout);
            _stack.Children.Add(text.Text);
        }

        return panel;
    }

    protected override Color GetBackgroundTint()
    {
        return RmcColors.GetColor(RmcColorSetting.UI_Borders);
    }
}
