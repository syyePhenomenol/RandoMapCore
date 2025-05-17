using MapChanger.UI;
using RandoMapCore.Input;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class PinSelectionText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return UIExtensions.GetBoolTextFormat(
            $"{"Toggle pin selection".L()} {PinPanelInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.GS.PinSelectionOn
        );
    }
}
