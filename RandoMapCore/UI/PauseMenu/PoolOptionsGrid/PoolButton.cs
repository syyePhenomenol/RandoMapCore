using MapChanger;
using MapChanger.UI;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class PoolButton(string poolGroup) : ExtraButton
{
    public override string Name => $"{Layout.Mod} {PoolGroup}";

    internal string PoolGroup { get; init; } = poolGroup;

    protected override void OnClick()
    {
        RandoMapCoreMod.LS.TogglePoolGroupSetting(PoolGroup);
    }

    protected override TextFormat GetTextFormat()
    {
        return (
            PoolGroup.L().Replace(" ", "\n"),
            RandoMapCoreMod.LS.GetPoolGroupSetting(PoolGroup) switch
            {
                PoolState.On => RmcColorSetting.UI_On,
                PoolState.Off => RmcColorSetting.UI_Neutral,
                PoolState.Mixed => RmcColorSetting.UI_Custom,
                _ => RmcColorSetting.UI_On,
            }
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return $"{"Toggle".L()} {PoolGroup.L()} {"on/off".L()}.".ToNeutralTextFormat();
    }
}
