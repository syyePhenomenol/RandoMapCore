using ItemChanger;
using RandoMapCore.Settings;

namespace RandoMapCore.Pins;

internal sealed class RandomizedStagPinDef: RandomizedPinDef
{
    public RandomizedStagPinDef(AbstractPlacement placement)
        : base(placement)
    {
        Stag = new(placement.Name);
    }

    internal StagInfo Stag { get; }

    internal override void Update()
    {
        base.Update();
        Stag.Update();
    }

    internal override bool ActiveByCurrentMode()
    {
        return DoOverride() || base.ActiveByCurrentMode();
    }

    internal override bool ActiveBySettings()
    {
        return DoOverride() || base.ActiveBySettings();
    }

    internal override bool ActiveByProgress()
    {
        return DoOverride() || base.ActiveByProgress();
    }

    internal override IEnumerable<ScaledPinSprite> GetPinSprites()
    {
        return DoOverride() ? [RmcPinManager.Psm.GetSprite("Stags")] : base.GetPinSprites();
    }

    internal override PinShape GetMixedPinShape()
    {
        return DoOverride() ? PinShape.Circle : base.GetMixedPinShape();
    }

    internal override float GetZPriority()
    {
        return DoOverride() ? -1f : base.GetZPriority();
    }

    private bool DoOverride()
    {
        return Stag.IsActiveStag && RandoMapCoreMod.GS.StagSprite == StagSpriteSetting.AlwaysStag;
    }
}
