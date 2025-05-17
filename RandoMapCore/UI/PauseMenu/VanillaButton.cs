using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class VanillaButton : RmcMainButton
{
    protected override void OnClick()
    {
        RandoMapCoreMod.LS.ToggleVanilla();
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Vanilla".L()}:\n";
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

        if (RandoMapCoreMod.LS.IsRandomizedCustom())
        {
            text += $" ({"Custom".L()})";
            color = RmcColorSetting.UI_Custom;
        }

        return (text, color).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "Toggle pins for vanilla locations on/off.".L().ToNeutralTextFormat();
    }
}
