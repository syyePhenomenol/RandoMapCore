using MapChanger.UI;
using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class RoomSelectionText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return UIExtensions.GetBoolTextFormat(
            $"{"Toggle room selection".L()} {RoomPanelInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.GS.RoomSelectionOn
        );
    }
}
