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
        return base.UseCondition() && Interop.HasBenchwarp;
    }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.ToggleAllowBenchWarpSearch();
        RmcPathfinder.RM.ResetRoute();
        base.DoAction();
    }
}
