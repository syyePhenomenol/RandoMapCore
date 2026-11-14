namespace RandoMapCore.Pins;

internal class StagInfo(string name)
{
    internal bool IsActiveStag { get; private set; }

    internal void Update()
    {
        IsActiveStag = MoreStags.MoreStags.localData.activeStags.Any(data => MoreStagsInterop.FormatStagName(data.name) == name);
    }
}