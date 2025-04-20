using MapChanger.UI;

namespace RandoMapCore.UI;

internal class PinOptionsPanel : RmcOptionsPanel
{
    internal PinOptionsPanel()
        : base(nameof(PinOptionsPanel), GetButtons())
    {
        Instance = this;
    }

    internal static PinOptionsPanel Instance { get; private set; }

    private static IEnumerable<ExtraButton> GetButtons()
    {
        return [new ClearedButton(), new ReachablePinsButton(), new QMarkSettingButton()];
    }
}
