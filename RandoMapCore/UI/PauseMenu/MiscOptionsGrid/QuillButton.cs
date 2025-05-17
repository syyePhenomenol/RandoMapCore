using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class QuillButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RandoMapCoreMod.GS.ToggleAlwaysHaveQuill();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Always Have\nQuill".L()}: ";

        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            return (text + "On".L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return UIExtensions.GetBoolTextFormat(text, RandoMapCoreMod.GS.AlwaysHaveQuill);
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableVisualCustomization
                ? "Doesn't affect Full Map and Transition modes.".L()
                : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
