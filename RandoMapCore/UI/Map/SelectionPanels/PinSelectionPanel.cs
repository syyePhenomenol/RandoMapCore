using MagicUI.Core;
using MagicUI.Elements;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Pins;
using UnityEngine;

namespace RandoMapCore.UI;

internal class PinSelectionPanel : WorldMapPanel
{
    private TextObject _text;

    internal static PinSelectionPanel Instance { get; private set; }

    internal bool ShowHint { get; private set; }

    protected override Panel Build(WorldMapLayout layout)
    {
        Instance = this;

        var panel = base.Build(layout);

        _text = new(layout, $"{Name} Text")
        {
            ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            Font = MagicUI.Core.UI.Perpetua,
            MaxWidth = 450f,
        };

        panel.Child = _text;

        return panel;
    }

    public override void Update()
    {
        base.Update();

        if (RandoMapCoreMod.GS.PinSelectionOn && RmcPinManager.Selector?.GetSelectionText() is RunCollection inlines)
        {
            Panel.Visibility = Visibility.Visible;
            _text.Inlines = inlines;
            _text.FontSize = (int)(20f * MapChangerMod.GS.UIScale);
            _text.MaxWidth = (int)(450f * MapChangerMod.GS.UIScale);
        }
        else
        {
            Panel.Visibility = Visibility.Collapsed;
        }
    }

    public override void Destroy()
    {
        _text?.Destroy();
        base.Destroy();
        Instance = null;
    }

    internal void RevealHint()
    {
        ShowHint = true;
    }

    internal void HideHint()
    {
        ShowHint = false;
    }

    protected override Color GetBackgroundTint()
    {
        return RmcColors.GetColor(RmcColorSetting.UI_Borders);
    }
}
