using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class SpoilersButton() : MainButton(nameof(SpoilersButton), RandoMapCoreMod.Data.ModName, 0, 3)
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            RandoMapCoreMod.LS.ToggleSpoilers();
        }
    }

    protected override void OnHover()
    {
        if (!RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            RmcTitle.Instance.HoveredText = "Toggle disabled".L();
            return;
        }

        RmcTitle.Instance.HoveredText = "Reveals the items at each location.".L();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        var text = $"{"Spoilers".L()}:\n";

        if (!RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            Button.Content = text + "Off".L();
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
            return;
        }

        this.SetButtonBoolToggle(text, RandoMapCoreMod.LS.SpoilerOn);
    }
}
