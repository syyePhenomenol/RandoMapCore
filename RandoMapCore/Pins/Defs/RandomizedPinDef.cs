using ItemChanger;
using RandomizerCore.Logic;

namespace RandoMapCore.Pins;

internal class RandomizedPinDef(
    AbstractPlacement placement,
    ProgressionManager pm,
    ProgressionManager pmNoSequenceBreak
) : ICLogicPinDef(placement, "Randomized", pm, pmNoSequenceBreak) { }
