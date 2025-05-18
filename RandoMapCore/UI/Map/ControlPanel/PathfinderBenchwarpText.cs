using MapChanger.UI;
using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class PathfinderBenchwarpText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        return UIExtensions.GetBoolTextFormat(
            $"{"Pathfinder benchwarp".L()} {PathfinderBenchwarpInput.Instance.GetBindingsText()}: ",
            RandoMapCoreMod.GS.PathfinderBenchwarp
        );
    }
}
