using DbcliModels.TaskModels;
using Newtonsoft.Json;

namespace DbcliModels.TaskModels;

public class Task18Model
{
    [JsonProperty("bestPath")]
    public required BestPathModel BestPath { get; set; }
    
    [JsonProperty("maxPopularity")]
    public required int MaxPopularity { get; set; }

    public override string ToString()
    {
        return $"{BestPath}, Max Popularity : {MaxPopularity}";
    }
}