using MapChanger;
using MapChanger.UI;
using RandoMapCore.Input;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class SizeText : BottomRowText
{
    protected override TextFormat GetTextFormat()
    {
        return (
            $"{"Size".L()} {ToggleSizeInput.Instance.GetBindingsText()}: "
                + RandoMapCoreMod.GS.PinSize.ToString().ToWhitespaced().L(),
            RmcColorSetting.UI_Neutral
        ).ToTextFormat();
    }
}
