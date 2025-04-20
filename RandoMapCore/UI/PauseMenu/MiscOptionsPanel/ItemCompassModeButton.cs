using MagicUI.Elements;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class ItemCompassModeButton() : BorderlessExtraButton(nameof(ItemCompassModeButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleItemCompassMode();
        ItemCompass.Info.UpdateCompassTargets();
        OnHover();
    }

    protected override void OnHover()
    {
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
        Button.Content =
            $"{"Item compass\nmode".L()}: "
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
