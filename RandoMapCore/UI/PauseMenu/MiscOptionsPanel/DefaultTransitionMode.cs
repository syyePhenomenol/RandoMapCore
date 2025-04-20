using MagicUI.Elements;
using MapChanger;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class DefaultTransitionModeButton() : BorderlessExtraButton(nameof(DefaultTransitionModeButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleDefaultTransitionRandoMode();
        OnHover();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText =
            $"{"Default map mode when starting a new transition rando save".L()}: {RandoMapCoreMod.GS.DefaultTransitionRandoMode.ToString().ToCleanName().L()}";
    }

    public override void Update()
    {
        var text = $"{"Def. transition\nmode".L()}: ";

        var colorSetting = RmcColorSetting.UI_Neutral;

        switch (RandoMapCoreMod.GS.DefaultTransitionRandoMode)
        {
            case RmcMode.Full_Map:
                colorSetting = RmcColorSetting.UI_On;
                text += "FM".L();
                break;
            case RmcMode.All_Pins:
                text += "AP".L();
                break;
            case RmcMode.Pins_Over_Area:
                text += "POA".L();
                break;
            case RmcMode.Pins_Over_Room:
                text += "POR".L();
                break;
            case RmcMode.Transition_Normal:
                colorSetting = RmcColorSetting.UI_Special;
                text += "T1".L();
                break;
            case RmcMode.Transition_Visited_Only:
                colorSetting = RmcColorSetting.UI_Special;
                text += "T2".L();
                break;
            case RmcMode.Transition_All_Rooms:
                colorSetting = RmcColorSetting.UI_Special;
                text += "T3".L();
                break;
            default:
                break;
        }

        Button.ContentColor = RmcColors.GetColor(colorSetting);
        Button.Content = text;
    }
}
