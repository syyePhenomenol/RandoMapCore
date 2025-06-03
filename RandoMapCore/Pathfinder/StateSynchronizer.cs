using RandoMapCore.Pathfinder;
using RandomizerCore.Logic.StateLogic;

namespace RCPathfinder;

internal class StateSynchronizer
{
    private readonly StateManager _sm;

    internal StateSynchronizer(StateManager stateManager)
    {
        _sm = stateManager;
        Update();
    }

    internal StateUnion CurrentState { get; private set; }

    internal void Update()
    {
        var pd = PlayerData.instance;

        // Update current state
        StateBuilder sb = new(_sm);

        // USEDSHADE
        TrySetStateBool("OVERCHARMED", pd.GetBool(nameof(PlayerData.overcharmed)));
        // CANNOTOVERCHARM
        // CANNOTREGAINSOUL
        // TrySetStateBool("SPENTALLSOUL", pd.GetInt(nameof(PlayerData.MPCharge)) is 0 && pd.GetInt(nameof(PlayerData.MPReserve)) is 0);
        // CANNOTSHADESKIP
        TrySetStateBool("BROKEHEART", pd.GetBool(nameof(PlayerData.brokenCharm_23)));
        TrySetStateBool("BROKEGREED", pd.GetBool(nameof(PlayerData.brokenCharm_24)));
        TrySetStateBool("BROKESTRENGTH", pd.GetBool(nameof(PlayerData.brokenCharm_25)));
        TrySetStateBool("NOFLOWER", !pd.GetBool(nameof(PlayerData.hasXunFlower)));
        // NOPASSEDCHARMEQUIP
        for (var i = 1; i <= 40; i++)
        {
            TrySetStateBool($"CHARM{i}", pd.GetBool($"equippedCharm_{i}"));
            TrySetStateBool($"noCHARM{i}", !pd.GetBool($"gotCharm_{i}"));
        }

        // SPENTSOUL
        // SPENTRESERVESOUL
        // SOULLIMITER
        // REQUIREDMAXSOUL
        // SPENTHP
        // SPENTBLUEHP
        // LAZYSPENTHP
        TrySetStateInt("USEDNOTCHES", pd.GetInt(nameof(PlayerData.charmSlotsFilled)));
        TrySetStateInt("MAXNOTCHCOST", pd.GetInt(nameof(PlayerData.charmSlots)));

        CurrentState = new((State)new(sb));

        void TrySetStateBool(string name, bool value) => sb.TrySetStateBool(_sm, name, value);

        void TrySetStateInt(string name, int value) => sb.TrySetStateInt(_sm, name, value);
    }
}
