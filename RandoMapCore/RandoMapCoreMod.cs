using System.Reflection;
using MapChanger;
using MapChanger.Defs;
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

    private static readonly List<HookModule> _hookModules = [];

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

    public bool ToggleButtonInsideMenu => false;

    public override string GetVersion()
    {
        return "1.0.11";
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
                new PathfinderStagsInput(),
                new PathfinderDreamgateInput(),
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

        Finder.InjectLocations(
            JsonUtil.DeserializeFromAssembly<Dictionary<string, MapLocationDef>>(
                Assembly,
                "RandoMapCore.Resources.locations.json"
            )
        );

        LogDebug($"Initialization complete.");
    }

    public static void AddDataModule(RmcDataModule dataModule)
    {
        _dataModules.Add(dataModule);
    }

    internal static IEnumerable<string> GetRegisteredModNames()
    {
        return _dataModules.Select(d => d.ModName);
    }

    internal static void ResetGlobalSettings()
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

        if (Data.ForceMapMode is string mapMode && _modes.FirstOrDefault(m => m.ModeName == mapMode) is RmcMapMode mode)
        {
            ModeManager.AddModes([mode]);
        }
        else
        {
            ModeManager.AddModes(_modes);
        }

        Events.OnSetGameMap += OnSetGameMap;

        if (Interop.HasBenchwarp)
        {
            _hookModules.Add(new BenchwarpInterop());
        }

        if (Data.EnableVisualCustomization)
        {
            _hookModules.Add(new RmcColors());
        }

        _hookModules.AddRange([new RmcRoomManager(), new TransitionData(), new RmcPathfinder(), new RmcPinManager()]);

        if (Data.EnableItemCompass)
        {
            _hookModules.Add(new ItemCompass());
        }

        if (Data.EnableRoomSelection && Data.EnablePathfinder)
        {
            _hookModules.Add(new RouteCompass());
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

        RmcUIManager.Destroy();

        foreach (var hookModule in _hookModules)
        {
            hookModule.OnQuitToMenu();
        }

        _hookModules.Clear();

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

            RmcUIManager.Build();
        }
        catch (Exception e)
        {
            Instance.LogError(e);
        }
    }
}
