using MagicUI.Elements;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class QuickMapCompassesButton() : BorderlessExtraButton(nameof(QuickMapCompassesButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleQuickMapCompasses();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Show compasses pointing to transitions in room.".L();
    }

    public override void Update()
    {
        var text = $"{"Quick map\n compass".L()}: ";

        switch (RandoMapCoreMod.GS.ShowQuickMapCompasses)
        {
            case QuickMapCompassSetting.Unchecked:
                text += "Unchecked".L();
                break;
            case QuickMapCompassSetting.All:
                text += "All".L();
                break;
            default:
                break;
        }

        Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        Button.Content = text;
    }
}
