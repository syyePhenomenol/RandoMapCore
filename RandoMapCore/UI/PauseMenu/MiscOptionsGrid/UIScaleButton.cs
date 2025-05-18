using MapChanger;
using MapChanger.UI;

namespace RandoMapCore.UI;

internal class UIScaleButton : ExtraButton
{
    protected override void OnClick()
    {
        MapChangerMod.GS.ToggleUIScale();
    }

    protected override TextFormat GetTextFormat()
    {
        return $"{"UI Scale".L()}\n{MapChangerMod.GS.UIScale:n1} ×".ToNeutralTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return $"{"Set the scaling for all UI.".L()}".ToNeutralTextFormat();
    }
}
