using MapChanger.UI;

namespace RandoMapCore.UI;

internal class PinOptionsGrid : RmcOptionsGrid
{
    protected override IEnumerable<ExtraButton> GetButtons()
    {
        return Interop.HasMoreStags ? [new ClearedButton(), new ReachablePinsButton(), new QMarkSettingButton(), new StagSpriteButton()]
            : [new ClearedButton(), new ReachablePinsButton(), new QMarkSettingButton()];
    }
}
