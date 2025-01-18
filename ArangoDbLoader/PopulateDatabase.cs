using System.Globalization;
using ArangoDBNetStandard;
using ArangoDBNetStandard.DatabaseApi.Models;
using ArangoDBNetStandard.GraphApi.Models;
using ArangoDBNetStandard.Transport.Http;
using CsvHelper;
using CsvHelper.Configuration;

namespace ArangoDbLoader;

public class PopulateDatabase
{
    public Uri ArangoUri => new Uri("http://localhost:8529");
    public string SystemDb => "_system";
    public string Username => "root";
    public string Password => "Da7f+uhuti";

    public string DbName { get; private set; }
    public string GraphName => "WikipediaTaxonomyGraph";
    public string SourceCollectionName => "TaxonomyVertexCollection";
    public string TargetCollectionName => "TaxonomyVertexCollection";
    public string EdgeCollectionName => "TaxonomyEdgeCollection";

    public PopulateDatabase(string nameForNewDb)
    {
        DbName = nameForNewDb;
    }

    public async Task DropIfExist()
    {
        using (var transport = HttpApiTransport.UsingBasicAuth(ArangoUri, SystemDb, Username, Password))
        {
            using (var db = new ArangoDBClient(transport))
            {
                var response = await db.Database.GetDatabasesAsync();
                var lst = response.Result as List<string>;

                if (lst.Contains(DbName))
                {
                    var deleteResponse = await db.Database.DeleteDatabaseAsync(DbName);
                }
            }
        }
    }

    public async Task CreateDb()
    {
        try
        {
            using (var transport = HttpApiTransport.UsingBasicAuth(ArangoUri, SystemDb, Username, Password))
            {
                using (var db = new ArangoDBClient(transport))
                {
                    var response = await db.Database.GetDatabasesAsync();
                    var lst = response.Result as List<string>;

                    if (!lst.Contains(DbName))
                    {
                        var body = new PostDatabaseBody()
                        {
                            Name = DbName,
                            Users = new List<DatabaseUser>()
                            {
                                new DatabaseUser()
                                {
                                    Username = Username,
                                    Passwd = Password,
                                    Active = true
                                }
                            }
                        };

                        var res = await db.Database.PostDatabaseAsync(body);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error creating database (Connection error): " + e.Message);
            throw;
        }
    }

    public async Task PrepareGraph()
    {
        try
        {
            using (var transport = HttpApiTransport.UsingBasicAuth(ArangoUri, DbName, Username, Password))
            {
                using (var db = new ArangoDBClient(transport))
                {
                    try
                    {
                        await db.Graph.PostGraphAsync(new PostGraphBody
                        {
                            Name = GraphName,
                            EdgeDefinitions = new List<EdgeDefinition>
                            {
                                new EdgeDefinition
                                {
                                    From = new string[] { SourceCollectionName },
                                    To = new string[] { TargetCollectionName },
                                    Collection = EdgeCollectionName
                                }
                            }
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error preparing graph: " + e.Message);
                        throw;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error connecting to arangodb during preparing graph: " + e.Message);
            throw;
        }
    }

    public async Task PopulateGraphNodes(Dictionary<String, String> taxonomyData, Dictionary<String, int?> popularityData)
    {
        try
        {
            using (var transport = HttpApiTransport.UsingBasicAuth(ArangoUri, DbName, Username, Password))
            {
                using (var db = new ArangoDBClient(transport))
                {
                    try
                    {
                        var keys = taxonomyData.Keys.ToList();

                        Parallel.ForEachAsync(keys, async (s, token) =>
                        {
                            await db.Document.PostDocumentAsync(SourceCollectionName, new
                            {
                                _key = taxonomyData[s],
                                name = s,
                                popularity_score = popularityData.ContainsKey(s) ? popularityData[s] : null
                            });
                        }).Wait();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error populating graph nodes: " + e.Message);
                        throw;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error connecting to arangodb: {e.Message}");
        }
    }

    public async Task MakeEdges(String taxonomyPath, Dictionary<String, string> taxonomyDictionary)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",", // Default delimiter (comma)
            Escape = '\\', // Set escape character to \
            HasHeaderRecord = false // Assuming no header in your example file
        };

        try
        {
            // Read the CSV file using CsvReader
            var records = new List<TaxonomyKeyValue>();

            using (var reader = new StreamReader(taxonomyPath))
            using (var csv = new CsvReader(reader, config))
            {
                while (await csv.ReadAsync())
                {
                    var record = new TaxonomyKeyValue
                    {
                        Key = csv.GetField(0), // First column
                        Value = csv.GetField(1) // Second column
                    };
                    records.Add(record);
                }
            }

            Parallel.ForEachAsync(records,
                async (record, token) => { await _CreateEdge(taxonomyDictionary[record.Key], taxonomyDictionary[record.Value]); }).Wait();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error reading taxonomy CSV file during creating edges: {e.Message}");
            throw;
        }
    }

    private async Task _CreateEdge(string srcKey, string dstKey)
    {
        using (var transport = HttpApiTransport.UsingBasicAuth(ArangoUri, DbName, Username, Password))
        {
            using (var db = new ArangoDBClient(transport))
            {
                try
                {
                    await db.Graph.PostEdgeAsync(GraphName, EdgeCollectionName, new
                    {
                        _from = $"{SourceCollectionName}/{srcKey}",
                        _to = $"{TargetCollectionName}/{dstKey}"
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error creating edge in _CreateEdge: {e.Message}");
                    throw;
                }
            }
        }
    }
}