namespace RandoMapCore.Input;

internal class RoomPanelInput : MapUIKeyInput
{
    internal RoomPanelInput()
        : base("Toggle Room Panel", UnityEngine.KeyCode.R)
    {
        Instance = this;
    }

    internal static RoomPanelInput Instance { get; private set; }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.ToggleRoomSelection();
        base.DoAction();
    }
}
