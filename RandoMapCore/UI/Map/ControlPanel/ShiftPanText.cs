using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class ShiftPanText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return $"{"Hold".L()} [Shift]: {"Pan faster".L()}".ToNeutralTextFormat();
    }
}
