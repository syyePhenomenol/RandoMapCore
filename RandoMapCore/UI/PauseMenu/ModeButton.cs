using MagicUI.Elements;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Localization;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class ModeButton() : MainButton(nameof(ModeButton), RandoMapCoreMod.Data.ModName, 1, 0)
{
    protected override void OnClick()
    {
        MapChanger.Settings.ToggleMode();
        OnHover();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText =
            $"{"Current map mode".L()}: {MapChanger.Settings.CurrentMode().ModeName.ToString().ToCleanName().L()}";
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        var text = $"{"Mode".L()}:";

        var colorSetting = RmcColorSetting.UI_Neutral;

        switch (MapChanger.Settings.CurrentMode())
        {
            case FullMapMode:
                colorSetting = RmcColorSetting.UI_On;
                text += $"\n{"Full Map".L()}";
                break;
            case AllPinsMode:
                text += $"\n{"All Pins".L()}";
                break;
            case PinsOverAreaMode:
                text += $" {"Pins\nOver Area".L()}";
                break;
            case PinsOverRoomMode:
                text += $" {"Pins\nOver Room".L()}";
                break;
            case TransitionNormalMode:
                colorSetting = RmcColorSetting.UI_Special;
                text += $"\n{"Transition".L()} 1";
                break;
            case TransitionVisitedOnlyMode:
                colorSetting = RmcColorSetting.UI_Special;
                text += $"\n{"Transition".L()} 2";
                ;
                break;
            case TransitionAllRoomsMode:
                colorSetting = RmcColorSetting.UI_Special;
                text += $"\n{"Transition".L()} 3";
                break;
            default:
                break;
        }

        Button.ContentColor = RmcColors.GetColor(colorSetting);
        Button.Content = text;
    }
}
