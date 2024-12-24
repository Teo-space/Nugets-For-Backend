using Newtonsoft.Json.FluentConfiguration.Models;

namespace Newtonsoft.Json.FluentConfiguration.Interfaces;

public interface ISerializationSettings
{
    IEnumerable<Rule> Rules { get; }
}
