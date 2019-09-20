using ComposerCore.Attributes;

namespace WebApiSample.Components
{
    [Component, Transient]
    public class ValueProviderComponent : IValueProvider
    {
        public string[] GetValues()
        {
            return new[] {"value1", "value2"};
        }
    }
}