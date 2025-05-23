using MagicUI.Core;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Input;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class RoomSelectionText : ControlPanelText
{
    public override void Update()
    {
        if (!Conditions.TransitionRandoModeEnabled())
        {
            Text.Visibility = Visibility.Collapsed;
            return;
        }

        base.Update();
    }

    private protected override TextFormat GetTextFormat()
    {
        return UIExtensions.GetBoolTextFormat(
            $"{"Toggle room selection".L()} {RoomPanelInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.GS.RoomSelectionOn
        );
    }
}
