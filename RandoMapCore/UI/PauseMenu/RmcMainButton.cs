using MapChanger.UI;
using UnityEngine;

namespace RandoMapCore.UI;

public abstract class RmcMainButton : MainButton
{
    protected override Color GetBorderColor()
    {
        return RmcColors.GetColor(RmcColorSetting.UI_Borders);
    }
}
