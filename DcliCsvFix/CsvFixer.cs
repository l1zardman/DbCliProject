using System.Text;

namespace DcliCsvFix;

public class CsvFixer
{
    public static async Task PopularityCsvRepairAsync(string readFilePath, string writeFilePath)
    {
        try
        {
            // Open the output file for writing
            await using var writeFile =
                new FileStream(writeFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
            using var writer = new StreamWriter(writeFile, Encoding.UTF8);

            // Open the input file for reading
            await using var readFile = new FileStream(readFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            using var reader = new StreamReader(readFile, Encoding.UTF8);

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                // Process the line
                string newLine = line.Replace("\"", ""); // Remove quotes
                newLine = "\"" + newLine.Replace(",", "\","); // Add quotes and replace commas
                string outputString = newLine + "\n"; // Add a newline

                // Debugging output (optional)
                // Console.WriteLine(outputString);

                // Write the modified line to the output file
                await writer.WriteAsync(outputString);
            }

            // Ensure all buffered content is written
            await writer.FlushAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}