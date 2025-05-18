using MagicUI.Core;
using MapChanger.UI;
using RandoMapCore.Input;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class PathfinderStagsText : ControlPanelText
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
            $"{"Pathfinder stags".L()} {PathfinderStagsInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.GS.PathfinderStags
        );
    }
}
