using MapChanger.UI;

namespace RandoMapCore.UI;

internal class PoolOptionsPanel : RmcOptionsPanel
{
    internal PoolOptionsPanel()
        : base(nameof(PoolOptionsPanel), GetButtons())
    {
        Instance = this;
    }

    internal static PoolOptionsPanel Instance { get; private set; }

    private static IEnumerable<ExtraButton> GetButtons()
    {
        return RandoMapCoreMod
            .LS.AllPoolGroups.Select(p => new PoolButton(p))
            .Cast<ExtraButton>()
            .Append(new GroupByButton());
    }
}
