using RandoMapCore.Modes;
using RandoMapCore.Pathfinder;

namespace RandoMapCore.Input;

internal class PathfinderDreamgateInput : MapUIKeyInput
{
    internal PathfinderDreamgateInput()
        : base("Toggle Pathfinder Dreamgate", UnityEngine.KeyCode.D)
    {
        Instance = this;
    }

    internal static PathfinderDreamgateInput Instance { get; private set; }

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
        RandoMapCoreMod.GS.TogglePathfinderDreamgate();
        RmcPathfinder.RM.ResetRoute();
        base.DoAction();
    }
}
