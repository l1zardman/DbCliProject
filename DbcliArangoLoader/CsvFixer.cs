using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace DbcliArangoLoader;

public class CsvFixer
{
    public static async Task PopularityCsvRepairAsync(string readFilePath, string writeFilePath)
    {
        try
        {
            await using var readFile
                = new FileStream(readFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            using var reader = new StreamReader(readFile, Encoding.UTF8);

            await using var writeFile =
                new FileStream(writeFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
            using var writer = new StreamWriter(writeFile, Encoding.UTF8);

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                string newLine = line.Replace("\"", "");
                newLine = "\"" + newLine.Replace(",", "\",");
                string outputString = newLine + "\n";

                // Debugging output (optional)
                // Console.WriteLine(outputString);

                // Write the modified line to the output file
                // csvWriter.WriteRecord(newLine);
                // await csvWriter.NextRecordAsync();

                await writer.WriteAsync(outputString);
            }

            await writer.FlushAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}