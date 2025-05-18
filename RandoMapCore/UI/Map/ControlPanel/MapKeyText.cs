using MapChanger.UI;
using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class MapKeyText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return UIExtensions.GetBoolTextFormat(
            $"{"Toggle map key".L()} {MapKeyInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.GS.MapKeyOn
        );
    }
}
