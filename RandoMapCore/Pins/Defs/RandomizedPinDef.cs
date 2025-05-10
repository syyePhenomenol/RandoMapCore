using ItemChanger;
using RandomizerCore.Logic;

namespace RandoMapCore.Pins;

internal class RandomizedPinDef(
    AbstractPlacement placement,
    ProgressionManager pm,
    ProgressionManager pmNoSequenceBreak
) : ICLogicPinDef(placement, "Randomized", pm, pmNoSequenceBreak)
{
    private protected override string GetPreviewText()
    {
        return base.GetPreviewText()
            ?.Replace("Pay ", "")
            ?.Replace("Once you own ", "")
            ?.Replace(", I'll gladly sell it to you.", "")
            ?.Replace("Requires ", "");
    }
}
