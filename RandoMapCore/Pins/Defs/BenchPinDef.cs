using MapChanger.Defs;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.Pins;

internal class BenchPinDef : PinDef
{
    internal BenchInfo Bench { get; }

    internal BenchPinDef(string name, string sceneName)
        : base()
    {
        Name = name;
        LocationPoolGroups = ["Benches"];
        ItemPoolGroups = ["Benches"];

        SceneName = sceneName;

        var mrp = GetBenchMapPosition();
        MapPosition = mrp;
        MapZone = mrp.MapZone;

        Bench = new(name);

        TextBuilders.Add(Bench.GetBenchwarpText);
    }

    private protected virtual MapRoomPosition GetBenchMapPosition()
    {
        return RmcPinManager.Dpm.GetDefaultMapPosition(Name);
    }

    internal override void Update()
    {
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
        return RandoMapCoreMod.GS.ShowBenchwarpPins;
    }

    internal override bool ActiveByProgress()
    {
        return true;
    }

    internal override IEnumerable<ScaledPinSprite> GetPinSprites()
    {
        return [RmcPinManager.Psm.GetSprite("Benches")];
    }

    internal override bool ShrinkPin()
    {
        return !Bench.IsVisitedBench;
    }

    internal override bool DarkenPin()
    {
        return !Bench.IsVisitedBench;
    }

    internal override PinShape GetMixedPinShape()
    {
        return PinShape.Square;
    }

    internal override float GetZPriority()
    {
        if (Bench.IsActiveBench)
        {
            return -1f;
        }

        return base.GetZPriority();
    }

    private protected override string GetStatusText()
    {
        return $"{"Status".L()}: {(Bench.IsVisitedBench ? "Can warp" : "Cannot warp").L()}";
    }
}
