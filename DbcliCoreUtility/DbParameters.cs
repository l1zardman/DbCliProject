using Newtonsoft.Json;

namespace DbcliCoreUtility;

public class DbParameters
{
    [JsonProperty("db_uri")]
    public required string DbUri { get; set; }

    [JsonProperty("db_name")]
    public required string DbName { get; set; }

    [JsonProperty("username")]
    public required string Username { get; set; }

    [JsonProperty("password")]
    public required string Password { get; set; }

    // add Node collection name
    [JsonProperty("collection_name")]
    public required string NodeCollectionName { get; set; }

    [JsonProperty("edge_collection_name")]
    public required string EdgeCollectionName { get; set; }

    [JsonProperty("graph_name")]
    public required string GraphName { get; set; }
}