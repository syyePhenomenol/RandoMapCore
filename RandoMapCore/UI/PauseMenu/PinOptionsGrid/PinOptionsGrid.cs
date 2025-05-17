using MapChanger.UI;

namespace RandoMapCore.UI;

internal class PinOptionsGrid : RmcOptionsGrid
{
    protected override IEnumerable<ExtraButton> GetButtons()
    {
        return [new ClearedButton(), new ReachablePinsButton(), new QMarkSettingButton()];
    }
}
