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
        if (Bench.IsActiveBench)
        {
            return true;
        }

        return base.ActiveByCurrentMode();
    }

    internal override bool ActiveBySettings()
    {
        if (Bench.IsActiveBench)
        {
            return true;
        }

        return base.ActiveBySettings();
    }

    internal override bool ActiveByProgress()
    {
        if (Bench.IsActiveBench)
        {
            return true;
        }

        return base.ActiveByProgress();
    }

    internal override IEnumerable<ScaledPinSprite> GetPinSprites()
    {
        if (Bench.IsActiveBench)
        {
            return [RmcPinManager.Psm.GetSprite("Benches")];
        }

        return base.GetPinSprites();
    }

    internal override PinShape GetMixedPinShape()
    {
        if (Bench.IsActiveBench)
        {
            return PinShape.Square;
        }

        return base.GetMixedPinShape();
    }

    internal override float GetZPriority()
    {
        if (Bench.IsActiveBench)
        {
            return -1f;
        }

        return base.GetZPriority();
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
