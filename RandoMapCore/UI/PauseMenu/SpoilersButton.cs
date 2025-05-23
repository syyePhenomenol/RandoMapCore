using MapChanger;
using MapChanger.UI;

namespace RandoMapCore.UI;

internal class SpoilersButton : RmcMainButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            RandoMapCoreMod.LS.ToggleSpoilers();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Spoilers".L()}:\n";

        if (!RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            return (text + "Off".L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return UIExtensions.GetBoolTextFormat(text, RandoMapCoreMod.LS.SpoilerOn);
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableSpoilerToggle ? "Reveals the items at each location.".L() : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
