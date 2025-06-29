﻿using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using MapChanger;
using RandoMapCore.Data;

namespace RandoMapCore.Transition;

internal class TransitionData : HookModule
{
    internal static ReadOnlyDictionary<string, RmcTransitionDef> ExtraTransitions { get; private set; }
    internal static ReadOnlyDictionary<RmcTransitionDef, RmcTransitionDef> ExtraPlacements { get; private set; }

    public override void OnEnterGame()
    {
        Dictionary<string, RmcTransitionDef> extraTransitions = [];
        Dictionary<RmcTransitionDef, RmcTransitionDef> extraPlacements = [];

        foreach (
            var td in JsonUtil.DeserializeFromAssembly<RmcTransitionDef[]>(
                RandoMapCoreMod.Assembly,
                "RandoMapCore.Resources.extraTransitions.json"
            )
        )
        {
            if (IsRandomizedTransition(td.Name) || IsVanillaTransition(td.Name))
            {
                continue;
            }

            extraTransitions[td.Name] = td;
        }

        foreach (var source in extraTransitions.Values)
        {
            if (source.VanillaTarget is not null && extraTransitions.TryGetValue(source.VanillaTarget, out var target))
            {
                extraPlacements[source] = target;
            }
        }

        ExtraTransitions = new(extraTransitions);
        ExtraPlacements = new(extraPlacements);
    }

    public override void OnQuitToMenu()
    {
        ExtraTransitions = null;
        ExtraPlacements = null;
    }

    internal static bool IsRandomizedTransition(string transition)
    {
        return RandoMapCoreMod.Data.RandomizedTransitions.ContainsKey(transition);
    }

    internal static bool IsVanillaTransition(string transition)
    {
        return RandoMapCoreMod.Data.VanillaTransitions.ContainsKey(transition);
    }

    internal static bool IsExtraTransition(string transition)
    {
        return ExtraTransitions.ContainsKey(transition);
    }

    internal static RmcTransitionDef GetTransitionDef(string transition)
    {
        if (
            RandoMapCoreMod.Data.RandomizedTransitions.TryGetValue(transition, out var def)
            || RandoMapCoreMod.Data.VanillaTransitions.TryGetValue(transition, out def)
            || ExtraTransitions.TryGetValue(transition, out def)
        )
        {
            return def;
        }

        return null;
    }

    internal static IEnumerable<(RmcTransitionDef, RmcTransitionDef)> GetPlacements()
    {
        return RandoMapCoreMod
            .Data.RandomizedTransitionPlacements.Select(kvp => (kvp.Key, kvp.Value))
            .Concat(RandoMapCoreMod.Data.VanillaTransitionPlacements.Select(kvp => (kvp.Key, kvp.Value)))
            .Concat(ExtraPlacements.Select(kvp => (kvp.Key, kvp.Value)));
    }

    internal static bool TryGetPlacementTarget(string source, out RmcTransitionDef target)
    {
        if (
            GetTransitionDef(source) is RmcTransitionDef sourceTd
            && (
                RandoMapCoreMod.Data.RandomizedTransitionPlacements.TryGetValue(sourceTd, out target)
                || RandoMapCoreMod.Data.VanillaTransitionPlacements.TryGetValue(sourceTd, out target)
                || ExtraPlacements.TryGetValue(sourceTd, out target)
            )
        )
        {
            return true;
        }

        target = null;
        return false;
    }

    // Works for in/out transitions, localized waypoints (scene names, but NOT Can_Stag, Lower_Tram etc.), bench names
    internal static bool TryGetScene(string name, out string scene)
    {
        if (IsScene(name))
        {
            scene = name;
            return true;
        }

        if (GetTransitionDef(name) is RmcTransitionDef td)
        {
            scene = td.SceneName;
            return true;
        }

        // The following matches other non-RmcTransitionDef transition names in SearchData
        var sourceMatch = Regex.Match(name, @"^(\w+)\[(\w+)\]$");

        if (sourceMatch.Groups.Count == 3 && IsScene(sourceMatch.Groups[1].Value))
        {
            scene = sourceMatch.Groups[1].Value;
            return true;
        }

        if (Interop.HasBenchwarp)
        {
            if (BenchwarpInterop.BenchKeys.TryGetValue(name, out var benchKey))
            {
                scene = benchKey.SceneName;
                return true;
            }

            if (name == RandoMapCoreMod.Data.StartTerm.ToString())
            {
                scene = BenchwarpInterop.StartKey.SceneName;
                return true;
            }
        }

        scene = default;
        return false;
    }

    internal static bool IsScene(string scene)
    {
        return Finder.IsScene(scene);
    }
}
