using Newtonsoft.Json;

namespace DbcliCoreUtility;

public class EdgeModel
{
    [JsonProperty("_key")]
    public required string Key { get; set; }
    
    [JsonProperty("_id")]
    public required string Id { get; set; }

    [JsonProperty("_from")]
    public required string From { get; set; }

    [JsonProperty("_to")]
    public required string To { get; set; }

    [JsonProperty("_rev")]
    public required string Rev { get; set; }

    public override string ToString()
    {
        return $"Edge_From: {From}, Edge_To: {To}";
    }
}