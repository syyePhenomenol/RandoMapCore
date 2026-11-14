using ItemChanger;
using MagicUI.Elements;
using MapChanger;
using RandoMapCore.Settings;

namespace RandoMapCore.Pins;

internal sealed class RandomizedBenchPinDef : RandomizedPinDef
{
    public RandomizedBenchPinDef(AbstractPlacement placement)
        : base(placement)
    {
        Bench = new(placement.Name);

        if (RandoMapCoreMod.Data.EnableMapBenchwarp)
        {
            TextBuilders.Add(Bench.GetBenchwarpText);
        }
    }

    internal BenchInfo Bench { get; }

    internal override void Update()
    {
        base.Update();
        Bench.Update();
    }

    internal override bool ActiveByCurrentMode()
    {
        return Bench.IsActiveBench || base.ActiveByCurrentMode();
    }

    internal override bool ActiveBySettings()
    {
        return Bench.IsActiveBench || base.ActiveBySettings();
    }

    internal override bool ActiveByProgress()
    {
        return Bench.IsActiveBench || base.ActiveByProgress();
    }

    internal override IEnumerable<ScaledPinSprite> GetPinSprites()
    {
        return Bench.IsActiveBench ? [RmcPinManager.Psm.GetSprite("Benches")] : base.GetPinSprites();
    }

    internal override PinShape GetMixedPinShape()
    {
        return Bench.IsActiveBench ? PinShape.Square : base.GetMixedPinShape();
    }

    internal override float GetZPriority()
    {
        return Bench.IsActiveBench ? -1f : base.GetZPriority();
    }

    private protected override RunCollection GetStatusText()
    {
        return
        [
            .. base.GetStatusText(),
            new Run(", "),
            new Run((Bench.IsVisitedBench ? "can warp" : "cannot warp").L()),
        ];
    }
}
