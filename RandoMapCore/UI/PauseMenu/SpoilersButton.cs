using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class SpoilersButton() : MainButton(nameof(SpoilersButton), RandoMapCoreMod.Data.ModName, 0, 3)
{
    protected override void OnClick()
    {
        RandoMapCoreMod.LS.ToggleSpoilers();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Reveals the items at each location.".L();

        if (!RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            RmcTitle.Instance.HoveredText += $" {"Toggle disabled".L()}";
        }
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        if (!RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            Button.Content = $"{"Spoilers".L()}:\nDisabled".L();
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
        }
        else
        {
            this.SetButtonBoolToggle($"{"Spoilers".L()}:\n", RandoMapCoreMod.LS.SpoilerOn);
        }
    }
}
