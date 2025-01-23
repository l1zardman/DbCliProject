using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace DbcliArangoLoader;

public class PopularityLoader
{
    private Dictionary<String, int?> _popularityData = new();

    public Dictionary<String, int?> PopularityData => _popularityData;

    public PopularityLoader() { }

    public async Task LoadPopularityData(String filePath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = false
        };

        try
        {
            using (var reader = new StreamReader(filePath))

            using (var csv = new CsvReader(reader, config))
            {
                while (await csv.ReadAsync())
                {
                    int ret;
                    int.TryParse(csv.GetField(1), out ret);

                    var record = new PopularityKeyValue
                    {
                        Key = csv.GetField(0),
                        Value = ret
                    };

                    if (!_popularityData.ContainsKey(record.Key))
                    {
                        _popularityData.Add(key: record.Key, record.Value);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error reading popularity CSV file: {e}");
        }
    }
}

class PopularityKeyValue
{
    public string Key { get; set; }
    public int Value { get; set; }
}