using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class AreaNamesButton() : BorderlessExtraButton(nameof(AreaNamesButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleAreaNames();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Show area names on the world/quick map.";
    }

    public override void Update()
    {
        this.SetButtonBoolToggle($"{"Show area\nnames".L()}: ", RandoMapCoreMod.GS.ShowAreaNames);
    }
}
