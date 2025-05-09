using MapChanger.UI;

namespace RandoMapCore.UI;

internal class RmcUIBuilder
{
    private readonly Title _title = new RmcTitle();

    private readonly MainButton[] _mainButtons =
    [
        new ModEnabledButton(),
        new ModeButton(),
        new PinSizeButton(),
        new PinShapeButton(),
        new RandomizedButton(),
        new VanillaButton(),
        new SpoilersButton(),
        new PoolOptionsPanelButton(),
        new PinOptionsPanelButton(),
        new PathfinderOptionsPanelButton(),
        new MiscOptionsPanelButton(),
    ];

    private readonly ExtraButtonPanel[] _extraButtonPanels =
    [
        new PoolOptionsPanel(),
        new PinOptionsPanel(),
        new PathfinderOptionsPanel(),
        new MiscOptionsPanel(),
    ];

    internal void Build()
    {
        // Construct pause menu
        PauseMenu.Add(_title);

        foreach (var button in _mainButtons)
        {
            PauseMenu.Add(button);
        }

        foreach (var ebp in _extraButtonPanels)
        {
            PauseMenu.Add(ebp);
        }

        // Construct map UI
        List<MapUILayer> mapUILayers =
        [
            new ControlPanel(),
            new TopLeftPanels(),
            new RmcBottomRowText(),
            new QuickMapTransitions(),
        ];

        if (RandoMapCoreMod.Data.EnablePinSelection || RandoMapCoreMod.Data.EnableRoomSelection)
        {
            mapUILayers.Add(new SelectionPanels());
        }

        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            mapUILayers.AddRange([new RouteSummaryText(), new RouteText()]);
        }

        foreach (var uiLayer in mapUILayers)
        {
            MapUILayerUpdater.Add(uiLayer);
        }
    }
}
