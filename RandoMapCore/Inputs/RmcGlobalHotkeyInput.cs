using MapChanger.Input;
using RandoMapCore.Modes;
using RandoMapCore.Pins;
using RandoMapCore.Rooms;
using RandoMapCore.UI;
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
        RmcPinManager.Update();
        RmcRoomManager.Update();
        RmcUIManager.UpdateAll();
    }
}
