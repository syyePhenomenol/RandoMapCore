using MapChanger;
using MapChanger.UI;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class GroupByButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            RandoMapCoreMod.LS.ToggleGroupBy();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Group By".L()}:\n";

        if (!RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            return (
                text + GroupBySetting.Location.ToString().ToWhitespaced().L(),
                RmcColorSetting.UI_Disabled
            ).ToTextFormat();
        }

        return (
            text + RandoMapCoreMod.LS.GroupBy.ToString().ToWhitespaced().L(),
            RmcColorSetting.UI_Special
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableSpoilerToggle
                ? "Group pools by either location (normal) or by item (spoilers).".L()
                : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
