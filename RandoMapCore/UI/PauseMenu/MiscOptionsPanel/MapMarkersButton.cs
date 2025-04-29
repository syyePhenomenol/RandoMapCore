using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class MapMarkersButton() : BorderlessExtraButton(nameof(MapMarkersButton))
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RandoMapCoreMod.GS.ToggleMapMarkers();
        }
    }

    protected override void OnHover()
    {
        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RmcTitle.Instance.HoveredText = "Toggle disabled".L();
        }

        RmcTitle.Instance.HoveredText = "Enable vanilla map marker behavior.".L();
    }

    public override void Update()
    {
        var text = $"{"Show map\nmarkers".L()}: ";

        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            Button.Content = text + "Off".L();
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
            return;
        }

        this.SetButtonBoolToggle(text, RandoMapCoreMod.GS.ShowMapMarkers);
    }
}
