using System;
using DbcliArangoLoader;
using ArangoDBNetStandard;
using ArangoDBNetStandard.AuthApi;
using DbcliModels;
using DbcliProject;
using DbcliModels.TaskModels;
using Newtonsoft.Json;

try
{
    var root = Directory.GetCurrentDirectory();

    var json = File.ReadAllText("./dbcli_config.json");
    ConfigParameters parameters = JsonConvert.DeserializeObject<ConfigParameters>(json) ??
                                  throw new ApplicationException("Without JSON cannot procceed");
    
    Enum.TryParse(args[0], out TasksEnum taskType);
    var commandManager = new CommandManager(parameters);
    
    switch (taskType)
    {
        case TasksEnum.Task0:
            string task = args[1];
            if (task == "fix")
            {
                await commandManager.FixCsv(args);
            }
            else if (task == "load")
            {
                await commandManager.LoadToArangoDb(args);
            }
            else
            {
                throw new ArgumentException("Invalid number of arguments");
            }
            break;
        case TasksEnum.Task1:
            await commandManager.PrepareForTask1(args);
            break;
        case TasksEnum.Task2:
            await commandManager.PrepareForTask2(args);
            break;
        case TasksEnum.Task3:
            await commandManager.PrepareForTask3(args);
            break;
        case TasksEnum.Task4:
            await commandManager.PrepareForTask4(args);
            break;
        case TasksEnum.Task5:
            await commandManager.PrepareForTask5(args);
            break;
        case TasksEnum.Task6:
            await commandManager.PrepareForTask6(args);
            break;
        case TasksEnum.Task7:
            await commandManager.PrepareForTask7(args);
            break;
        case TasksEnum.Task8:
            await commandManager.PrepareForTask8(args);
            break;
        case TasksEnum.Task9:
            await commandManager.PrepareForTask9(args);
            break;
        case TasksEnum.Task10:
            await commandManager.PrepareForTask10(args);
            break;
        case TasksEnum.Task11:
            await commandManager.PrepareForTask11(args);
            break;
        case TasksEnum.Task12:
            await commandManager.PrepareForTask12(args);
            break;
        case TasksEnum.Task13:
            await commandManager.PrepareForTask13(args);
            break;
        case TasksEnum.Task14:
            await commandManager.PrepareForTask14(args);
            break;
        case TasksEnum.Task15:
            await commandManager.PrepareForTask15(args);
            break;
        case TasksEnum.Task16:
            await commandManager.PrepareForTask16(args);
            break;
        case TasksEnum.Task17:
            await commandManager.PrepareForTask17(args);
            break;
        case TasksEnum.Task18:
            await commandManager.PrepareForTask18(args);
            break;
        default:
            throw new ArgumentOutOfRangeException();
    }
}
catch (Exception e)
{
    Console.WriteLine($"An error occurred: {e.Message}");
}