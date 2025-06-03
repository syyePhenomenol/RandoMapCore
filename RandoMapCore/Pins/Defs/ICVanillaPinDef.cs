using ItemChanger;

namespace RandoMapCore.Pins;

// A non-randomized ItemChanger placement pin
internal sealed class ICVanillaPinDef(AbstractPlacement placement) : ICLogicPinDef(placement, "Vanilla")
{
    internal override float GetZPriority()
    {
        return base.GetZPriority() + 50f;
    }
}
