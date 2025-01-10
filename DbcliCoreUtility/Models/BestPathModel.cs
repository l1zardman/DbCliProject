using Newtonsoft.Json;

namespace DbcliCoreUtility;

public class BestPathModel
{
    [JsonProperty("vertices")]
    public required List<VertexModel> Vertices { get; set; }
    
    [JsonProperty("edges")]
    public required List<EdgeModel> Edges { get; set; }
    
    [JsonProperty("weight")]
    public required int Weight { get; set; }
    
   
}