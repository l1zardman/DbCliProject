using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace DbcliArangoLoader;

public class TaxonomyLoader {
    private Dictionary<String, String> _taxonomyData = new Dictionary<String, String>();

    public Dictionary<String, String> TaxonomyData => _taxonomyData;

    public TaxonomyLoader() { }

    public async Task LoadTaxonomyData(String filePath) {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
            Delimiter = ",", // Default delimiter (comma)
            Escape = '\\', // Set escape character to \
            HasHeaderRecord = false // Assuming no header in your example file
        };

        try {
            // Read the CSV file using CsvReader
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config)) {
                // Parse the rows into a list of key-value pairs

                while (await csv.ReadAsync()) {
                    var record = new TaxonomyKeyValue {
                        Key = csv.GetField(0), // First column
                        Value = csv.GetField(1) // Second column
                    };

                    if (!_taxonomyData.ContainsKey(record.Key)) {
                        _taxonomyData.Add(key: record.Key, Guid.NewGuid().ToString());
                    }

                    if (!_taxonomyData.ContainsKey(record.Value)) {
                        _taxonomyData.Add(record.Value, Guid.NewGuid().ToString());
                    }
                }
            }
        }
        catch (Exception e) {
            Console.WriteLine($"Error reading CSV file: {e}");
        }
    }
}

// Define a simple class for the key-value structure
public class TaxonomyKeyValue {
    public string Key { get; set; }
    public string Value { get; set; }
}