using ComposerCore.Extensibility;

namespace ComposerCore.CompositionalQueries
{
    public class NullQuery : ICompositionalQuery
    {
        public bool IsResolvable(IComposer composer)
        {
            return true;
        }
        
        public object Query(IComposer composer, IComposer scope = null)
        {
            return null;
        }

        public override string ToString()
        {
            return "NullQuery()";
        }
    }
}