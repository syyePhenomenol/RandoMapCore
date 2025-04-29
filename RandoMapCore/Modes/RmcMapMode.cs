using MapChanger;

namespace RandoMapCore.Modes;

public class RmcMapMode : MapMode
{
    public override string Mod => RandoMapCoreMod.Data.ModName;
    public override float Priority => 0f;
    public override bool ForceHasMap => true;
    public override bool ForceHasQuill => RandoMapCoreMod.GS.AlwaysHaveQuill;
    public override bool? VanillaPins => false;
    public override bool? MapMarkers => RandoMapCoreMod.GS.ShowMapMarkers ? null : false;
    public override bool ImmediateMapUpdate => true;
    public override bool FullMap => true;

    public override bool InitializeToThis()
    {
        if (ModeName == RandoMapCoreMod.Data.ForceMapMode)
        {
            return true;
        }

        if (RandoMapCoreMod.Data.RandomizedTransitions.Any())
        {
            return ModeName == RandoMapCoreMod.GS.DefaultTransitionRandoMode.ToString().ToCleanName();
        }

        return ModeName == RandoMapCoreMod.GS.DefaultItemRandoMode.ToString().ToCleanName();
    }
}
