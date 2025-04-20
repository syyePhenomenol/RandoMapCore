namespace RandoMapCore.Input;

internal class ToggleItemCompassInput : MapUIKeyInput
{
    internal ToggleItemCompassInput()
        : base("Toggle Item Compass", UnityEngine.KeyCode.C)
    {
        Instance = this;
    }

    internal static ToggleItemCompassInput Instance { get; private set; }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.ToggleItemCompass();
        base.DoAction();
    }
}
