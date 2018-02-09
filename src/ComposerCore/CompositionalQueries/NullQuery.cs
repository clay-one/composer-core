using ComposerCore.Extensibility;

namespace ComposerCore.CompositionalQueries
{
    public class NullQuery : ICompositionalQuery
    {
        public object Query(IComposer composer)
        {
            return null;
        }

        public override string ToString()
        {
            return "NullQuery()";
        }
    }
}