using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class ReachablePinsButton() : BorderlessExtraButton(nameof(ReachablePinsButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleShowReachablePins();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Toggle how reachable locations are displayed.".L();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        var text =
            $"{"Reachable pins".L()}:\n"
            + RandoMapCoreMod.GS.ShowReachablePins switch
            {
                Settings.ReachablePinsSetting.HideUnreachable => "hide unreachable".L(),
                Settings.ReachablePinsSetting.ExpandReachable => "expand reachable".L(),
                Settings.ReachablePinsSetting.ExpandAll => "expand all".L(),
                _ => "",
            };

        Button.Content = text;
        Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }
}
