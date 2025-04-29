using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class RouteCompassButton() : BorderlessExtraButton(nameof(RouteCompassButton))
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            RandoMapCoreMod.GS.ToggleRouteCompassEnabled();
        }
    }

    protected override void OnHover()
    {
        if (!RandoMapCoreMod.Data.EnableRoomSelection || !RandoMapCoreMod.Data.EnablePathfinder)
        {
            RmcTitle.Instance.HoveredText = "Toggle disabled".L();
            return;
        }

        RmcTitle.Instance.HoveredText = "Point compass to next transition in the pathfinder route.".L();
    }

    public override void Update()
    {
        var text = $"{"Route compass".L()}:\n";

        if (!RandoMapCoreMod.Data.EnableRoomSelection || !RandoMapCoreMod.Data.EnablePathfinder)
        {
            Button.Content = text + "Disabled".L();
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
            return;
        }

        this.SetButtonBoolToggle(text, RandoMapCoreMod.GS.ShowRouteCompass);
    }
}
