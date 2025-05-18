using MapChanger.UI;

namespace RandoMapCore.UI;

internal class MapMarkersButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RandoMapCoreMod.GS.ToggleMapMarkers();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Show Map\nMarkers".L()}: ";

        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            return (text + "Off".L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return UIExtensions.GetBoolTextFormat(text, RandoMapCoreMod.GS.ShowMapMarkers);
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableVisualCustomization
                ? "Enable vanilla map marker behavior.".L()
                : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
