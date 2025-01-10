using System;
using ArangoDBNetStandard;
using ArangoDBNetStandard.AuthApi;
using DbcliCoreUtility;
using Newtonsoft.Json;

// namespace DatabasesProjectFinal;

// class Program {
//     static void Main(string[] args) {
//         if (args.Length != 2) {
//             Console.WriteLine("Usage: dbcli <int_arg> <string_arg>");
//             return;
//         }
//
//         if (!int.TryParse(args[0], out int intArg)) {
//             Console.WriteLine("Error: The first argument must be an integer.");
//             return;
//         }
//
//         string stringArg = args[1];
//
//         Console.WriteLine($"Integer argument: {intArg}");
//         Console.WriteLine($"String argument: {stringArg}");
//     }
// }


string path_to_config = @"./dbcli_config.json";
string json = "";

try
{
    json = File.ReadAllText(path_to_config);
    Console.WriteLine(json);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
DbParameters parameters;
try
{
    parameters = JsonConvert.DeserializeObject<DbParameters>(json);
    Console.WriteLine($"Db uri: {parameters?.DbUri}");
    Console.WriteLine($"Db name: {parameters?.DbName}");
    Console.WriteLine($"Db user: {parameters?.Username}");
    Console.WriteLine($"Db password: {parameters?.Password}");
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

var transport = DbConnector.GetApiTransport(parameters!);
var dbClient = new ArangoDBClient(transport);

var dbTasksInstance = new DbTasks(parameters!, dbClient);

// await dbTasksInstance.DbCliTask1("1880s_films");
//
// var c1 = await dbTasksInstance.DbCliTask2("1880s_films");
// Console.WriteLine($"Number of descendants: {c1}");
//
// await dbTasksInstance.DbCliTask3("1880s_films");
//
// var parents = await dbTasksInstance.DbCliTask4("1880s_films");
//
// Console.WriteLine($"Number of parents: {parents.Count}");
//
// var grandParents = await dbTasksInstance.DbCliTask6("1880s_films");
//
// Console.WriteLine($"Number of grand parents: {grandParents.Count}");

// var lst = await dbTasksInstance.DbCliTask8();
//
// Console.WriteLine($"{lst.Count}");

// await dbTasksInstance.DbCliTask11(2);

// await dbTasksInstance.DbCliTask12("1880s_films_42", "1880s_films");


// await dbTasksInstance.DbCliTask13("1880s_films", 30941);

// int num15 = await dbTasksInstance.DbCliTask15("Works_by_type_and_year", "1969_paintings");
// Console.WriteLine($"Number of nodes: {num15}");
// await dbTasksInstance.DbCliTask16("1880s_films", 8);

// int pop_score = await dbTasksInstance.DbCliTask17("Wikipedia_administration_by_MediaWiki_feature", "1880s_films");
//
// Console.WriteLine($"Popularity score: {pop_score}");

var obj = await dbTasksInstance.DbCliTask18("Wikipedia_administration_by_MediaWiki_feature", "1880s_films", 15);

Console.WriteLine($"Pop: {obj.MaxPopularity}");

Console.WriteLine($"Weight: {obj.BestPath.Weight}");

foreach (var vertex in obj.BestPath.Vertices)
{
    Console.WriteLine($"Vertex: {vertex}");
}