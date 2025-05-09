﻿using MapChanger.MonoBehaviours;

namespace RandoMapCore.Rooms;

internal abstract class RoomSelector : Selector
{
    public override float SelectionRadius { get; } = 2.5f;

    public override float SpriteSize { get; } = 0.6f;

    public override void Initialize(IEnumerable<ISelectable> rooms)
    {
        base.Initialize(rooms);

        ActiveModifiers.AddRange([ActiveByCurrentMode, ActiveByToggle]);
    }

    private protected abstract bool ActiveByCurrentMode();
    private protected abstract bool ActiveByToggle();

    public override void OnMainUpdate(bool active)
    {
        base.OnMainUpdate(active);

        SpriteObject.SetActive(RandoMapCoreMod.GS.ShowReticle);
    }

    protected override void Select(ISelectable selectable)
    {
        selectable.Selected = true;
    }

    protected override void Deselect(ISelectable selectable)
    {
        selectable.Selected = false;
    }
}
