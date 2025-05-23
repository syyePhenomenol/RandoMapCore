using MapChanger;
using MapChanger.UI;
using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class ShowHotkeysText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return $"{ControlPanelInput.Instance.GetBindingsText()}: {(RandoMapCoreMod.GS.ControlPanelOn ? "Hide hotkeys" : "More hotkeys").L()}".ToNeutralTextFormat();
    }
}
