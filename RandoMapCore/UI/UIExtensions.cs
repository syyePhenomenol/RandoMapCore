using MapChanger;
using MapChanger.UI;

namespace RandoMapCore.UI;

public static class UIExtensions
{
    public static TextFormat ToTextFormat(this (string text, RmcColorSetting colorSetting) format)
    {
        return new(format.text, RmcColors.GetColor(format.colorSetting));
    }

    public static TextFormat ToNeutralTextFormat(this string text)
    {
        return new(text, RmcColors.GetColor(RmcColorSetting.UI_Neutral));
    }

    public static TextFormat GetBoolTextFormat(string baseText, bool value)
    {
        return (
            value ? (baseText + "On".L(), RmcColorSetting.UI_On) : (baseText + "Off".L(), RmcColorSetting.UI_Neutral)
        ).ToTextFormat();
    }
}
