using MagicUI.Elements;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class GroupByButton() : BorderlessExtraButton(nameof(GroupByButton))
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            RandoMapCoreMod.LS.ToggleGroupBy();
        }
    }

    protected override void OnHover()
    {
        if (!RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            RmcTitle.Instance.HoveredText = "Toggle disabled".L();
            return;
        }

        RmcTitle.Instance.HoveredText = "Group pools by either location (normal) or by item (spoilers).".L();
    }

    public override void Update()
    {
        var text = $"{"Group by".L()}:\n";

        if (!RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            Button.Content = text + "Location".L();
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
            return;
        }

        switch (RandoMapCoreMod.LS.GroupBy)
        {
            case GroupBySetting.Location:
                text += "Location".L();
                break;

            case GroupBySetting.Item:
                text += "Item".L();
                break;
            default:
                break;
        }

        Button.Content = text;
        Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Special);
    }
}
