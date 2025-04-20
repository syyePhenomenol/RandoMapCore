namespace RandoMapCore.Input;

internal class MapKeyInput : MapUIKeyInput
{
    internal MapKeyInput()
        : base("Toggle Map Key", UnityEngine.KeyCode.K)
    {
        Instance = this;
    }

    internal static MapKeyInput Instance { get; private set; }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.ToggleMapKey();
        base.DoAction();
    }
}
