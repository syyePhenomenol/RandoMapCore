using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class RouteTextButton() : BorderlessExtraButton(nameof(RouteTextButton))
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            RandoMapCoreMod.GS.ToggleRouteTextInGame();
            MapUILayerUpdater.Update();
        }
    }

    protected override void OnHover()
    {
        if (!RandoMapCoreMod.Data.EnableRoomSelection || !RandoMapCoreMod.Data.EnablePathfinder)
        {
            RmcTitle.Instance.HoveredText = "Toggle disabled".L();
            return;
        }

        RmcTitle.Instance.HoveredText = "How the route text is displayed during gameplay.".L();
    }

    public override void Update()
    {
        var text = $"{"Route text".L()}:\n";

        if (!RandoMapCoreMod.Data.EnableRoomSelection || !RandoMapCoreMod.Data.EnablePathfinder)
        {
            Button.Content = text + "Disabled".L();
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
            return;
        }

        switch (RandoMapCoreMod.GS.RouteTextInGame)
        {
            case Settings.RouteTextInGame.Hide:
                text += "hide".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
                break;

            case Settings.RouteTextInGame.NextTransitionOnly:
                text += "next transition".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
                break;

            case Settings.RouteTextInGame.Show:
                text += "all transitions".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
                break;
            default:
                break;
        }

        Button.Content = text;
    }
}
