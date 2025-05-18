using MapChanger;
using MapChanger.UI;
using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class ShapeText : BottomRowText
{
    protected override TextFormat GetTextFormat()
    {
        return (
            $"{"Shape".L()} {ToggleShapeInput.Instance.GetBindingsText()}: "
                + RandoMapCoreMod.GS.PinShapes.ToString().ToWhitespaced().L(),
            RmcColorSetting.UI_Neutral
        ).ToTextFormat();
    }
}
