using MapChanger.UI;
using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class PathfinderStagsText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return UIExtensions.GetBoolTextFormat(
            $"{"Pathfinder stags".L()} {PathfinderStagsInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.GS.PathfinderStags
        );
    }
}
