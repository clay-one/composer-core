using System.Collections.Generic;
using ComposerCore.Tests.Performance.Scenarios;

namespace ComposerCore.Tests.Performance
{
    class Program
    {
        static void Main()
        {
            var scenarios = BuildScenarioList();
            foreach (var scenario in scenarios)
            {
                scenario.Run();
            }
        }
        
        private static IEnumerable<ITestScenario> BuildScenarioList()
        {
            return new ITestScenario[]
            {
                new SimpleCachedQuery(),
                new SimpleUncachedQuery(), 
                new PropertyInjection(), 
                new ConstructorInjection(), 
                new ArrayOfSimilarlyNamed(), 
                new ArrayOfDefferentlyNamed(), 
                new OpenGeneric(), 
                new RegisterAndResolve(), 
                new PrepareContext(), 
                new PrepareContextAndRegister(), 
                new PrepareContextAndRegisterAndResolve()
            };
        }
    }
}