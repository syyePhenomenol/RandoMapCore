using GlobalEnums;
using MapChanger;
using MapChanger.Defs;
using RandoMapCore.Localization;
using RandoMapCore.Modes;
using RandoMapCore.Pathfinder;
using RandoMapCore.Settings;
using UnityEngine;

namespace RandoMapCore.Pins;

internal abstract class PinDef
{
    internal PinDef()
    {
        TextBuilders.AddRange([GetNameText, GetRoomText, GetStatusText]);
    }

    internal string Name { get; init; }
    internal IReadOnlyCollection<string> LocationPoolGroups { get; init; }
    internal IReadOnlyCollection<string> ItemPoolGroups { get; init; }

    internal string SceneName { get; init; }
    internal IMapPosition MapPosition { get; init; }
    internal MapZone MapZone { get; init; } = MapZone.NONE;

    private protected List<Func<string>> TextBuilders { get; } = [];

    internal virtual void Update() { }

    internal virtual bool CorrectMapOpen()
    {
        return States.WorldMapOpen || (States.QuickMapOpen && States.CurrentMapZone == MapZone);
    }

    internal virtual bool ActiveByCurrentMode()
    {
        return SceneName is null
            || MapZone is MapZone.NONE
            || ModeManager.CurrentMode() is FullMapMode or AllPinsMode
            || (ModeManager.CurrentMode() is PinsOverAreaMode && Utils.HasMapSetting(MapZone))
            || (
                ModeManager.CurrentMode() is PinsOverRoomMode
                && Utils.HasMapSetting(MapZone)
                && (
                    (
                        Tracker.HasVisitedScene(Finder.GetMappedScene(SceneName))
                        && (
                            PlayerData.instance.GetBool(nameof(PlayerData.hasQuill))
                            || RandoMapCoreMod.GS.AlwaysHaveQuill
                        )
                    ) || Finder.IsMinimalMapScene(Finder.GetMappedScene(SceneName))
                )
            )
            || (Conditions.TransitionRandoModeEnabled() && RmcPathfinder.Slt.GetRoomActive(SceneName));
    }

    internal abstract bool ActiveBySettings();
    internal abstract bool ActiveByProgress();

    internal abstract IEnumerable<ScaledPinSprite> GetPinSprites();
    internal abstract bool ShrinkPin();
    internal abstract bool DarkenPin();

    internal virtual PinShape GetMixedPinShape()
    {
        return PinShape.Circle;
    }

    internal virtual Color GetBorderColor()
    {
        return RmcColors.GetColor(RmcColorSetting.Pin_Normal);
    }

    public string GetText()
    {
        return string.Join("\n\n", TextBuilders.Select(tb => tb()).Where(t => t is not null));
    }

    private protected virtual string GetNameText()
    {
        return Name.LC();
    }

    private protected virtual string GetRoomText()
    {
        return $"{"Room".L()}: {(SceneName is not null ? SceneName.LC() : "Unknown")}";
    }

    private protected virtual string GetStatusText()
    {
        return $"{"Status".L()}: {"Unknown".L()}";
    }

    internal virtual float GetZPriority()
    {
        return float.MaxValue;
    }
}
