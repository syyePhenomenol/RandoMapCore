using MagicUI.Core;
using MagicUI.Elements;
using MapChanger;
using MapChanger.UI;

namespace RandoMapCore.UI;

internal abstract class TopEdgeText<T> : UIElementWrapper<T, TextObject>
    where T : UILayout
{
    internal TextObject Text => Element;

    internal abstract HorizontalAlignment Alignment { get; }

    public override void Update()
    {
        var textFormat = GetTextFormat();
        Text.Text = textFormat.Text;
        Text.ContentColor = textFormat.Color;
        Text.FontSize = (int)(14f * MapChangerMod.GS.UIScale);
    }

    protected override TextObject Build(T layout)
    {
        TextObject text =
            new(layout, Name)
            {
                ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral),
                HorizontalAlignment = Alignment,
                VerticalAlignment = VerticalAlignment.Top,
                TextAlignment = Alignment,
                Font = MagicUI.Core.UI.TrajanNormal,
            };

        if (Alignment is HorizontalAlignment.Right)
        {
            text.Padding = new(0f, 20f, 20f, 0f);
        }
        else
        {
            text.Padding = new(20f, 20f, 0f, 0f);
        }

        return text;
    }

    private protected abstract TextFormat GetTextFormat();
}
