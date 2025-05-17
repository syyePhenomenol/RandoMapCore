using MagicUI.Core;
using MagicUI.Elements;
using MapChanger;
using MapChanger.UI;

namespace RandoMapCore.UI;

internal abstract class ControlPanelText : UIElementWrapper<WorldMapLayout, TextObject>
{
    internal TextObject Text => Element;

    public override void Update()
    {
        var textFormat = GetTextFormat();
        Text.Text = textFormat.Text;
        Text.ContentColor = textFormat.Color;

        var scaleFactor = (float)MapChangerMod.GS.UIScale;
        Text.FontSize = (int)(14f * scaleFactor);
    }

    protected override TextObject Build(WorldMapLayout layout)
    {
        return new(layout, Name)
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = HorizontalAlignment.Left,
            Font = MagicUI.Core.UI.TrajanNormal,
            Padding = new(0f, 2f, 0f, 2f),
        };
    }

    private protected abstract TextFormat GetTextFormat();
}
