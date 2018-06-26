namespace game.Procedual
{
    public interface IMapGeneratorPhase
    {
        void Apply(MapGeneratorState state);
        string Label();
    }
}