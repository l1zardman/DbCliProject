using Newtonsoft.Json;

namespace DbcliModels.TaskModels;

public class VertexModel
{
    [JsonProperty("_key")]
    public required string Key { get; set; }
    
    [JsonProperty("_id")]
    public required string Id { get; set; }
    
    [JsonProperty("_rev")]
    public required string Rev { get; set; }
    
    [JsonProperty("name")]
    public required string Name { get; set; }

    public override string ToString()
    {
        return $"Vertex_Name: {Name}";
    }
}