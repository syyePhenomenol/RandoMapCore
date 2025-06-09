namespace RandoMapCore.Settings;

public enum ProgressHintSetting
{
    Off,
    Area,
    Room,
    Location,
}

public enum PinShape
{
    Circle,
    Diamond,
    Square,
    Pentagon,
    Hexagon,
}

public enum PinSize
{
    Tiny,
    Small,
    Medium,
    Large,
    Huge,
}

public enum PinShapeSetting
{
    Mixed,
    Circles,
    Diamonds,
    Squares,
    Pentagons,
    Hexagons,
    NoBorders,
}

public enum ClearedPinsSetting
{
    Off,
    Persistent,
    All,
}

public enum ReachablePinsSetting
{
    HideUnreachable,
    ExpandReachable,
    ExpandAll,
}

public enum QMarkSetting
{
    Off,
    Red,
    Mixed,
}

public enum SequenceBreakSetting
{
    Off,
    SequenceBreaks,
    SuperSequenceBreaks,
}

public enum RouteTextInGame
{
    Hide,
    AllTransitions,
    NextTransition,
}

public enum OffRouteBehaviour
{
    KeepRoute,
    CancelRoute,
    Reevaluate,
}

public enum ItemCompassMode
{
    Reachable,
    ReachableOutOfLogic,
    All,
}

public enum QuickMapCompassSetting
{
    Unchecked,
    All,
}

public enum NextAreaSetting
{
    Off,
    Arrows,
    Full,
}

public enum RmcMode
{
    Full_Map,
    All_Pins,
    Pins_Over_Area,
    Pins_Over_Room,
    Transition_Normal,
    Transition_Visited_Only,
    Transition_All_Rooms,
}

public enum GroupBySetting
{
    Location,
    Item,
}

public enum PoolState
{
    Off,
    On,
    Mixed,
}
