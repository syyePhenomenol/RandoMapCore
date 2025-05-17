using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class RandomizedButton : RmcMainButton
{
    protected override void OnClick()
    {
        RandoMapCoreMod.LS.ToggleRandomized();
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Randomized".L()}:\n";
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
            text += $" ({"Custom".L()})";
            color = RmcColorSetting.UI_Custom;
        }

        return (text, color).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "Toggle pins for randomized locations on/off.".L().ToNeutralTextFormat();
    }
}
