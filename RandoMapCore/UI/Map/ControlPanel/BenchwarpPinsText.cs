using MapChanger;
using MapChanger.UI;
using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class BenchwarpPinsText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return UIExtensions.GetBoolTextFormat(
            $"{"Benchwarp pins".L()} {ToggleBenchwarpPinsInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.GS.ShowBenchwarpPins
        );
    }
}
