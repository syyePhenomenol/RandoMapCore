using MapChanger;
using MapChanger.UI;

namespace RandoMapCore.UI;

internal class StagSpriteButton: ExtraButton
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleCustomStags();
    }

    protected override TextFormat GetTextFormat()
    {
        return UIExtensions.GetBoolTextFormat($"{"Custom Stags".L()}:\n", RandoMapCoreMod.GS.ShowCustomStags);
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "Always show unlocked custom stags".L().ToNeutralTextFormat();
    }
}
