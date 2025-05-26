using MagicUI.Elements;
using MapChanger;
using RandoMapCore.Input;

namespace RandoMapCore.Pins;

internal class BenchInfo(string name)
{
    internal bool IsActiveBench { get; private set; }
    internal bool IsVisitedBench { get; private set; }

    internal void Update()
    {
        IsVisitedBench = BenchwarpInterop.IsVisitedBench(name);
        IsActiveBench = RandoMapCoreMod.GS.ShowBenchwarpPins && IsVisitedBench;
    }

    internal RunCollection GetBenchwarpText()
    {
        if (!IsActiveBench)
        {
            return null;
        }

        var bindingsText = BenchwarpInput.Instance.GetBindingsText();
        return [
            new Run($"{"Hold".L()} "),
            new Run(bindingsText),
            new Run($"{"to benchwarp".L()}.")
        ];
    }
}
