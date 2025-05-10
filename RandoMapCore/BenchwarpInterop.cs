using System.Collections;
using System.Collections.ObjectModel;
using Benchwarp;
using InControl;
using MapChanger;
using Modding;
using UnityEngine;

namespace RandoMapCore;

public record struct RmcBenchKey(string SceneName, string RespawnMarkerName);

internal class BenchwarpInterop : HookModule
{
    internal const string BENCH_WARP_START = "Start_Warp";

    internal static ReadOnlyDictionary<RmcBenchKey, string> BenchNames { get; private set; }
    internal static ReadOnlyDictionary<string, RmcBenchKey> BenchKeys { get; private set; }
    internal static RmcBenchKey StartKey { get; private set; }

    public override void OnEnterGame()
    {
        Dictionary<RmcBenchKey, string> benchNames = [];

        if (RandoMapCoreMod.Data.GetCustomBenches() is IReadOnlyDictionary<RmcBenchKey, string> customBenches)
        {
            benchNames = new(customBenches.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }
        else
        {
            var defaultBenches = MapChanger.JsonUtil.DeserializeFromAssembly<Dictionary<string, string>>(
                RandoMapCoreMod.Assembly,
                "RandoMapCore.Resources.defaultBenches.json"
            );

            foreach (var kvp in defaultBenches)
            {
                if (Bench.Benches.FirstOrDefault(b => b.sceneName == kvp.Value) is Bench bench)
                {
                    benchNames.Add(new(bench.sceneName, bench.respawnMarker), kvp.Key);
                }
            }
        }

        StartKey = new(ItemChanger.Internal.Ref.Settings.Start.SceneName, "ITEMCHANGER_RESPAWN_MARKER");
        benchNames.Add(StartKey, BENCH_WARP_START);

        BenchNames = new(benchNames);
        BenchKeys = new(benchNames.ToDictionary(t => t.Value, t => t.Key));
    }

    public override void OnQuitToMenu()
    {
        BenchNames = null;
        BenchKeys = null;
        StartKey = default;
    }

    internal static IEnumerator DoBenchwarp(string benchName)
    {
        if (benchName is null)
        {
            yield return null;
        }

        if (BenchKeys.TryGetValue(benchName, out var benchKey))
        {
            yield return DoBenchwarp(benchKey);
        }
    }

    internal static IEnumerator DoBenchwarp(RmcBenchKey benchKey)
    {
        InputHandler.Instance.inputActions.openInventory.CommitWithState(
            true,
            ReflectionHelper.GetField<OneAxisInputControl, ulong>(
                InputHandler.Instance.inputActions.openInventory,
                "pendingTick"
            ) + 1,
            0
        );
        yield return new WaitWhile(() => GameManager.instance.inventoryFSM.ActiveStateName != "Closed");
        yield return new WaitForSeconds(0.15f);
        UIManager.instance.TogglePauseGame();
        yield return new WaitWhile(() => !GameManager.instance.IsGamePaused());
        yield return new WaitForSecondsRealtime(0.1f);

        if (GameManager.instance.IsGamePaused())
        {
            var bench = Bench.Benches.FirstOrDefault(b =>
                b.sceneName == benchKey.SceneName && b.respawnMarker == benchKey.RespawnMarkerName
            );

            if (bench is not null)
            {
                bench.SetBench();
            }
            else
            {
                Benchwarp.Events.SetToStart();
            }

            ChangeScene.WarpToRespawn();
        }
    }

    internal static bool IsVisitedBench(string benchName)
    {
        return benchName is not null
            && BenchKeys.TryGetValue(benchName, out var key)
            && GetVisitedBenchKeys().Contains(key);
    }

    /// <summary>
    /// Gets the BenchKeys from Benchwarp's visited benches and converts them to RmcBenchKeys.
    /// </summary>
    internal static IEnumerable<RmcBenchKey> GetVisitedBenchKeys()
    {
        return
        [
            .. Benchwarp.Benchwarp.LS.visitedBenchScenes.Select(bwKey => new RmcBenchKey(
                bwKey.SceneName,
                bwKey.RespawnMarkerName
            )),
            StartKey,
        ];
    }

    internal static IEnumerable<string> GetVisitedBenchNames()
    {
        return GetVisitedBenchKeys().Where(BenchNames.ContainsKey).Select(b => BenchNames[b]);
    }
}
