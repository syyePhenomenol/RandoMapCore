using MapChanger;
using MapChanger.Input;
using MapChanger.UI;

namespace RandoMapCore.UI;

internal class ModEnabledText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return $"{ModEnabledInput.Instance.GetBindingsText()}: {"Disable mod".L()}".ToNeutralTextFormat();
    }
}
