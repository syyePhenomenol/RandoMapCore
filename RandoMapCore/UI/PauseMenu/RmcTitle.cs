using MapChanger.UI;

namespace RandoMapCore.UI;

internal class RmcTitle : PauseMenuTitle
{
    protected override TextFormat GetDefaultTextFormat()
    {
        return Layout.Mod.L().ToNeutralTextFormat();
    }
}
