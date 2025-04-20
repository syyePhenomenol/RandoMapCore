using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class ReachablePinsButton() : BorderlessExtraButton(nameof(ReachablePinsButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleReachablePins();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        this.SetButtonBoolToggle($"{"Indicate\nreachable".L()}: ", RandoMapCoreMod.GS.ReachablePins);
    }
}
