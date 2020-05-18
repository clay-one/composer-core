using System;
using System.Linq;
using ComposerCore.Extensibility;

namespace ComposerCore.CompositionalQueries
{
    /// <summary>
    /// Similar to VariableQuery, but takes multiple variable names and tries them in order
    /// returning the first one that has a value in the context.
    /// </summary>
    public class CascadeVariableQuery : ICompositionalQuery
    {
        public CascadeVariableQuery(string[] variableNames)
        {
            VariableNames = variableNames ?? throw new ArgumentNullException(nameof(variableNames));
        }

        #region Implementation of ICompositionalQuery

        public bool IsResolvable(IComposer composer)
        {
            var composerToUse = ComposerOverride ?? composer;
            if (composerToUse == null)
                throw new ArgumentNullException(nameof(composer));

            return VariableNames.Where(n => n != null).Any(variableName => composerToUse.HasVariable(variableName));
        }
		
        public object Query(IComposer composer)
        {
            var composerToUse = ComposerOverride ?? composer;
            if (composerToUse == null)
                throw new ArgumentNullException(nameof(composer));

            return VariableNames.Where(n => n != null).Select(n => composerToUse.GetVariable(n)).FirstOrDefault(v => v != null);
        }

        #endregion

        public override string ToString()
        {
            return $"VariableQuery('{string.Join(",", VariableNames)}')";
        }

        public string[] VariableNames { get; }

        /// <summary>
        /// Specifies the instance of IComposer to use for resolving references.
        /// </summary>
        /// <remarks>
        /// Setting this property is not required for the query to work.
        /// If this property is set, its value will be used to resolve the component.
        /// Otherwise, the default instance of the composer (that is passed to
        /// the Query method) will be used to query for the value.
        /// </remarks>
        public IComposer ComposerOverride { get; set; }

    }
}