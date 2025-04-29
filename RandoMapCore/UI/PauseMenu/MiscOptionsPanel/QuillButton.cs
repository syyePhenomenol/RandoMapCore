using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class QuillButton() : BorderlessExtraButton(nameof(QuillButton))
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RandoMapCoreMod.GS.ToggleAlwaysHaveQuill();
        }
    }

    protected override void OnHover()
    {
        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RmcTitle.Instance.HoveredText = "Toggle disabled".L();
            return;
        }

        RmcTitle.Instance.HoveredText = "Doesn't affect Full Map and Transition modes.".L();
    }

    public override void Update()
    {
        var text = $"{"Always have\nQuill".L()}: ";

        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            Button.Content = text + "On".L();
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
            return;
        }

        this.SetButtonBoolToggle(text, RandoMapCoreMod.GS.AlwaysHaveQuill);
    }
}
