using MapChanger;
using MapChanger.UI;
using RandoMapCore.Localization;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class ModeButton : RmcMainButton
{
    protected override void OnClick()
    {
        ModeManager.ToggleMode();
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Mode".L()}:";

        (string, RmcColorSetting) format = ModeManager.CurrentMode() switch
        {
            FullMapMode => new(text + $"\n{"Full Map".L()}", RmcColorSetting.UI_On),
            AllPinsMode => new(text + $"\n{"All Pins".L()}", RmcColorSetting.UI_Neutral),
            PinsOverAreaMode => new(text + $" {"Pins\nOver Area".L()}", RmcColorSetting.UI_Neutral),
            PinsOverRoomMode => new(text + $" {"Pins\nOver Room".L()}", RmcColorSetting.UI_Neutral),
            TransitionNormalMode => new(text + $"\n{"Transition".L()} 1", RmcColorSetting.UI_Special),
            TransitionVisitedOnlyMode => new(text + $"\n{"Transition".L()} 2", RmcColorSetting.UI_Special),
            TransitionAllRoomsMode => new(text + $"\n{"Transition".L()} 3", RmcColorSetting.UI_Special),
            _ => new(text, RmcColorSetting.UI_Neutral),
        };

        return format.ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return $"{"Current map mode".L()}: {ModeManager.CurrentMode().ModeName.ToString().ToCleanName().L()}".ToNeutralTextFormat();
    }
}
