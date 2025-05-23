using MapChanger;
using MapChanger.UI;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class ItemCompassModeButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableItemCompass)
        {
            RandoMapCoreMod.GS.ToggleItemCompassMode();
            ItemCompass.Info.UpdateCompassTargets();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Item Compass\nMode".L()}: ";

        if (!RandoMapCoreMod.Data.EnableItemCompass)
        {
            return (text + "Disabled".L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return (
            text
            + (
                RandoMapCoreMod.GS.ItemCompassMode switch
                {
                    ItemCompassMode.Reachable => "Reachable",
                    ItemCompassMode.ReachableOutOfLogic => "OOL",
                    ItemCompassMode.All => "All",
                    _ => "",
                }
            ).L()
        ).ToNeutralTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        if (!RandoMapCoreMod.Data.EnableItemCompass)
        {
            return "Toggle disabled".L().ToNeutralTextFormat();
        }

        return (
            "Item compass will point to: ".L()
            + RandoMapCoreMod.GS.ItemCompassMode switch
            {
                ItemCompassMode.Reachable => "Reachable items".L(),
                ItemCompassMode.ReachableOutOfLogic => "Reachable items including sequence break".L(),
                ItemCompassMode.All => "All items".L(),
                _ => "",
            }
        ).ToNeutralTextFormat();
    }
}
