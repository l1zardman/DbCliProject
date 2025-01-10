using Newtonsoft.Json;

namespace DbcliCoreUtility;

public class VertexEdgeModel
{
    [JsonProperty("vertices")]
    public required List<VertexModel> Vertices { get; set; }
    
    [JsonProperty("edges")]
    public required List<EdgeModel> Edges { get; set; }
}