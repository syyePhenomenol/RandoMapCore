﻿using MapChanger;
using MapChanger.MonoBehaviours;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RandoMapCore.UI;

internal class ItemCompass : HookModule
{
    private static GameObject _goCompass;

    private bool _finishedLateUpdate;

    internal static ItemCompassInfo Info { get; private set; }

    public override void OnEnterGame()
    {
        Info = new();
        Make();

        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += EarlyUpdate;
        On.PlayMakerFSM.Start += LateUpdate;
        Data.PlacementTracker.Update += Info.UpdateCurrentCompassTargets;
    }

    public override void OnQuitToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= EarlyUpdate;
        On.PlayMakerFSM.Start -= LateUpdate;
        Data.PlacementTracker.Update -= Info.UpdateCurrentCompassTargets;

        Destroy();
        Info = null;
    }

    private void EarlyUpdate(Scene from, Scene to)
    {
        Info.UpdateCompassTargets();

        _finishedLateUpdate = false;
    }

    private void LateUpdate(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self)
    {
        orig(self);

        if (_finishedLateUpdate)
        {
            return;
        }

        RandoMapCoreMod.Instance.LogFine("Update item compass");
        Info.UpdateCurrentCompassTargets();
        _finishedLateUpdate = true;
    }

    private void Make()
    {
        _goCompass = DirectionalCompass.Make(Info);
    }

    private void Destroy()
    {
        UnityEngine.Object.Destroy(_goCompass);
    }
}
