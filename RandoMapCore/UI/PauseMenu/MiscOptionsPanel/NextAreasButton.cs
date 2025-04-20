using MagicUI.Elements;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class NextAreasButton() : BorderlessExtraButton(nameof(NextAreasButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleNextAreas();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Show next area indicators (text/arrow) on the quick map.".L();
    }

    public override void Update()
    {
        var text = $"{"Show next\nareas".L()}: ";

        switch (RandoMapCoreMod.GS.ShowNextAreas)
        {
            case NextAreaSetting.Off:
                text += "Off".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
                break;

            case NextAreaSetting.Arrows:
                text += "Arrows".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
                break;

            case NextAreaSetting.Full:
                text += "Full".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
                break;
            default:
                break;
        }

        Button.Content = text;
    }
}
