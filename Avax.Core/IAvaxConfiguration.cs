using System.Collections.Generic;

namespace Avax.Core
{
    public interface IAvaxConfiguration
    {
        Dictionary<string, object> CollectionsClass { get; set; }
        Dictionary<string, object> Results { get; set; }
    }
}