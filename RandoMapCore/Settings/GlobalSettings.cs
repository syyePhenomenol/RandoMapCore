using Newtonsoft.Json;

namespace RandoMapCore.Settings;

public class GlobalSettings
{
    // Map UI toggles

    [JsonProperty]
    public bool ControlPanelOn { get; private set; } = true;

    [JsonProperty]
    public bool MapKeyOn { get; private set; } = false;

    [JsonProperty]
    public ProgressHintSetting ProgressHint { get; private set; } = ProgressHintSetting.Area;

    [JsonProperty]
    public bool ItemCompassOn { get; private set; } = false;

    [JsonProperty]
    public bool PinSelectionOn { get; private set; } = true;

    [JsonProperty]
    public bool ShowBenchwarpPins { get; private set; } = true;

    [JsonProperty]
    public bool RoomSelectionOn { get; private set; } = true;

    [JsonProperty]
    public bool PathfinderStags { get; private set; } = true;

    [JsonProperty]
    public bool PathfinderDreamgate { get; private set; } = true;

    [JsonProperty]
    public bool PathfinderBenchwarp { get; private set; } = true;

    // Pin options
    [JsonProperty]
    public PinShapeSetting PinShapes { get; private set; } = PinShapeSetting.Mixed;

    [JsonProperty]
    public PinSize PinSize { get; private set; } = PinSize.Medium;

    [JsonProperty]
    public ClearedPinsSetting ShowClearedPins { get; private set; } = ClearedPinsSetting.Persistent;

    [JsonProperty]
    public ReachablePinsSetting ShowReachablePins { get; private set; } = ReachablePinsSetting.ExpandReachable;

    [JsonProperty]
    public QMarkSetting QMarks { get; private set; } = QMarkSetting.Off;

    // Pathfinder options
    [JsonProperty]
    public SequenceBreakSetting PathfinderSequenceBreaks { get; private set; } =
        SequenceBreakSetting.SuperSequenceBreaks;

    [JsonProperty]
    public bool ShowRouteCompass { get; private set; } = true;

    [JsonProperty]
    public RouteTextInGame RouteTextInGame { get; private set; } = RouteTextInGame.NextTransition;

    [JsonProperty]
    public OffRouteBehaviour WhenOffRoute { get; private set; } = OffRouteBehaviour.Reevaluate;

    // Miscellaneous options

    [JsonProperty]
    public ItemCompassMode ItemCompassMode { get; private set; } = ItemCompassMode.Reachable;

    [JsonProperty]
    public QuickMapCompassSetting ShowQuickMapCompasses { get; private set; } = QuickMapCompassSetting.Unchecked;

    [JsonProperty]
    public bool ShowAreaNames { get; private set; } = true;

    [JsonProperty]
    public NextAreaSetting ShowNextAreas { get; private set; } = NextAreaSetting.Full;

    [JsonProperty]
    public bool ShowMapMarkers { get; private set; } = false;

    [JsonProperty]
    public bool AlwaysHaveQuill { get; private set; } = true;

    /// <summary>
    /// By default, the mode is set to Full Map in item rando, and Transition Normal in a transition rando (at
    /// least one randomized transition). Use the below settings to override them.
    /// </summary>
    [JsonProperty]
    public RmcMode DefaultItemRandoMode { get; private set; } = RmcMode.Full_Map;

    [JsonProperty]
    public RmcMode DefaultTransitionRandoMode { get; private set; } = RmcMode.Transition_Normal;

    internal void ToggleControlPanel()
    {
        ControlPanelOn = !ControlPanelOn;
    }

    internal void ToggleMapKey()
    {
        MapKeyOn = !MapKeyOn;
    }

    internal void ToggleProgressHint()
    {
        ProgressHint = (ProgressHintSetting)(
            ((int)ProgressHint + 1) % Enum.GetNames(typeof(ProgressHintSetting)).Length
        );
    }

    internal void ToggleItemCompass()
    {
        ItemCompassOn = !ItemCompassOn;
    }

    internal void TogglePinSelection()
    {
        PinSelectionOn = !PinSelectionOn;
    }

    internal void ToggleBenchwarpPins()
    {
        ShowBenchwarpPins = !ShowBenchwarpPins;
    }

    internal void ToggleRoomSelection()
    {
        RoomSelectionOn = !RoomSelectionOn;
    }

    internal void TogglePathfinderStags()
    {
        PathfinderStags = !PathfinderStags;
    }

    internal void TogglePathfinderDreamgate()
    {
        PathfinderDreamgate = !PathfinderDreamgate;
    }

    internal void TogglePathfinderBenchwarp()
    {
        PathfinderBenchwarp = !PathfinderBenchwarp;
    }

    internal void TogglePinShape()
    {
        PinShapes = (PinShapeSetting)(((int)PinShapes + 1) % Enum.GetNames(typeof(PinShapeSetting)).Length);
    }

    internal void TogglePinSize()
    {
        PinSize = (PinSize)(((int)PinSize + 1) % Enum.GetNames(typeof(PinSize)).Length);
    }

    internal void ToggleShowClearedPins()
    {
        ShowClearedPins = (ClearedPinsSetting)(
            ((int)ShowClearedPins + 1) % Enum.GetNames(typeof(ClearedPinsSetting)).Length
        );
    }

    internal void ToggleShowReachablePins()
    {
        ShowReachablePins = (ReachablePinsSetting)(
            ((int)ShowReachablePins + 1) % Enum.GetNames(typeof(ReachablePinsSetting)).Length
        );
    }

    internal void ToggleQMarkSetting()
    {
        QMarks = (QMarkSetting)(((int)QMarks + 1) % Enum.GetNames(typeof(QMarkSetting)).Length);
    }

    internal void TogglePathfinderSequenceBreaks()
    {
        PathfinderSequenceBreaks = (SequenceBreakSetting)(
            ((int)PathfinderSequenceBreaks + 1) % Enum.GetNames(typeof(SequenceBreakSetting)).Length
        );
        ;
    }

    internal void ToggleRouteCompassEnabled()
    {
        ShowRouteCompass = !ShowRouteCompass;
    }

    internal void ToggleRouteTextInGame()
    {
        RouteTextInGame = (RouteTextInGame)(((int)RouteTextInGame + 1) % Enum.GetNames(typeof(RouteTextInGame)).Length);
    }

    internal void ToggleWhenOffRoute()
    {
        WhenOffRoute = (OffRouteBehaviour)(((int)WhenOffRoute + 1) % Enum.GetNames(typeof(OffRouteBehaviour)).Length);
    }

    internal void ToggleItemCompassMode()
    {
        ItemCompassMode = (ItemCompassMode)(((int)ItemCompassMode + 1) % Enum.GetNames(typeof(ItemCompassMode)).Length);
    }

    internal void ToggleQuickMapCompasses()
    {
        ShowQuickMapCompasses = (QuickMapCompassSetting)(
            ((int)ShowQuickMapCompasses + 1) % Enum.GetNames(typeof(QuickMapCompassSetting)).Length
        );
    }

    internal void ToggleAreaNames()
    {
        ShowAreaNames = !ShowAreaNames;
    }

    internal void ToggleNextAreas()
    {
        ShowNextAreas = (NextAreaSetting)(((int)ShowNextAreas + 1) % Enum.GetNames(typeof(NextAreaSetting)).Length);
    }

    internal void ToggleMapMarkers()
    {
        ShowMapMarkers = !ShowMapMarkers;
    }

    internal void ToggleAlwaysHaveQuill()
    {
        AlwaysHaveQuill = !AlwaysHaveQuill;
    }

    internal void ToggleDefaultItemRandoMode()
    {
        DefaultItemRandoMode = (RmcMode)(((int)DefaultItemRandoMode + 1) % Enum.GetNames(typeof(RmcMode)).Length);
    }

    internal void ToggleDefaultTransitionRandoMode()
    {
        DefaultTransitionRandoMode = (RmcMode)(
            ((int)DefaultTransitionRandoMode + 1) % Enum.GetNames(typeof(RmcMode)).Length
        );
    }
}
