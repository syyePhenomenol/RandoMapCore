using RandoMapCore.Pathfinder;
using RandoMapCore.Pathfinder.Actions;
using RandoMapCore.Pins;

namespace RandoMapCore.Input;

internal class BenchwarpInput : RmcWorldMapInput
{
    internal BenchwarpInput()
        : base("World Map Benchwarp", () => InputHandler.Instance.inputActions.attack, 500f)
    {
        Instance = this;
    }

    internal static BenchwarpInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && Interop.HasBenchwarp;
    }

    public override bool ActiveCondition()
    {
        return base.ActiveCondition() && (RandoMapCoreMod.GS.PinSelectionOn || RandoMapCoreMod.GS.RoomSelectionOn);
    }

    public override void DoAction()
    {
        if (PinSelector.Instance.SelectedObject is RmcPin pin && BenchwarpInterop.IsVisitedBench(pin.Name))
        {
            _ = GameManager.instance.StartCoroutine(BenchwarpInterop.DoBenchwarp(pin.Name));
        }
        else if (
            PinSelector.Instance.SelectedObject is PinCluster pinCluster
            && BenchwarpInterop.IsVisitedBench(pinCluster.SelectedPin.Name)
        )
        {
            _ = GameManager.instance.StartCoroutine(BenchwarpInterop.DoBenchwarp(pinCluster.SelectedPin.Name));
        }
        else if (TryGetBenchwarpFromRoute(out var benchKey))
        {
            _ = GameManager.instance.StartCoroutine(BenchwarpInterop.DoBenchwarp(benchKey));
        }
    }

    internal static bool TryGetBenchwarpFromRoute(out RmcBenchKey key)
    {
        if (
            RmcPathfinder.RM.CurrentRoute is Route currentRoute
            && currentRoute.CurrentInstruction is BenchwarpAction ba
        )
        {
            key = ba.BenchKey;
            return true;
        }

        key = default;
        return false;
    }
}
