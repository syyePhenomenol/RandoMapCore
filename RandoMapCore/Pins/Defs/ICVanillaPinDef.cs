using ItemChanger;
using RandomizerCore.Logic;

namespace RandoMapCore.Pins;

// A non-randomized ItemChanger placement pin
internal sealed class ICVanillaPinDef(
    AbstractPlacement placement,
    ProgressionManager pm,
    ProgressionManager pmNoSequenceBreak
) : ICLogicPinDef(placement, "Vanilla", pm, pmNoSequenceBreak)
{
    internal override float GetZPriority()
    {
        return base.GetZPriority() + 50f;
    }
}
