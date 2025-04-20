using MapChanger.UI;

namespace RandoMapCore.UI;

internal abstract class RmcOptionsPanel : ExtraButtonPanel
{
    internal RmcOptionsPanel(string name, IEnumerable<ExtraButton> buttons)
        : base(name, RandoMapCoreMod.Data.ModName, buttons, 390f, 10) { }
}
