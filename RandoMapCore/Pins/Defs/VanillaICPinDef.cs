using ItemChanger;
using RandomizerCore.Logic;

namespace RandoMapCore.Pins;

// A non-randomized ItemChanger placement pin
internal sealed class VanillaICPinDef(
    AbstractPlacement placement,
    ProgressionManager pm,
    ProgressionManager pmNoSequenceBreak
) : LogicICPinDef(placement, "Vanilla", pm, pmNoSequenceBreak)
{
    internal override float GetZPriority()
    {
        return base.GetZPriority() + 50f;
    }
}
