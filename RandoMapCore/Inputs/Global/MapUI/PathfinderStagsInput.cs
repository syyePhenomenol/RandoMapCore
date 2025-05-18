using MapChanger.Map;
using RandoMapCore.Modes;
using RandoMapCore.Pathfinder;
using RandoMapCore.Rooms;

namespace RandoMapCore.Input;

internal class PathfinderStagsInput : MapUIKeyInput
{
    internal PathfinderStagsInput()
        : base("Toggle Pathfinder Stags", UnityEngine.KeyCode.S)
    {
        Instance = this;
    }

    internal static PathfinderStagsInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder;
    }

    public override bool ActiveCondition()
    {
        return Conditions.TransitionRandoModeEnabled();
    }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.TogglePathfinderStags();
        RmcPathfinder.RM.ResetRoute();
        RmcPathfinder.Slt.Update();

        foreach (var room in BuiltInObjects.MappedRooms.Values)
        {
            room.UpdateColor();
        }

        foreach (var roomText in RmcRoomManager.RoomTexts.Values)
        {
            roomText.UpdateColor();
        }

        base.DoAction();
    }
}
