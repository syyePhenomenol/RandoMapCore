using MapChanger.UI;
using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class VanillaText : BottomRowText
{
    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Vanilla".L()} {ToggleVanillaInput.Instance.GetBindingsText()}: ";
        RmcColorSetting color;

        if (RandoMapCoreMod.LS.VanillaOn)
        {
            text += "On".L();
            color = RmcColorSetting.UI_On;
        }
        else
        {
            text += "Off".L();
            color = RmcColorSetting.UI_Neutral;
        }

        if (RandoMapCoreMod.LS.IsVanillaCustom())
        {
            color = RmcColorSetting.UI_Custom;
        }

        return (text, color).ToTextFormat();
    }
}
