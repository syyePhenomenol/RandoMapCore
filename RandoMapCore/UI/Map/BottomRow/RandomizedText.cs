using MapChanger.UI;
using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class RandomizedText : BottomRowText
{
    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Randomized".L()} {ToggleRandomizedInput.Instance.GetBindingsText()}: ";
        RmcColorSetting color;

        if (RandoMapCoreMod.LS.RandomizedOn)
        {
            text += "On".L();
            color = RmcColorSetting.UI_On;
        }
        else
        {
            text += "Off".L();
            color = RmcColorSetting.UI_Neutral;
        }

        if (RandoMapCoreMod.LS.IsRandomizedCustom())
        {
            color = RmcColorSetting.UI_Custom;
        }

        return (text, color).ToTextFormat();
    }
}
