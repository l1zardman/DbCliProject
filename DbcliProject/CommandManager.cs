using ArangoDBNetStandard;
using DbcliArangoLoader;
using DbcliModels;
using DbcliModels.TaskModels;
using Newtonsoft.Json;

namespace DbcliProject;

public class CommandManager
{
    ConfigParameters _parameters;
    DbTasks? _dbTasks = null;

    public CommandManager(ConfigParameters parameters)
    {
        _parameters = parameters;
    }

    public async Task FixCsv(string[] args)
    {
        try
        {
            if (args == null || args.Length != 2)
                throw new ArgumentException("Invalid number of arguments. Expected 2 arguments.");

            var root = Directory.GetCurrentDirectory();
            string popularityRawPath = Path.Combine(root, $"Resources/{_parameters.PopularityFileRaw}");
            string taxonomyPath = Path.Combine(root, $"Resources/{_parameters.TaxonomyFile}");
            string popularityPath = Path.Combine(root, $"Resources/{_parameters.PopularityFile}");

            Console.WriteLine(root);
            Console.WriteLine($"Popularity: {popularityPath}");
            Console.WriteLine($"Taxonomy: {taxonomyPath}");
            
            await CsvFixer.PopularityCsvRepairAsync(popularityRawPath, popularityPath);
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e}");
        }
    }

    public async Task LoadToArangoDb(string[] args)
    {
        if (args == null || args.Length != 2)
            throw new ArgumentException("Invalid number of arguments. Expected 2 arguments.");
        
        var root = Directory.GetCurrentDirectory();
        string popularityRawPath = Path.Combine(root, $"Resources/{_parameters.PopularityFileRaw}");
        string taxonomyPath = Path.Combine(root, $"Resources/{_parameters.TaxonomyFile}");
        string popularityPath = Path.Combine(root, $"Resources/{_parameters.PopularityFile}");

        Console.WriteLine(root);
        Console.WriteLine($"Popularity: {popularityPath}");
        Console.WriteLine($"Taxonomy: {taxonomyPath}");

        await PopulateDatabase.Execute(config: _parameters, taxonomyPath, popularityPath);
    }

    private void _prepareConnection()
    {
        var transport = DbConnector.GetApiTransport(_parameters);
        var client = new ArangoDBClient(transport);
        _dbTasks = new DbTasks(_parameters, client);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask1(string[] args)
    {
        try
        {
            if (args == null || args.Length != 2)
                throw new ArgumentException("Invalid number of arguments. Expected 2 arguments.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            string nodeName = args[1];

            await _dbTasks.DbCliTask1(nodeName);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 1, does correct DB exist? Error message: {e}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask2(string[] args)
    {
        try
        {
            if (args == null || args.Length != 2)
                throw new ArgumentException("Invalid number of arguments. Expected 2 arguments.");
            
            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            string nodeName = args[1];

            await _dbTasks.DbCliTask2(nodeName);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 2, does correct DB exist? Error message: {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask3(string[] args)
    {
        try
        {
            if (args == null || args.Length != 2)
                throw new ArgumentException("Invalid number of arguments. Expected 2 arguments.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            string nodeName = args[1];

            await _dbTasks.DbCliTask3(nodeName);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 3, does correct DB exist? Error message: {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask4(string[] args)
    {
        try
        {
            if (args == null || args.Length != 2)
                throw new ArgumentException("Invalid number of arguments. Expected 2 arguments.");
            

            string nodeName = args[1];

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            await _dbTasks.DbCliTask4(nodeName);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 4, does correct DB exist? Error message: {e}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask5(string[] args)
    {
        try
        {
            if (args == null || args.Length != 2)
                throw new ArgumentException("Invalid number of arguments. Expected 2 arguments.");

            string nodeName = args[1];

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            await _dbTasks.DbCliTask5(nodeName);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 5, does correct DB exist? Error message: {e}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask6(string[] args)
    {
        try
        {
            if (args == null || args.Length != 2)
                throw new ArgumentException("Invalid number of arguments. Expected 2 arguments.");
            

            string nodeName = args[1];

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            await _dbTasks.DbCliTask6(nodeName);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 6, does correct DB exist? Error message: {e}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask7(string[] args)
    {
        try
        {
            if (args == null || args.Length != 1)
                throw new ArgumentException("Invalid number of arguments. Expected 1 argument.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            await _dbTasks.DbCliTask7();
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 1, does correct DB exist? Error message: {{e}}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask8(string[] args)
    {
        try
        {
            if (args == null || args.Length != 1)
                throw new ArgumentException("Invalid number of arguments. Expected 1 argument.");
            
            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            await _dbTasks.DbCliTask8();
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 8, does correct DB exist? Error message: {e}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask9(string[] args)
    {
        try
        {
            if (args == null || args.Length != 1)
                throw new ArgumentException("Invalid number of arguments. Expected 1 argument.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            await _dbTasks.DbCliTask9();
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 9, does correct DB exist? Error message: {e}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask10(string[] args)
    {
        try
        {
            if (args == null || args.Length != 2)
                throw new ArgumentException("Invalid number of arguments. Expected 2 arguments.");
            

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            if (!int.TryParse(args[1], out var depth))
                throw new ArgumentException("Invalid argument at position 2. Expected an integer.");

            await _dbTasks.DbCliTask10(depth);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 10, does correct DB exist? Error message: {e}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"An argument error occurred: {e}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask11(string[] args)
    {
        try
        {
            if (args == null || args.Length != 1)
                throw new ArgumentException("Invalid number of arguments. Expected 1 arguments.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            await _dbTasks.DbCliTask11();
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 11, does correct DB exist? Error message: {e}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"An argument error occurred: {e}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask12(string[] args)
    {
        try
        {
            if (args == null || args.Length != 3)
                throw new ArgumentException("Invalid number of arguments. Expected 2 arguments.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            string nodeName1 = args[1];
            string nodeName2 = args[2];

            await _dbTasks.DbCliTask12(nodeName1, nodeName2);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 12, does correct DB exist? Error message: {e}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"An argument error occurred: {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask13(string[] args)
    {
        try
        {
            if (args == null || args.Length != 3)
                throw new ArgumentException("Invalid number of arguments. Expected 3 arguments.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            string nodeName = args[1];
            if (!int.TryParse(args[2], out var depth))
                throw new ArgumentException("Invalid argument at position 2. Expected an integer.");

            await _dbTasks.DbCliTask13(nodeName, depth);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 13, does correct DB exist? Error message: {e}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"An argument error occurred: {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask14(string[] args)
    {
        try
        {
            if (args == null || args.Length != 3)
                throw new ArgumentException("Invalid number of arguments. Expected 3 arguments.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            string nodeName1 = args[1];
            string nodeName2 = args[2];

            await _dbTasks.DbCliTask14(nodeName1, nodeName2);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null ref exception in trying to run Task 14, does db exist? Err message: {e}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"An argument error occurred: {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask15(string[] args)
    {
        try
        {
            if (args == null || args.Length != 3)
                throw new ArgumentException("Invalid number of arguments. Expected 3 arguments.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            string nodeName1 = args[1];
            string nodeName2 = args[2];

            await _dbTasks.DbCliTask15(nodeName1, nodeName2);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null ref exception in trying to run Task 15, does db exist? Err message: {e}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"An argument error occurred: {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask16(string[] args)
    {
        try
        {
            if (args == null || args.Length != 3)
                throw new ArgumentException("Invalid number of arguments. Expected 3 arguments.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            string nodeName = args[1];
            if (!int.TryParse(args[2], out var depth))
                throw new ArgumentException("Invalid argument at position 2. Expected an integer.");

            await _dbTasks.DbCliTask16(nodeName, depth);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 16, does correct DB exist? Error message: {e}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"An argument error occurred: {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask17(string[] args)
    {
        try
        {
            if (args == null || args.Length != 3)
                throw new ArgumentException("Invalid number of arguments. Expected 3 arguments.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            string nodeName1 = args[1];
            string nodeName2 = args[2];

            await _dbTasks.DbCliTask17(nodeName1, nodeName2);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null ref exception in trying to run Task 17, does db exist? Err message: {e}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"An argument error occurred: {e}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task PrepareForTask18(string[] args)
    {
        try
        {
            if (args == null || args.Length != 4)
                throw new ArgumentException("Invalid number of arguments. Expected 4 arguments.");

            _prepareConnection();
            if (_dbTasks is null)
                throw new NullReferenceException("DbTasks is null, cannot proceed.");

            string nodeName1 = args[1];
            string nodeName2 = args[2];
            if (!int.TryParse(args[3], out var depth))
                throw new ArgumentException("Invalid argument at position 2. Expected an integer.");

            await _dbTasks.DbCliTask18(nodeName1, nodeName2, depth);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception in trying to run Task 18, does correct DB exist? Error message: {e}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"An argument error occurred: {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e}");
        }
    }
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