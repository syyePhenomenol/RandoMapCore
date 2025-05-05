using ItemChanger;
using MapChanger.Defs;

namespace RandoMapCore.Pins;

internal sealed class StartBenchPinDef : BenchPinDef
{
    internal StartBenchPinDef()
        : base(BenchwarpInterop.BENCH_WARP_START, ItemChanger.Internal.Ref.Settings.Start?.SceneName ?? SceneNames.Tutorial_01) { }

    private protected override MapRoomPosition GetBenchMapPosition()
    {
        var start = ItemChanger.Internal.Ref.Settings.Start;

        if (MapChanger.Finder.IsMappedScene(SceneName))
        {
            return new WorldMapPosition((SceneName, start?.X ?? 0, start?.Y ?? 0));
        }
        else
        {
            return new MapRoomPosition((MapChanger.Finder.GetMappedScene(SceneName), 0, 0));
        }
    }
}
