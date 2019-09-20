using ComposerCore.Attributes;

namespace WebApiSample.Components
{
    [Contract]
    public interface IValueProvider
    {
        string[] GetValues();
    }
}