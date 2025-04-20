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
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        this.SetButtonBoolToggle($"{"Spoilers".L()}:\n", RandoMapCoreMod.LS.SpoilerOn);
    }
}
