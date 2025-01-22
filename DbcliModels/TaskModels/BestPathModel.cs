using DbcliModels.TaskModels;
using Newtonsoft.Json;

namespace DbcliModels.TaskModels;

public class BestPathModel
{
    [JsonProperty("vertices")]
    public required List<VertexModel> Vertices { get; set; }
    
    [JsonProperty("edges")]
    public required List<EdgeModel> Edges { get; set; }
    
    [JsonProperty("weight")]
    public required int Weight { get; set; }

    public override string ToString()
    {
        var vertices = "[" + string.Join(", ", Vertices) + "]";
        // var edges = "[" + string.Join(", ", Edges) + "]";
        return $"Path:\n\nVertices : {vertices}\nWeight : {Weight}";
    }
}