using Newtonsoft.Json;

namespace DbcliCoreUtility;

public class Task18Model
{
    [JsonProperty("bestPath")]
    public required BestPathModel BestPath { get; set; }
    
    [JsonProperty("maxPopularity")]
    public required int MaxPopularity { get; set; }
}