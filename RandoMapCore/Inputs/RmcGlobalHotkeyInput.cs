using MapChanger.Input;
using MapChanger.UI;
using RandoMapCore.Modes;
using RandoMapCore.Pins;
using RandoMapCore.Rooms;
using UnityEngine;

namespace RandoMapCore.Input;

internal abstract class RmcGlobalHotkeyInput(string name, string category, KeyCode key)
    : GlobalHotkeyInput(name, $"MapMod {category}", key)
{
    public override bool UseCondition()
    {
        return RandoMapCoreMod.Data is not null && RandoMapCoreMod.Data.IsCorrectSaveType;
    }

    public override bool ActiveCondition()
    {
        return Conditions.RandoCoreMapModEnabled();
    }

    public override void DoAction()
    {
        PauseMenu.Update();
        RmcPinManager.MainUpdate();
        PinSelector.Instance.MainUpdate();
        TransitionRoomSelector.Instance.MainUpdate();
        MapUILayerUpdater.Update();
    }
}
