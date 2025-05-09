﻿using MagicUI.Core;
using MapChanger.UI;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class SelectionPanels : WorldMapStack
{
    private PinSelectionPanel _pinSelectionPanel;
    private RoomSelectionPanel _roomSelectionPanel;

    internal static SelectionPanels Instance { get; private set; }

    protected override HorizontalAlignment StackHorizontalAlignment => HorizontalAlignment.Right;

    protected override bool Condition()
    {
        return base.Condition() && Conditions.RandoCoreMapModEnabled();
    }

    protected override void BuildStack()
    {
        Instance = this;
        _pinSelectionPanel = RandoMapCoreMod.Data.EnablePinSelection ? new(Root, Stack) : null;
        _roomSelectionPanel = RandoMapCoreMod.Data.EnableRoomSelection ? new(Root, Stack) : null;
    }

    public override void Update()
    {
        _pinSelectionPanel?.Update();
        _roomSelectionPanel?.Update();
    }
}
