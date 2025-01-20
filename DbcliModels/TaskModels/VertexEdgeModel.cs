using Newtonsoft.Json;

namespace DbcliModels.TaskModels;

public class VertexEdgeModel
{
    [JsonProperty("vertices")]
    public required List<VertexModel> Vertices { get; set; }
    
    [JsonProperty("edges")]
    public required List<EdgeModel> Edges { get; set; }
}