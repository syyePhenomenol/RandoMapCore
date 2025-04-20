namespace RandoMapCore.Input;

internal class ProgressHintPanelInput : MapUIKeyInput
{
    internal ProgressHintPanelInput()
        : base("Toggle Progress Hint Panel", UnityEngine.KeyCode.G)
    {
        Instance = this;
    }

    internal static ProgressHintPanelInput Instance { get; private set; }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.ToggleProgressHint();
        base.DoAction();
    }
}
