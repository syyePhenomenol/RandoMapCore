using MapChanger.UI;
using UnityEngine;

namespace RandoMapCore.UI;

public abstract class RmcGridControlButton<T> : GridControlButton<T>
    where T : ExtraButtonGrid
{
    protected override Color GetBorderColor()
    {
        return RmcColors.GetColor(RmcColorSetting.UI_Borders);
    }
}
