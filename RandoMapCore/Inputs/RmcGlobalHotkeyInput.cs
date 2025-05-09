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
        return RandoMapCoreMod.Data is not null;
    }

    public override bool ActiveCondition()
    {
        return Conditions.RandoCoreMapModEnabled();
    }

    public override void DoAction()
    {
        PauseMenu.Update();
        RmcPinManager.MainUpdate();

        if (RandoMapCoreMod.Data.EnablePinSelection)
        {
            PinSelector.Instance.MainUpdate();
        }

        if (RandoMapCoreMod.Data.EnableRoomSelection)
        {
            TransitionRoomSelector.Instance.MainUpdate();
        }

        MapUILayerUpdater.Update();
    }
}
