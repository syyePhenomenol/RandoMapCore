using GlobalEnums;
using MagicUI.Core;
using MagicUI.Elements;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Modes;
using RandoMapCore.Pathfinder;
using RandoMapCore.Settings;
using RandoMapCore.Transition;
using UnityEngine;

namespace RandoMapCore.UI;

internal class QuickMapLayout : MapLayout
{
    private readonly StackLayout _stack;

    public QuickMapLayout()
        : base(RandoMapCoreMod.Data.ModName, nameof(QuickMapLayout))
    {
        _stack = new(this, $"{Name} Stack")
        {
            Padding = new(0f, 20f, 20f, 0f),
            Orientation = Orientation.Vertical,
            Spacing = 20f,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
        };
    }

    public override void Update()
    {
        if (_stack is null)
        {
            return;
        }

        _stack.Children.Clear();

        var tsd = new TransitionStringDef(Utils.CurrentScene());
        var showAllCompasses = RandoMapCoreMod.GS.ShowQuickMapCompasses is QuickMapCompassSetting.All;

        MakeTransitionSubsection([tsd.RandomizedUncheckedReachable], "Unchecked Reachable".L(), true);
        MakeTransitionSubsection([tsd.RandomizedUncheckedUnreachable], "Unchecked Unreachable".L(), true);
        MakeTransitionSubsection([tsd.RandomizedUncheckedIn], "Unchecked (in)".L(), showAllCompasses);
        MakeTransitionSubsection([tsd.RandomizedVisitedOut, tsd.RandomizedVisitedIn], "Visited".L(), showAllCompasses);
        MakeTransitionSubsection(
            [tsd.VanillaReachableOut, tsd.VanillaReachableIn],
            "Vanilla Reachable".L(),
            showAllCompasses
        );
        MakeTransitionSubsection(
            [tsd.VanillaUnreachableOut, tsd.VanillaUnreachableIn],
            "Vanilla Unreachable".L(),
            showAllCompasses
        );
    }

    protected override bool ActiveCondition()
    {
        return Conditions.TransitionRandoModeEnabled() && States.QuickMapOpen;
    }

    protected override void OnOpenWorldMap(GameMap obj) { }

    protected override void OnOpenQuickMap(GameMap gameMap, MapZone mapZone)
    {
        if (ActiveCondition())
        {
            Update();
        }
    }

    protected override void OnCloseMap(GameMap obj)
    {
        _stack?.Children?.Clear();
    }

    private void MakeTransitionSubsection(
        IEnumerable<TransitionStringList> transitionLists,
        string header,
        bool showCompasses
    )
    {
        if (!transitionLists.Any(tsl => tsl.Placements.Any()))
        {
            return;
        }

        StackLayout listStack =
            new(this, $"{Name} {header}")
            {
                Orientation = Orientation.Vertical,
                Spacing = 10f,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
            };

        _stack.Children.Add(listStack);

        listStack.Children.Add(RightCenteredText($"Header {header}", $"{header}:"));

        foreach (var list in transitionLists)
        {
            if (!list.Placements.Any())
            {
                continue;
            }

            if (list is OutTransitionStringList && showCompasses)
            {
                foreach (var placement in list.Placements)
                {
                    StackLayout lineStack =
                        new(this, $"{Name} Unchecked Line Stack {placement}")
                        {
                            Orientation = Orientation.Horizontal,
                            Spacing = 10f,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            VerticalAlignment = VerticalAlignment.Center,
                        };

                    listStack.Children.Add(lineStack);

                    lineStack.Children.Add(
                        RightCenteredText(
                            $"{Name} Unchecked Placement {placement.Key.Name}",
                            list.GetFormattedPlacement(placement.Key, placement.Value)
                        )
                    );

                    if (RmcPathfinder.SD.TransitionActions.TryGetValue(placement.Key.Name, out var ta))
                    {
                        lineStack.Children.Add(
                            new QuickMapCompass(this, placement.Key.Name, new TransitionCompassPosition(ta.CompassObj))
                        );
                    }
                }
            }
            else
            {
                listStack.Children.Add(
                    RightCenteredText($"Text {header}", RunCollection.Join("\n", list.GetFormattedPlacements()))
                );
            }
        }
    }

    private TextObject RightCenteredText(string name, string text)
    {
        return RightCenteredText(name, new RunCollection(new Run(text)));
    }

    private TextObject RightCenteredText(string name, RunCollection text)
    {
        return new TextObject(this, name)
        {
            Inlines = text,
            ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = HorizontalAlignment.Right,
            Font = MagicUI.Core.UI.TrajanNormal,
            FontSize = (int)(14f * MapChangerMod.GS.UIScale),
        };
    }

    private class QuickMapCompass : ArrangableElement, IGameObjectWrapper
    {
        private static readonly Vector2 _correction = new(15.5f, 8.7f);
        private readonly TransitionCompassPosition _tcp;

        private readonly Vector2 _cameraPosition;

        public QuickMapCompass(LayoutRoot onLayout, string transition, TransitionCompassPosition tcp)
            : base(onLayout, $"Quick Map Compass {transition}")
        {
            _tcp = tcp;
            _cameraPosition = (Vector2)GameCameras.instance?.tk2dCam?.transform?.position;

            GameObject = new(Name, typeof(SpriteRenderer));

            var sr = GameObject.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "HUD";
            sr.sprite = new EmbeddedSprite("GUI.ArrowInCircle").Value;

            GameObject.layer = 5;
            UnityEngine.Object.DontDestroyOnLoad(GameObject);
            GameObject.SetActive(false);
        }

        public GameObject GameObject { get; }

        protected override void ArrangeOverride(Vector2 alignedTopLeftCorner)
        {
            var dir = (Vector2)_tcp.Value - _cameraPosition;
            var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90f;

            var screenPos = alignedTopLeftCorner + new Vector2(15f, 15f);
            var viewportPos = Camera.main.ScreenToViewportPoint(screenPos);
            Vector3 scaledPos =
                new(
                    (viewportPos.x * _correction.x * 2f) - _correction.x,
                    _correction.y - (viewportPos.y * _correction.y * 2f),
                    -25f
                );

            GameObject.transform.eulerAngles = new Vector3(0, 0, angle);
            GameObject.transform.position = scaledPos;
            GameObject.transform.localScale = Vector3.one * 0.7f * MapChangerMod.GS.UIScale;
            GameObject.SetActive(IsEffectivelyVisible);
        }

        protected override void DestroyOverride()
        {
            UnityEngine.Object.Destroy(GameObject);
        }

        protected override Vector2 MeasureOverride()
        {
            return new(30f, 30f);
        }
    }
}
