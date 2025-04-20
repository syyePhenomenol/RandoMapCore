using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class MapMarkersButton() : BorderlessExtraButton(nameof(MapMarkersButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleMapMarkers();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Enable vanilla map marker behavior.".L();
    }

    public override void Update()
    {
        this.SetButtonBoolToggle($"{"Show map\nmarkers".L()}: ", RandoMapCoreMod.GS.ShowMapMarkers);
    }
}
