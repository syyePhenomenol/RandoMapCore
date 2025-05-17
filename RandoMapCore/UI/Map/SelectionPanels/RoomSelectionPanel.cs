using MagicUI.Core;
using MagicUI.Elements;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Modes;
using RandoMapCore.Rooms;
using UnityEngine;

namespace RandoMapCore.UI;

internal class RoomSelectionPanel : WorldMapPanel
{
    private TextObject _text;

    public override void Update()
    {
        base.Update();

        if (
            Conditions.TransitionRandoModeEnabled()
            && RandoMapCoreMod.GS.RoomSelectionOn
            && RmcRoomManager.Selector?.GetSelectionText() is string text
        )
        {
            Panel.Visibility = Visibility.Visible;
            _text.Text = text;
            _text.FontSize = (int)(14f * MapChangerMod.GS.UIScale);
            _text.MaxWidth = (int)(450f * MapChangerMod.GS.UIScale);
        }
        else
        {
            Panel.Visibility = Visibility.Collapsed;
        }
    }

    protected override Panel Build(WorldMapLayout layout)
    {
        var panel = base.Build(layout);

        _text = new(layout, $"{Name} Text")
        {
            ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            Font = MagicUI.Core.UI.TrajanNormal,
        };

        panel.Child = _text;

        return panel;
    }

    public override void Destroy()
    {
        _text?.Destroy();
        base.Destroy();
        // Instance = null;
    }

    protected override Color GetBackgroundTint()
    {
        return RmcColors.GetColor(RmcColorSetting.UI_Borders);
    }
}
