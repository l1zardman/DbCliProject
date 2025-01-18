using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace ArangoDbLoader;

public class PopularityLoader {
    private Dictionary<String, int?> _popularityData = new();
    
    public Dictionary<String, int?> PopularityData => _popularityData;

    public PopularityLoader () { }
    
    public async Task LoadPopularityData (String filePath) {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
            Delimiter = ",", // Default delimiter (comma)
            HasHeaderRecord = false // Assuming no header in your example file
        };
        
        try {
            // Read the CSV file using CsvReader
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config)) {
                // Parse the rows into a list of key-value pairs
                
                while (await csv.ReadAsync()) {
                    int ret;
                    int.TryParse(csv.GetField(1), out ret);
                    
                    var record = new PopularityKeyValue {
                        Key = csv.GetField(0), // First column
                         // Second column
                        Value = ret
                    };
                    
                    if (!_popularityData.ContainsKey(record.Key)) {
                        _popularityData.Add(key: record.Key, record.Value);
                    }
                }
            }
        }
        catch (Exception e) {
            Console.WriteLine($"Error reading popularity CSV file: {e.Message}");
        }
    }
}

class PopularityKeyValue {
    public string Key { get; set; }
    public int Value { get; set; }
}