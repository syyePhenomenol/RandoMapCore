using ItemChanger;
using MagicUI.Elements;
using MapChanger;
using RandoMapCore.Pins.Defs;
using RandoMapCore.Settings;

namespace RandoMapCore.Pins;

internal sealed class RandomizedStagPinDef(AbstractPlacement placement, PoolsCollection poolsCollection) : ICLogicPinDef(placement, poolsCollection)
{
    internal bool IsActiveStag { get; private set; }
    internal bool IsOpenedStag { get; private set; }

    internal override void Update()
    {
        base.Update();

        IsOpenedStag = RandoMapCoreMod.Data.IsOpenedCustomStag(Placement.Name);
        IsActiveStag = RandoMapCoreMod.GS.ShowCustomStags;
    }

    internal override bool ActiveByCurrentMode()
    {
        return IsActiveStag || base.ActiveByCurrentMode();
    }

    internal override bool ActiveBySettings()
    {
        return IsActiveStag || base.ActiveBySettings();
    }

    internal override bool ActiveByProgress()
    {
        return IsActiveStag || base.ActiveByProgress();
    }

    internal override IEnumerable<ScaledPinSprite> GetPinSprites()
    {
        return IsActiveStag ? [RmcPinManager.Psm.GetSprite("Stags")] : base.GetPinSprites();
    }

    internal override PinShape GetMixedPinShape()
    {
        return IsActiveStag ? PinShape.Square : base.GetMixedPinShape();
    }

    internal override float GetZPriority()
    {
        return IsActiveStag ? -1f : base.GetZPriority();
    }

    private protected override RunCollection GetStatusText()
    {
        return
        [
            .. base.GetStatusText(),
            new Run(", "),
            new Run((IsOpenedStag ? "station open" : "station locked").L()),
        ];
    }
}
