using MagicUI.Elements;
using MapChanger;
using MapChanger.Defs;
using MapChanger.Map;
using MapChanger.MonoBehaviours;
using RandoMapCore.Input;
using RandoMapCore.Rooms;
using System.Collections.ObjectModel;

namespace RandoMapCore.Pins;

internal class GridPin : RmcPin
{
    private readonly Dictionary<string, QuickMapPosition> _quickMapPositions = [];

    private IMapPosition _worldMapGridPosition;

    internal string ModSource { get; private set; } = RandoMapCoreMod.Data.ModName;
    internal int GridIndex { get; private set; } = int.MaxValue;
    internal IReadOnlyCollection<string> HighlightScenes { get; private set; }
    internal ReadOnlyCollection<ColoredMapObject> HighlightRooms { get; private set; }

    internal new void Initialize(PinDef def)
    {
        base.Initialize(def);

        if (def is not ICPinDef icpd)
        {
            return;
        }

        ModSource = icpd.ModSource;
        GridIndex = icpd.GridIndex;
        HighlightScenes = icpd.HighlightScenes;

        if (RandoMapCoreMod.Data.EnablePinSelection && HighlightScenes is not null)
        {
            List<ColoredMapObject> highlightRooms = [];
            foreach (var scene in HighlightScenes)
            {
                if (BuiltInObjects.SelectableRooms.TryGetValue(scene, out var room))
                {
                    highlightRooms.AddRange(room.Selectables);
                }
                else if (RmcRoomManager.RoomTexts.TryGetValue(scene, out var roomText))
                {
                    highlightRooms.Add(roomText);
                }
            }

            HighlightRooms = new(highlightRooms);
        }
    }

    internal void AddWorldMapPosition(IMapPosition position)
    {
        _worldMapGridPosition = position;
    }

    internal void AddQuickMapPosition(string sceneName, QuickMapPosition position)
    {
        _quickMapPositions.Add(sceneName, position);
    }

    internal void UpdatePositionWorldMap()
    {
        MapPosition = _worldMapGridPosition;
    }

    internal void UpdatePositionQuickMap(string sceneName)
    {
        if (_quickMapPositions.TryGetValue(sceneName, out var position))
        {
            MapPosition = position;
        }
        else
        {
            MapPosition = QuickMapPosition.HiddenPosition;
        }
    }

    private protected override bool CorrectMapOpen()
    {
        return States.WorldMapOpen || (States.QuickMapOpen && _quickMapPositions.ContainsKey(Utils.CurrentScene()));
    }

    private protected override bool ActiveByCurrentMode()
    {
        return true;
    }

    public RunCollection GetText(bool lockSelection)
    {
        if (HighlightScenes is null)
        {
            return GetText();
        }

        return [
            .. GetText(),
            new Run("\n\n"),
            new Run($"{"Press".L()} "),
            new Run(LockGridPinInput.Instance.GetBindingsText()),
            new Run($" {(lockSelection ? "to unlock pin selection" : "to lock pin selection and view highlighted rooms").L()}."),
        ];
    }
}
