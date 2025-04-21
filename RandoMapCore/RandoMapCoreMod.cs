using System.Reflection;
using MapChanger;
using Modding;
using RandoMapCore.Data;
using RandoMapCore.Input;
using RandoMapCore.Modes;
using RandoMapCore.Pathfinder;
using RandoMapCore.Pins;
using RandoMapCore.Rooms;
using RandoMapCore.Settings;
using RandoMapCore.Transition;
using RandoMapCore.UI;
using UnityEngine;

namespace RandoMapCore;

public class RandoMapCoreMod : Mod, ILocalSettings<LocalSettings>, IGlobalSettings<GlobalSettings>
{
    private static readonly IEnumerable<MapMode> _modes =
    [
        new FullMapMode(),
        new AllPinsMode(),
        new PinsOverAreaMode(),
        new PinsOverRoomMode(),
        new TransitionNormalMode(),
        new TransitionVisitedOnlyMode(),
        new TransitionAllRoomsMode(),
    ];

    private static readonly IEnumerable<HookModule> _hookModules =
    [
        new RmcColors(),
        new RmcRoomManager(),
        new TransitionData(),
        new RmcPathfinder(),
        new RmcPinManager(),
        new ItemCompass(),
        new RouteCompass(),
    ];

    private static readonly List<RmcDataModule> _dataModules = [];

    public RandoMapCoreMod()
    {
        Instance = this;
    }

    public static LocalSettings LS { get; private set; } = new();
    public static GlobalSettings GS { get; private set; } = new();

    internal static Assembly Assembly => Assembly.GetExecutingAssembly();
    internal static RandoMapCoreMod Instance { get; private set; }

    internal static RmcDataModule Data { get; private set; }

    public override string GetVersion()
    {
        return "1.0.1";
    }

    public override int LoadPriority()
    {
        return 10;
    }

    public void OnLoadLocal(LocalSettings ls)
    {
        LS = ls;
    }

    public LocalSettings OnSaveLocal()
    {
        return LS;
    }

    public void OnLoadGlobal(GlobalSettings gs)
    {
        GS = gs;
    }

    public GlobalSettings OnSaveGlobal()
    {
        return GS;
    }

    public override void Initialize()
    {
        if (!Dependencies.HasAll())
        {
            return;
        }

        LogDebug($"Initializing");

        Interop.FindInteropMods();

        InputManager.AddRange(
            [
                new LocationHintInput(),
                new TogglePinClusterInput(),
                new LockGridPinInput(),
                new SelectRoomRouteInput(),
                new BenchwarpInput(),
                new ProgressHintInput(),
                new ControlPanelInput(),
                new MapKeyInput(),
                new PinPanelInput(),
                new ToggleItemCompassInput(),
                new ToggleBenchwarpPinsInput(),
                new RoomPanelInput(),
                new SelectionReticleInput(),
                new PathfinderBenchwarpInput(),
                new ProgressHintPanelInput(),
                new ToggleSpoilersInput(),
                new ToggleVanillaInput(),
                new ToggleRandomizedInput(),
                new ToggleShapeInput(),
                new ToggleSizeInput(),
                new DebugInput(),
            ]
        );

        Events.OnEnterGame += OnEnterGame;
        Events.OnQuitToMenu += OnQuitToMenu;

        LogDebug($"Initialization complete.");
    }

    public static void AddDataModule(RmcDataModule dataModule)
    {
        _dataModules.Add(dataModule);
    }

    internal static void ResetToDefaultSettings()
    {
        GS = new();
    }

    private static void OnEnterGame()
    {
        if (_dataModules.FirstOrDefault(d => d.IsCorrectSaveType) is RmcDataModule data)
        {
            Data = data;
            Data.OnEnterGame();
        }
        else
        {
            return;
        }

        MapChanger.Settings.AddModes(_modes);
        Events.OnSetGameMap += OnSetGameMap;

        if (Interop.HasBenchwarp)
        {
            BenchwarpInterop.Load();
        }

        foreach (var hookModule in _hookModules)
        {
            hookModule.OnEnterGame();
        }
    }

    private static void OnQuitToMenu()
    {
        if (Data is null)
        {
            return;
        }

        Events.OnSetGameMap -= OnSetGameMap;

        if (Interop.HasBenchwarp)
        {
            BenchwarpInterop.Unload();
        }

        foreach (var hookModule in _hookModules)
        {
            hookModule.OnQuitToMenu();
        }

        Data.OnQuitToMenu();
        Data = null;
    }

    private static void OnSetGameMap(GameObject goMap)
    {
        try
        {
            // Make rooms and pins
            RmcRoomManager.Make(goMap);
            RmcPinManager.Make(goMap);

            LS.Initialize();

            RmcUIBuilder uiBuilder = new();
            uiBuilder.Build();
        }
        catch (Exception e)
        {
            Instance.LogError(e);
        }
    }
}
