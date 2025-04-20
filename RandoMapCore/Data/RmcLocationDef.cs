namespace RandoMapCore.Data;

public record RmcLocationDef
{
    public string Name { get; init; }
    public string SceneName { get; init; }

    public string GetMapArea()
    {
        return RandoMapCoreMod.Data.GetMapArea(SceneName);
    }
}
