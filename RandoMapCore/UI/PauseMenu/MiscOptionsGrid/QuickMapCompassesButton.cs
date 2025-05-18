using MapChanger.UI;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class QuickMapCompassesButton : ExtraButton
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleQuickMapCompasses();
    }

    protected override TextFormat GetTextFormat()
    {
        return (
            $"{"Quick Map\n Compass".L()}: "
            + RandoMapCoreMod.GS.ShowQuickMapCompasses switch
            {
                QuickMapCompassSetting.Unchecked => "Unchecked".L(),
                QuickMapCompassSetting.All => "All".L(),
                _ => "",
            }
        ).ToNeutralTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return "Show compasses pointing to transitions in room.".L().ToNeutralTextFormat();
    }
}
