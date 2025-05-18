using MapChanger;
using MapChanger.UI;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class QMarkSettingButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RandoMapCoreMod.GS.ToggleQMarkSetting();
            ItemCompass.Info.UpdateCurrentCompassTargets();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Question\nMarks".L()}: ";

        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            return (text + QMarkSetting.Off.ToString().ToWhitespaced().L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return (
            text + RandoMapCoreMod.GS.QMarks.ToString().ToWhitespaced().L(),
            RandoMapCoreMod.GS.QMarks switch
            {
                QMarkSetting.Off => RmcColorSetting.UI_Neutral,
                QMarkSetting.Red => RmcColorSetting.UI_On,
                QMarkSetting.Mixed => RmcColorSetting.UI_On,
                _ => RmcColorSetting.UI_Neutral,
            }
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableVisualCustomization
                ? "Toggle question mark sprites on/off.".L()
                : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
