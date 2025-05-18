using MapChanger;
using MapChanger.UI;

namespace RandoMapCore.UI;

internal class ResetGlobalSettingsButton : ExtraButton
{
    protected override void OnClick()
    {
        MapChangerMod.ResetGlobalSettings();
        RandoMapCoreMod.ResetGlobalSettings();
    }

    protected override TextFormat GetTextFormat()
    {
        return ("Reset Global\nSettings".L(), RmcColorSetting.UI_Special).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return $"Resets all global settings of {RandoMapCoreMod.Data.ModName}.".L().ToNeutralTextFormat();
    }
}
