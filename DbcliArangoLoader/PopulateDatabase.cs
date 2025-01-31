using System.Globalization;
using ArangoDBNetStandard;
using ArangoDBNetStandard.DatabaseApi.Models;
using ArangoDBNetStandard.GraphApi.Models;
using ArangoDBNetStandard.Transport.Http;
using CsvHelper;
using CsvHelper.Configuration;
using DbcliModels;

namespace DbcliArangoLoader;

public class PopulateDatabase
{
    private ConfigParameters _config;

    public Uri ArangoUri { get; private set; }

    public PopulateDatabase(ConfigParameters config)
    {
        _config = config;
        ArangoUri = new Uri(_config.DbUri);
    }

    public async Task DropIfExist()
    {
        using (var transport = HttpApiTransport.UsingBasicAuth(ArangoUri, _config.SystemDb, _config.Username, _config.Password))
        {
            using (var db = new ArangoDBClient(transport))
            {
                var response = await db.Database.GetDatabasesAsync();
                var lst = response.Result as List<string>;

                if (lst.Contains(_config.DbName))
                {
                    var deleteResponse = await db.Database.DeleteDatabaseAsync(_config.DbName);
                }
            }
        }
    }

    public async Task CreateDb()
    {
        try
        {
            using (var transport = HttpApiTransport.UsingBasicAuth(ArangoUri, _config.SystemDb, _config.Username, _config.Password))
            {
                using (var db = new ArangoDBClient(transport))
                {
                    var response = await db.Database.GetDatabasesAsync();
                    var lst = response.Result as List<string>;

                    if (!lst.Contains(_config.DbName))
                    {
                        var body = new PostDatabaseBody()
                        {
                            Name = _config.DbName,
                            Users = new List<DatabaseUser>()
                            {
                                new DatabaseUser()
                                {
                                    Username = _config.Username,
                                    Passwd = _config.Password,
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
            Console.WriteLine("Error creating database (Connection error): " + e);
            throw;
        }
    }

    public async Task PrepareGraph()
    {
        try
        {
            using (var transport = HttpApiTransport.UsingBasicAuth(ArangoUri, _config.DbName, _config.Username, _config.Password))
            {
                using (var db = new ArangoDBClient(transport))
                {
                    try
                    {
                        await db.Graph.PostGraphAsync(new PostGraphBody
                            {
                                Name = _config.GraphName,
                                EdgeDefinitions = new List<EdgeDefinition>
                                {
                                    new EdgeDefinition
                                    {
                                        From = new string[] { _config.NodeCollectionName },
                                        To = new string[] { _config.NodeCollectionName },
                                        Collection = _config.EdgeCollectionName
                                    }
                                }
                            }
                        );
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error preparing graph: " + e);
                        throw;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error connecting to arangodb during preparing graph: " + e);
            throw;
        }
    }

    public async Task PopulateGraphNodes(Dictionary<String, String> taxonomyData, Dictionary<String, int?> popularityData)
    {
        try
        {
            using var transport = HttpApiTransport.UsingBasicAuth(ArangoUri, _config.DbName, _config.Username, _config.Password);
            using var db = new ArangoDBClient(transport);

            var keys = taxonomyData.Keys.ToList();

            await Parallel.ForEachAsync(keys, async (s, token) =>
                {
                    await db.Document.PostDocumentAsync(_config.NodeCollectionName, new
                        {
                            _key = taxonomyData[s],
                            name = s,
                            popularity_score = popularityData.GetValueOrDefault(s)
                        }, token: token
                    );
                }
            );
        }
        catch (Exception e)
        {
            Console.WriteLine("Error populating graph nodes: " + e);
            throw;
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

            using var transport = HttpApiTransport.UsingBasicAuth(ArangoUri, _config.DbName, _config.Username, _config.Password);
            using var db = new ArangoDBClient(transport);
            await Parallel.ForEachAsync(records, async (record, token) => await _CreateEdge(db, taxonomyDictionary[record.Key], taxonomyDictionary[record.Value]));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error reading taxonomy CSV file during creating edges: {e}");
            throw;
        }
    }

    private async Task _CreateEdge(ArangoDBClient db, string srcKey, string dstKey)
    {
        try
        {
            await db.Graph.PostEdgeAsync(_config.GraphName, _config.EdgeCollectionName, new
                {
                    _from = $"{_config.NodeCollectionName}/{srcKey}",
                    _to = $"{_config.NodeCollectionName}/{dstKey}"
                }
            );
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error creating edge in _CreateEdge: {e}");
            throw;
        }
    }

    public static async Task Execute(ConfigParameters config, string taxonomyPath, string popularityPath)
    {
        var taxonomy = new TaxonomyLoader();
        Console.WriteLine("Loading Taxonomy Data...");
        await taxonomy.LoadTaxonomyData(taxonomyPath);

        var popularity = new PopularityLoader();
        Console.WriteLine("Loading Popularity Data...");
        await popularity.LoadPopularityData(popularityPath);

        var pd = new PopulateDatabase(config);

        await pd.DropIfExist();

        await pd.CreateDb();

        await pd.PrepareGraph();

        await pd.PopulateGraphNodes(taxonomy.TaxonomyData, popularity.PopularityData);

        await pd.MakeEdges(taxonomyPath, taxonomy.TaxonomyData);

        Console.WriteLine("Successfully Created Db!");
    }
}