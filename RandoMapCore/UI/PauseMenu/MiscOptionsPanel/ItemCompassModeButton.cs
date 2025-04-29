using MagicUI.Elements;
using RandoMapCore.Localization;
using RandoMapCore.Settings;
using UnityEngine.UI;

namespace RandoMapCore.UI;

internal class ItemCompassModeButton() : BorderlessExtraButton(nameof(ItemCompassModeButton))
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableItemCompass)
        {
            RandoMapCoreMod.GS.ToggleItemCompassMode();
            ItemCompass.Info.UpdateCompassTargets();
            OnHover();
        }
    }

    protected override void OnHover()
    {
        if (!RandoMapCoreMod.Data.EnableItemCompass)
        {
            RmcTitle.Instance.HoveredText = "Toggle disabled".L();
            return;
        }

        RmcTitle.Instance.HoveredText =
            "Item compass will point to: ".L()
            + RandoMapCoreMod.GS.ItemCompassMode switch
            {
                ItemCompassMode.Reachable => "Reachable items".L(),
                ItemCompassMode.ReachableOutOfLogic => "Reachable items including sequence break".L(),
                ItemCompassMode.All => "All items".L(),
                _ => "",
            };
    }

    public override void Update()
    {
        var text = $"{"Item compass\nmode".L()}: ";

        if (!RandoMapCoreMod.Data.EnableItemCompass)
        {
            Button.Content = text + "Disabled".L();
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
            return;
        }

        Button.Content =
            text
            + RandoMapCoreMod.GS.ItemCompassMode switch
            {
                ItemCompassMode.Reachable => "Reachable".L(),
                ItemCompassMode.ReachableOutOfLogic => "OoL".L(),
                ItemCompassMode.All => "All".L(),
                _ => "",
            };

        Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }
}
