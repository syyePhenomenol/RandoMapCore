using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class RouteCompassButton() : BorderlessExtraButton(nameof(RouteCompassButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleRouteCompassEnabled();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Point compass to next transition in the pathfinder route.".L();
    }

    public override void Update()
    {
        this.SetButtonBoolToggle($"{"Route compass".L()}:\n", RandoMapCoreMod.GS.ShowRouteCompass);
    }
}
