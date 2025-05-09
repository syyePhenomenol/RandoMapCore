namespace RandoMapCore.Input;

internal class ToggleItemCompassInput : MapUIKeyInput
{
    internal ToggleItemCompassInput()
        : base("Toggle Item Compass", UnityEngine.KeyCode.C)
    {
        Instance = this;
    }

    internal static ToggleItemCompassInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && RandoMapCoreMod.Data.EnableItemCompass;
    }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.ToggleItemCompass();
        base.DoAction();
    }
}
