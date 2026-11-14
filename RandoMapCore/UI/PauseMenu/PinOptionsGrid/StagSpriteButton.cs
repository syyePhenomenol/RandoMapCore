using MapChanger;
using MapChanger.UI;

namespace RandoMapCore.UI;

internal class StagSpriteButton: ExtraButton
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleStagSpriteSetting();
    }

    protected override TextFormat GetTextFormat()
    {
        return (
            $"{"Stag Sprite".L()}:\n" + RandoMapCoreMod.GS.StagSprite.ToString().ToWhitespaced().L()
        ).ToNeutralTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "Always show stag pin for MoreStags locations".L().ToNeutralTextFormat();
    }
}
