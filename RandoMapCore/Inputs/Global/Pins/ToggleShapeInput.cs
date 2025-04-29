namespace RandoMapCore.Input;

internal class ToggleShapeInput : PinHotkeyInput
{
    internal ToggleShapeInput()
        : base("Toggle Pin Shape", UnityEngine.KeyCode.Alpha4)
    {
        Instance = this;
    }

    internal static ToggleShapeInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && RandoMapCoreMod.Data.EnableVisualCustomization;
    }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.TogglePinShape();
        base.DoAction();
    }
}
