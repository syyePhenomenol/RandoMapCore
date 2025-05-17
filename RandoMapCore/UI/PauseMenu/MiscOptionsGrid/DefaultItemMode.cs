using MapChanger;
using MapChanger.UI;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class DefaultItemModeButton : ExtraButton
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleDefaultItemRandoMode();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return $"{"Default map mode when starting a new item rando save".L()}: {RandoMapCoreMod.GS.DefaultItemRandoMode.ToString().ToCleanName().L()}".ToNeutralTextFormat();
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Def. Item\nMode".L()}: ";

        return (
            RandoMapCoreMod.GS.DefaultItemRandoMode switch
            {
                RmcMode.Full_Map => (text + "FM".L(), RmcColorSetting.UI_On),
                RmcMode.All_Pins => (text + "AP".L(), RmcColorSetting.UI_Neutral),
                RmcMode.Pins_Over_Area => (text + "POA".L(), RmcColorSetting.UI_Neutral),
                RmcMode.Pins_Over_Room => (text + "POR".L(), RmcColorSetting.UI_Neutral),
                RmcMode.Transition_Normal => (text + "T1".L(), RmcColorSetting.UI_Special),
                RmcMode.Transition_Visited_Only => (text + "T2".L(), RmcColorSetting.UI_Special),
                RmcMode.Transition_All_Rooms => (text + "T3".L(), RmcColorSetting.UI_Special),
                _ => (text, RmcColorSetting.UI_Neutral),
            }
        ).ToTextFormat();
    }
}
