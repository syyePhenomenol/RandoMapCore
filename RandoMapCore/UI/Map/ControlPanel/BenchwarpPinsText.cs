using MapChanger.UI;
using RandoMapCore.Input;
using RandoMapCore.Localization;

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
