using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class AreaNamesButton() : BorderlessExtraButton(nameof(AreaNamesButton))
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RandoMapCoreMod.GS.ToggleAreaNames();
        }
    }

    protected override void OnHover()
    {
        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RmcTitle.Instance.HoveredText = "Toggle disabled".L();
            return;
        }

        RmcTitle.Instance.HoveredText = "Show area names on the world/quick map.".L();
    }

    public override void Update()
    {
        var text = $"{"Show area\nnames".L()}: ";

        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            Button.Content = text + "On".L();
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
            return;
        }

        this.SetButtonBoolToggle(text, RandoMapCoreMod.GS.ShowAreaNames);
    }
}
