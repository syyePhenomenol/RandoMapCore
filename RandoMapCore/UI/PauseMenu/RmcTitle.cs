using MapChanger.UI;

namespace RandoMapCore.UI;

internal class RmcTitle : Title
{
    private string _hoveredText;

    internal RmcTitle()
        : base("Mod Title", RandoMapCoreMod.Data.ModName)
    {
        Instance = this;
    }

    internal static RmcTitle Instance { get; private set; }

    internal string HoveredText
    {
        get => _hoveredText;
        set
        {
            _hoveredText = value;
            Update();
        }
    }

    public override void Update()
    {
        base.Update();

        if (_hoveredText is not null)
        {
            TitleText.Text = _hoveredText;
        }

        TitleText.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
    }
}
