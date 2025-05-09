﻿using MapChanger.UI;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class TopLeftPanels : WorldMapStack
{
    private MapKeyPanel _mapKeyPanel;
    private ProgressHintPanel _progressHintPanel;

    internal static TopLeftPanels Instance { get; private set; }

    protected override void BuildStack()
    {
        Instance = this;
        _mapKeyPanel = new(Root, Stack);
        _progressHintPanel = RandoMapCoreMod.Data.EnableProgressionHints ? new(Root, Stack) : null;
    }

    protected override bool Condition()
    {
        return base.Condition() && Conditions.RandoCoreMapModEnabled();
    }

    public override void Update()
    {
        _mapKeyPanel.Update();
        _progressHintPanel?.Update();
    }
}
