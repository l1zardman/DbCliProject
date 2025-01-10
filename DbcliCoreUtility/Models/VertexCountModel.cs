using Newtonsoft.Json;

namespace DbcliCoreUtility;

public class VertexCountModel
{
    [JsonProperty("node")]
    public required VertexModel Vertex { get; set; }
    [JsonProperty("count")]
    public required int Count { get; set; }
}