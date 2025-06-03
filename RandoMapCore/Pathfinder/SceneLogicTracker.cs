using System.Collections.ObjectModel;
using MapChanger;
using RandoMapCore.Data;
using RandoMapCore.Modes;
using RandoMapCore.Transition;
using RandomizerCore.Logic;
using UnityEngine;

namespace RandoMapCore.Pathfinder;

internal class SceneLogicTracker(
    RmcSearchData sd,
    ProgressionManager localPM,
    ProgressionManager localPMNoSequenceBreak
)
{
    private readonly HashSet<string> _inLogicScenes = [];
    private readonly HashSet<string> _sequenceBreakScenes = [];
    private readonly HashSet<string> _adjacentScenes = [];
    private readonly HashSet<string> _uncheckedReachableScenes = [];

    internal ReadOnlyCollection<string> InLogicScenes => new([.. _inLogicScenes, .. _sequenceBreakScenes]);

    internal void Update()
    {
        _inLogicScenes.Clear();
        _sequenceBreakScenes.Clear();
        _adjacentScenes.Clear();
        _uncheckedReachableScenes.Clear();

        // Get in-logic scenes from in-logic terms in SearchData
        foreach (var position in sd.GetAllStateTerms())
        {
            if (localPMNoSequenceBreak.Has(position))
            {
                if (TransitionData.TryGetScene(position.Name, out var scene))
                {
                    _inLogicScenes.Add(scene);
                }
            }
            else if (localPM.Has(position))
            {
                if (TransitionData.TryGetScene(position.Name, out var scene))
                {
                    _sequenceBreakScenes.Add(scene);
                }
            }
        }

        // Get more sequence break scenes from OOL transition actions
        foreach (var source in RandoMapCoreMod.LS.SequenceBreakActions.Values.SelectMany(t => t))
        {
            if (sd.TryGetSequenceBreakAction(source, out var action))
            {
                if (TransitionData.TryGetScene(source, out var scene) && !_inLogicScenes.Contains(scene))
                {
                    _sequenceBreakScenes.Add(scene);
                }

                if (
                    TransitionData.TryGetScene(action.Target.Name, out var targetScene)
                    && !_inLogicScenes.Contains(targetScene)
                )
                {
                    _sequenceBreakScenes.Add(targetScene);
                }
            }
        }

        // Get in-logic adjacent scenes from transitions in SearchData connected by 1-cost actions
        // to current scene
        _adjacentScenes.UnionWith(PositionHelper.GetVisitedAdjacentScenes());

        // Get scenes where there are unchecked reachable transitions
        foreach (var transition in RandoMapCoreMod.Data.UncheckedReachableTransitions)
        {
            if (TransitionData.GetTransitionDef(transition) is RmcTransitionDef td)
            {
                _uncheckedReachableScenes.Add(td.SceneName);
            }
        }
    }

    internal bool GetRoomActive(string scene)
    {
        if (ModeManager.CurrentMode() is TransitionNormalMode)
        {
            return Tracker.HasVisitedScene(scene)
                || _inLogicScenes.Contains(scene)
                || _sequenceBreakScenes.Contains(scene);
        }

        if (ModeManager.CurrentMode() is TransitionVisitedOnlyMode)
        {
            return Tracker.HasVisitedScene(scene);
        }

        return true;
    }

    internal Vector4 GetRoomColor(string scene)
    {
        var color = RmcColors.GetColor(RmcColorSetting.Room_Out_of_logic);

        if (scene == Utils.CurrentScene())
        {
            color = RmcColors.GetColor(RmcColorSetting.Room_Current);
        }
        else if (_adjacentScenes.Contains(scene))
        {
            color = RmcColors.GetColor(RmcColorSetting.Room_Adjacent);
        }
        else if (_inLogicScenes.Contains(scene))
        {
            color = RmcColors.GetColor(RmcColorSetting.Room_Normal);
        }
        else if (_sequenceBreakScenes.Contains(scene))
        {
            color = RmcColors.GetColor(RmcColorSetting.Room_Sequence_break);
        }

        if (_uncheckedReachableScenes.Contains(scene))
        {
            color.w = 1f;
        }

        return color;
    }
}
