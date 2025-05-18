using MapChanger.UI;
using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class PathfinderDreamgateText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return UIExtensions.GetBoolTextFormat(
            $"{"Pathfinder dreamgate".L()} {PathfinderDreamgateInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.GS.PathfinderDreamgate
        );
    }
}
