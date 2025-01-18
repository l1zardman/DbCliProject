using DbcliCoreUtility;
using DcliCsvFix;
using Newtonsoft.Json;

namespace DatabasesProjectFinal;

public class CommandManager
{
    ConfigParameters _parameters;
    
    public CommandManager(string configFilePath = "./dbcli_config.json")
    {
        try
        {
            var json = File.ReadAllText(configFilePath);
            // Console.WriteLine(json);
            ConfigParameters parameters = JsonConvert.DeserializeObject<ConfigParameters>(json);
            
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task FixCsv(string[] args)
    {
        try
        {
            if (args == null || args.Length != 3)
            {
                throw new ArgumentException("Invalid number of arguments. Expected 3 arguments.");
            }

            string readFilePath = args[1];
            string writeFilePath = args[2];
            
            await CsvFixer.PopularityCsvRepairAsync(readFilePath, writeFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public async Task LoadToArangoDb(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask1(string[] args)
    {
        try
        {
            if (args == null || args.Length != 2)
            {
                throw new ArgumentException("Invalid number of arguments. Expected 2 arguments.");
            }
            
            string nodeName = args[1];
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public async Task PrepareForTask2(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask3(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask4(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask5(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask6(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask7(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask8(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask9(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask10(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask11(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask12(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask13(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask14(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask15(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask16(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask17(string[] args)
    {
        throw new NotImplementedException();
    }

    public async Task PrepareForTask18(string[] args)
    {
        throw new NotImplementedException();
    }

// public bool ValidateArgs(string[] args)
// {
//     
// }
}

public enum TasksEnum
{
    Task0 = 0,
    Task1 = 1,
    Task2 = 2,
    Task3 = 3,
    Task4 = 4,
    Task5 = 5,
    Task6 = 6,
    Task7 = 7,
    Task8 = 8,
    Task9 = 9,
    Task10 = 10,
    Task11 = 11,
    Task12 = 12,
    Task13 = 13,
    Task14 = 14,
    Task15 = 15,
    Task16 = 16,
    Task17 = 17,
    Task18 = 18,
}