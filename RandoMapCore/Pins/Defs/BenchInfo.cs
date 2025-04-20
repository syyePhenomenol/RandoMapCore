using RandoMapCore.Input;
using RandoMapCore.Localization;

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

    internal string GetBenchwarpText()
    {
        if (!IsActiveBench)
        {
            return null;
        }

        var bindingsText = BenchwarpInput.Instance.GetBindingsText();
        return $"{"Hold".L()} {bindingsText} {"to benchwarp".L()}.";
    }
}
