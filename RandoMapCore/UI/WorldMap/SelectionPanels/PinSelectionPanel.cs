using MagicUI.Core;
using MagicUI.Elements;
using MagicUI.Graphics;
using MapChanger;
using RandoMapCore.Pins;

namespace RandoMapCore.UI;

internal class PinSelectionPanel
{
    private readonly Panel _pinPanel;
    private readonly TextObject _pinPanelText;

    internal PinSelectionPanel(LayoutRoot layout, StackLayout panelStack)
    {
        Instance = this;

        _pinPanel = new(
            layout,
            SpriteManager.Instance.GetTexture("GUI.PanelRight").ToSlicedSprite(100f, 50f, 200f, 50f),
            "Lookup Panel"
        )
        {
            Borders = new(30f, 30f, 30f, 30f),
            MinWidth = 200f,
            MinHeight = 100f,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
        };

        ((Image)layout.GetElement("Lookup Panel Background")).Tint = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        _pinPanelText = new(layout, "Pin Panel Text")
        {
            ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            Font = MagicUI.Core.UI.Perpetua,
            FontSize = 20,
            MaxWidth = 450f,
        };

        _pinPanel.Child = _pinPanelText;

        panelStack.Children.Add(_pinPanel);
    }

    internal static PinSelectionPanel Instance { get; private set; }

    internal bool ShowHint { get; private set; }

    internal void RevealHint()
    {
        ShowHint = true;
    }

    internal void HideHint()
    {
        ShowHint = false;
    }

    internal void Update()
    {
        if (_pinPanel is null || _pinPanelText is null)
        {
            return;
        }

        if (RandoMapCoreMod.GS.PinSelectionOn && PinSelector.Instance.SelectedObject is IPinSelectable pin)
        {
            _pinPanelText.Text = pin.GetText();
            _pinPanel.Visibility = Visibility.Visible;
        }
        else
        {
            _pinPanel.Visibility = Visibility.Collapsed;
        }
    }
}
