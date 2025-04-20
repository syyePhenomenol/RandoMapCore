using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class RouteTextButton() : BorderlessExtraButton(nameof(RouteTextButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleRouteTextInGame();
        MapUILayerUpdater.Update();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "How the route text is displayed during gameplay.".L();
    }

    public override void Update()
    {
        var text = $"{"Route text".L()}:\n";

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
