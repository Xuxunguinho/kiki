using System.Collections.Generic;

namespace Avax
{
    public interface IAvaxConfiguration
    {
        Dictionary<string, string> CollectionsClass { get; set; }
        Dictionary<string, object> Results { get; set; }
    }
}