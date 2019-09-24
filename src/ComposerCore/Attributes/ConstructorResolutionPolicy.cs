namespace ComposerCore.Attributes
{
    public enum ConstructorResolutionPolicy : byte
    {
        Explicit = 0,
        DefaultConstructor = 1,
        SingleOrDefault = 2,
        MostParameters = 3,
        LeastParameters = 4,
        MostResolvable = 5,
        Custom = 200
    }
}