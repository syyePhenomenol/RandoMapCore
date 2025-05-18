using RandoMapCore.Modes;
using RandoMapCore.Pathfinder;

namespace RandoMapCore.Input;

internal class PathfinderBenchwarpInput : MapUIKeyInput
{
    internal PathfinderBenchwarpInput()
        : base("Toggle Pathfinder Benchwarp", UnityEngine.KeyCode.B)
    {
        Instance = this;
    }

    internal static PathfinderBenchwarpInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition()
            && RandoMapCoreMod.Data.EnableRoomSelection
            && RandoMapCoreMod.Data.EnablePathfinder
            && Interop.HasBenchwarp;
    }

    public override bool ActiveCondition()
    {
        return Conditions.TransitionRandoModeEnabled();
    }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.TogglePathfinderBenchwarp();
        RmcPathfinder.RM.ResetRoute();
        base.DoAction();
    }
}
