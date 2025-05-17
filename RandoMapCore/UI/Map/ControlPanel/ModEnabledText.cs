using MapChanger.Input;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class ModEnabledText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return $"{ModEnabledInput.Instance.GetBindingsText()}: {"Disable mod".L()}".ToNeutralTextFormat();
    }
}
