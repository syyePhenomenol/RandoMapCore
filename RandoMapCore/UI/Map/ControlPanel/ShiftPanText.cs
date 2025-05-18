using MapChanger.UI;

namespace RandoMapCore.UI;

internal class ShiftPanText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return $"{"Hold".L()} [Shift]: {"Pan faster".L()}".ToNeutralTextFormat();
    }
}
