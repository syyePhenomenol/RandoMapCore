using MapChanger;
using MapChanger.UI;
using RandoMapCore.Input;

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
