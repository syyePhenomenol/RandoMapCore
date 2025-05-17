using MapChanger.UI;

namespace RandoMapCore.UI;

internal class PoolOptionsGrid : RmcOptionsGrid
{
    protected override IEnumerable<ExtraButton> GetButtons()
    {
        return RandoMapCoreMod
            .LS.AllPoolGroups.Select(p => new PoolButton(p))
            .Cast<ExtraButton>()
            .Append(new GroupByButton());
    }
}
