namespace RandoMapCore.Input;

internal class ToggleSizeInput : PinHotkeyInput
{
    internal ToggleSizeInput()
        : base("Toggle Pin Size", UnityEngine.KeyCode.Alpha5)
    {
        Instance = this;
    }

    internal static ToggleSizeInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && RandoMapCoreMod.Data.EnableVisualCustomization;
    }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.TogglePinSize();
        base.DoAction();
    }
}
