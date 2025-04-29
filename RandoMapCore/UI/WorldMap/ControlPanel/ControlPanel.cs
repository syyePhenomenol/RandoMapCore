using MagicUI.Core;
using MagicUI.Elements;
using MagicUI.Graphics;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class ControlPanel : WorldMapStack
{
    private static readonly List<ControlPanelText> _texts = [];
    private static Panel _panel;
    private static StackLayout _panelStack;

    protected override HorizontalAlignment StackHorizontalAlignment => HorizontalAlignment.Left;
    protected override VerticalAlignment StackVerticalAlignment => VerticalAlignment.Bottom;

    protected override void BuildStack()
    {
        _panel = new(
            Root,
            SpriteManager.Instance.GetTexture("GUI.PanelLeft").ToSlicedSprite(200f, 50f, 100f, 50f),
            "Panel"
        )
        {
            MinWidth = 0f,
            MinHeight = 0f,
            Borders = new(10f, 20f, 30f, 20f),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        Stack.Children.Add(_panel);

        ((Image)Root.GetElement("Panel Background")).Tint = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        _panelStack = new(Root, "Panel Stack")
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Orientation = Orientation.Vertical,
        };

        _panel.Child = _panelStack;

        _texts.Clear();
        _texts.AddRange(
            [new ShowHotkeysText(), new ModEnabledText(), new ModeText(), new ShiftPanText(), new MapKeyText()]
        );

        if (RandoMapCoreMod.Data.EnableProgressionHints)
        {
            _texts.Add(new ProgressHintText());
        }

        if (RandoMapCoreMod.Data.EnableItemCompass)
        {
            _texts.Add(new ItemCompassText());
        }

        if (RandoMapCoreMod.Data.EnablePinSelection)
        {
            _texts.Add(new PinSelectionText());
        }

        if (Interop.HasBenchwarp)
        {
            _texts.Add(new BenchwarpPinsText());
        }

        if (RandoMapCoreMod.Data.EnableRoomSelection)
        {
            _texts.Add(new RoomSelectionText());
        }

        if (RandoMapCoreMod.Data.EnablePinSelection || RandoMapCoreMod.Data.EnableRoomSelection)
        {
            _texts.Add(new ShowReticleText());
        }

        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder && Interop.HasBenchwarp)
        {
            _texts.Add(new PathfinderBenchwarpText());
        }

        foreach (var cpt in _texts)
        {
            cpt.Make(Root, _panelStack);
        }
    }

    protected override bool Condition()
    {
        return base.Condition() && Conditions.RandoCoreMapModEnabled();
    }

    public override void Update()
    {
        foreach (var cpt in _texts)
        {
            cpt.Update();
        }
    }
}
