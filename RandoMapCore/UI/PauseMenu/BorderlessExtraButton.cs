using MagicUI.Core;
using MagicUI.Elements;
using MapChanger.UI;

namespace RandoMapCore.UI;

internal abstract class BorderlessExtraButton(string name) : ExtraButton(name, RandoMapCoreMod.Data.ModName)
{
    protected override Button MakeButton(LayoutRoot root)
    {
        var button = base.MakeButton(root);
        button.Borderless = true;
        return button;
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }
}
