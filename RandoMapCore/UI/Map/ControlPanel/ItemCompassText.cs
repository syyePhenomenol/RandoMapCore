using MapChanger;
using MapChanger.UI;
using RandoMapCore.Input;

namespace RandoMapCore.UI;

internal class ItemCompassText : ControlPanelText
{
    private protected override TextFormat GetTextFormat()
    {
        var text = $"{"Toggle item compass".L()} {ToggleItemCompassInput.Instance.GetBindingsText()}: ";

        return (
            RandoMapCoreMod.GS.ItemCompassOn
                ? (text + "On".L(), RmcColorSetting.UI_On)
                : (text + "Off".L(), RmcColorSetting.UI_Neutral)
        ).ToTextFormat();
    }
}
