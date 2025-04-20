using RandoMapCore.Input;
using RandoMapCore.Localization;
using UnityEngine;

namespace RandoMapCore.UI;

internal class ShowReticleText : ControlPanelText
{
    private protected override string Name => "Show Reticle";

    private protected override bool ActiveCondition()
    {
        return RandoMapCoreMod.GS.ControlPanelOn;
    }

    private protected override Vector4 GetColor()
    {
        return RandoMapCoreMod.GS.ShowReticle
            ? RmcColors.GetColor(RmcColorSetting.UI_On)
            : RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }

    private protected override string GetText()
    {
        return $"{"Show reticles".L()} {SelectionReticleInput.Instance.GetBindingsText()}: {(RandoMapCoreMod.GS.ShowReticle ? "On" : "Off").L()}";
    }
}
