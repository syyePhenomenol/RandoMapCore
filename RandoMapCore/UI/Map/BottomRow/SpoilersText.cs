using MapChanger.UI;
using RandoMapCore.Input;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class SpoilersText : BottomRowText
{
    protected override TextFormat GetTextFormat()
    {
        return UIExtensions.GetBoolTextFormat(
            $"{"Spoilers".L()} {ToggleSpoilersInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.LS.SpoilerOn
        );
    }
}
