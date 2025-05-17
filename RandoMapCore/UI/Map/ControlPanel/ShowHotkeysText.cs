using MapChanger.UI;
using RandoMapCore.Input;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class ShowHotkeysText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return $"{ControlPanelInput.Instance.GetBindingsText()}: {(RandoMapCoreMod.GS.ControlPanelOn ? "Hide hotkeys" : "More hotkeys").L()}".ToNeutralTextFormat();
    }
}
