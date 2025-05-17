using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class ProgressHintInput : RmcWorldMapInput
{
    internal ProgressHintInput()
        : base("Show Progress Hint", () => InputHandler.Instance.inputActions.superDash)
    {
        Instance = this;
    }

    internal static ProgressHintInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && RandoMapCoreMod.Data.EnableProgressionHints;
    }

    public override bool ActiveCondition()
    {
        return base.ActiveCondition() && RandoMapCoreMod.GS.ProgressHint is not Settings.ProgressHintSetting.Off;
    }

    public override void DoAction()
    {
        ProgressHintPanel.Instance?.UpdateNewHint();
    }
}
