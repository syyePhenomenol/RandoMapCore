using MagicUI.Core;
using MapChanger.UI;
using RandoMapCore.Input;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class PathfinderBenchwarpText : ControlPanelText
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
            $"{"Pathfinder benchwarp".L()} {PathfinderBenchwarpInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.GS.PathfinderBenchwarp
        );
    }
}
