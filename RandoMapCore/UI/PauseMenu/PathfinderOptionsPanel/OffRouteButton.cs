using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class OffRouteButton() : BorderlessExtraButton(nameof(OffRouteButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleWhenOffRoute();
        MapUILayerUpdater.Update();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "When going off route, how the pathfinder route is updated.".L();
    }

    public override void Update()
    {
        var text = $"{"Off route".L()}:\n";

        switch (RandoMapCoreMod.GS.WhenOffRoute)
        {
            case Settings.OffRouteBehaviour.Cancel:
                text += "cancel route".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
                break;

            case Settings.OffRouteBehaviour.Keep:
                text += "keep route".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
                break;

            case Settings.OffRouteBehaviour.Reevaluate:
                text += "reevaluate".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
                break;
            default:
                break;
        }

        Button.Content = text;
    }
}
