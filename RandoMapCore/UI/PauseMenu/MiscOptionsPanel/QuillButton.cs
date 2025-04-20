using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class QuillButton() : BorderlessExtraButton(nameof(QuillButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleAlwaysHaveQuill();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Doesn't affect Full Map and Transition modes.".L();
    }

    public override void Update()
    {
        this.SetButtonBoolToggle($"{"Always have\nQuill".L()}: ", RandoMapCoreMod.GS.AlwaysHaveQuill);
    }
}
