using MagicUI.Core;
using MagicUI.Elements;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Localization;
using RandoMapCore.Modes;
using UnityEngine;

namespace RandoMapCore.UI;

internal class MapKeyPanel : WorldMapPanel
{
    private StackLayout _mapKeyContents;
    private GridLayout _pinKey;
    private GridLayout _roomKey;

    private List<TextObject> _texts;

    public override void Update()
    {
        base.Update();

        if (RandoMapCoreMod.GS.MapKeyOn)
        {
            Panel.Visibility = Visibility.Visible;
        }
        else
        {
            Panel.Visibility = Visibility.Collapsed;
        }

        if (Conditions.TransitionRandoModeEnabled())
        {
            _roomKey.Visibility = Visibility.Visible;
        }
        else
        {
            _roomKey.Visibility = Visibility.Collapsed;
        }

        foreach (var text in _texts)
        {
            text.FontSize = (int)(12f * MapChangerMod.GS.UIScale);
        }
    }

    protected override Panel Build(WorldMapLayout layout)
    {
        var panel = base.Build(layout);

        _mapKeyContents = new(layout, $"{Name} Stack")
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Orientation = Orientation.Horizontal,
            Spacing = 5f,
        };

        panel.Child = _mapKeyContents;

        _pinKey = new(layout, $"{Name} Pin Key")
        {
            MinWidth = 200f,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top,
            RowDefinitions =
            {
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1, GridUnit.Proportional),
            },
            ColumnDefinitions =
            {
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1.6f, GridUnit.Proportional),
            },
        };

        _mapKeyContents.Children.Add(_pinKey);

        _texts = [];
        var counter = 0;
        foreach (var colorSetting in RmcColors.PinBorderColors)
        {
            var pinPanel = new Panel(
                layout,
                SpriteManager.Instance.GetSprite("Pins.Blank"),
                colorSetting.ToString() + "Panel"
            )
            {
                MinHeight = 50f,
                MinWidth = 50f,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            }
                .WithProp(GridLayout.Column, 0)
                .WithProp(GridLayout.Row, counter);

            var pin = new Image(
                layout,
                SpriteManager.Instance.GetSprite("Pins.Border"),
                colorSetting.ToString() + " Pin"
            )
            {
                Width = 50f,
                Height = 50f,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            }
                .WithProp(GridLayout.Column, 0)
                .WithProp(GridLayout.Row, counter);

            ((Image)layout.GetElement(colorSetting.ToString() + " Pin")).Tint = RmcColors.GetColor(colorSetting);

            pinPanel.Child = pin;

            var text = new TextObject(layout, colorSetting.ToString() + " Text")
            {
                Text = Utils.ToCleanName(colorSetting.ToString().Replace("Pin_", "")).L(),
                Padding = new(10f, 0f, 0f, 0f),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            }
                .WithProp(GridLayout.Column, 1)
                .WithProp(GridLayout.Row, counter);

            _texts.Add(text);

            _pinKey.Children.Add(pinPanel);
            _pinKey.Children.Add(text);

            counter++;
        }

        _roomKey = new(layout, "Room Key")
        {
            MinWidth = 200f,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            RowDefinitions =
            {
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1, GridUnit.Proportional),
            },
            ColumnDefinitions =
            {
                new GridDimension(1, GridUnit.Proportional),
                new GridDimension(1.6f, GridUnit.Proportional),
            },
        };

        _mapKeyContents.Children.Add(_roomKey);

        var roomCopy = GameManager
            .instance.gameMap.transform.GetChild(12)
            .transform.GetChild(26)
            .GetComponent<SpriteRenderer>()
            .sprite;

        counter = 0;

        foreach (var color in RmcColors.RoomColors)
        {
            var cleanRoomColor = Utils.ToCleanName(color.ToString().Replace("Room_", ""));

            var room = new Image(layout, roomCopy, cleanRoomColor + " Room")
            {
                Width = 40f,
                Height = 40f,
                Tint = RmcColors.GetColor(color),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new(0f, 5f, 17f, 5f),
            }
                .WithProp(GridLayout.Column, 0)
                .WithProp(GridLayout.Row, counter);

            var text = new TextObject(layout, cleanRoomColor + " Text")
            {
                Text = cleanRoomColor.L(),
                Padding = new(10f, 0f, 0f, 0f),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            }
                .WithProp(GridLayout.Column, 1)
                .WithProp(GridLayout.Row, counter);

            _texts.Add(text);

            _roomKey.Children.Add(room);
            _roomKey.Children.Add(text);

            counter++;
        }

        var highlighted = RmcColors.GetColor(RmcColorSetting.Room_Normal);
        highlighted.w = 1f;

        var roomHighlight = new Image(layout, roomCopy, "Highlighted Room")
        {
            Width = 40f,
            Height = 40f,
            Tint = highlighted,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Padding = new(0f, 5f, 17f, 5f),
        }
            .WithProp(GridLayout.Column, 0)
            .WithProp(GridLayout.Row, counter);

        var textHighlight = new TextObject(layout, "Highlighted Text")
        {
            Text = "Contains\nunchecked\ntransitions".L(),
            Padding = new(10f, 0f, 0f, 0f),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
        }
            .WithProp(GridLayout.Column, 1)
            .WithProp(GridLayout.Row, counter);

        _texts.Add(textHighlight);

        _roomKey.Children.Add(roomHighlight);
        _roomKey.Children.Add(textHighlight);

        return panel;
    }

    protected override Color GetBackgroundTint()
    {
        return RmcColors.GetColor(RmcColorSetting.UI_Borders);
    }
}
