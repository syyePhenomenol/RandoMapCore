using MapChanger;
using MapChanger.UI;

namespace RandoMapCore.UI;

internal class AreaNamesButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RandoMapCoreMod.GS.ToggleAreaNames();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Show Area\nNames".L()}: ";

        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            return (text + "On".L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return UIExtensions.GetBoolTextFormat(text, RandoMapCoreMod.GS.ShowAreaNames);
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableVisualCustomization
                ? "Show area names on the world/quick map.".L()
                : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
