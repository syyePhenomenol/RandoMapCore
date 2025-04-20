using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class PoolButton(string poolGroup) : ExtraButton(poolGroup, RandoMapCoreMod.Data.ModName)
{
    internal string PoolGroup { get; init; } = poolGroup;

    protected override void OnClick()
    {
        RandoMapCoreMod.LS.TogglePoolGroupSetting(PoolGroup);
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = $"{"Toggle".L()} {PoolGroup.L()} {"on/off".L()}.";
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        Button.Content = PoolGroup.L().Replace(" ", "\n");

        Button.ContentColor = RandoMapCoreMod.LS.GetPoolGroupSetting(PoolGroup) switch
        {
            PoolState.On => RmcColors.GetColor(RmcColorSetting.UI_On),
            PoolState.Off => RmcColors.GetColor(RmcColorSetting.UI_Neutral),
            PoolState.Mixed => RmcColors.GetColor(RmcColorSetting.UI_Custom),
            _ => RmcColors.GetColor(RmcColorSetting.UI_On),
        };
    }
}
