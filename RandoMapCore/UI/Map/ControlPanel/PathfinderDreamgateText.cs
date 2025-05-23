using MagicUI.Core;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Input;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class PathfinderDreamgateText : ControlPanelText
{
    public override void Update()
    {
        if (!Conditions.TransitionRandoModeEnabled())
        {
            Text.Visibility = Visibility.Collapsed;
            return;
        }

        base.Update();
    }

    private protected override TextFormat GetTextFormat()
    {
        return UIExtensions.GetBoolTextFormat(
            $"{"Pathfinder dreamgate".L()} {PathfinderDreamgateInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.GS.PathfinderDreamgate
        );
    }
}
