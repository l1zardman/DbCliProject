using ArangoDBNetStandard;
using ArangoDBNetStandard.CursorApi.Models;
using Newtonsoft.Json;

namespace DbcliModels.TaskModels;

public class DbTasks
{
    private ArangoDBClient _client;
    private readonly ConfigParameters _parameters;

    public DbTasks(ConfigParameters parameters, ArangoDBClient client)
    {
        _client = client;
        _parameters = parameters;
    }

    public async Task<List<VertexModel>> DbCliTask1(string nodeName)
    {
        var query = $@"
            FOR startVertex IN {_parameters.NodeCollectionName}
            FILTER startVertex.name == ""{nodeName}""
              
            FOR v IN 1..1 OUTBOUND startVertex._id {_parameters.EdgeCollectionName}
                RETURN DISTINCT v
            ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        List<VertexModel> queryResult = new();


        foreach (var record in result.Result)
        {
            var vertex = JsonConvert.DeserializeObject<VertexModel>(record.ToString());
            Console.WriteLine($"Descendant Node: {vertex.Name}");
            queryResult.Add(vertex);
        }

        return queryResult;
    }

    public async Task<int> DbCliTask2(string nodeName)
    {
        var res = await DbCliTask1(nodeName);
        return res.Count;
    }

    public async Task<List<VertexModel>> DbCliTask3(string nodeName)
    {
        var query = $@"
            FOR startVertex IN {_parameters.NodeCollectionName}
            FILTER startVertex.name == ""{nodeName}""
          
            /* Traverse exactly 2 levels deep: from children (depth 1) to grandchildren (depth 2). */
            FOR v IN 2..2 OUTBOUND startVertex._id {_parameters.EdgeCollectionName}
                RETURN DISTINCT v
            ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        List<VertexModel> queryResult = new();


        foreach (var record in result.Result)
        {
            var vertex = JsonConvert.DeserializeObject<VertexModel>(record.ToString());
            Console.WriteLine($"Grandchild Node: {vertex.Name}");
            queryResult.Add(vertex);
        }

        return queryResult;
    }

    public async Task<List<VertexModel>> DbCliTask4(string nodeName)
    {
        var query = $@"
                FOR startVertex IN {_parameters.NodeCollectionName}
                    FILTER startVertex.name == ""{nodeName}""

                    /* Traverse exactly depth 1 inbound: find direct parents */
                    FOR v IN 1..1 INBOUND startVertex._id {_parameters.EdgeCollectionName}
                        RETURN DISTINCT v
            ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        List<VertexModel> queryResult = new();

        foreach (var record in result.Result)
        {
            var vertex = JsonConvert.DeserializeObject<VertexModel>(record.ToString());
            Console.WriteLine($"Parent Node: {vertex.Name}");
            queryResult.Add(vertex);
        }

        return queryResult;
    }

    public async Task<int> DbCliTask5(string nodeName)
    {
        var queryResult = await DbCliTask4(nodeName);

        return queryResult.Count;
    }

    public async Task<List<VertexModel>> DbCliTask6(string nodeName)
    {
        var query = $@"
            FOR startVertex IN {_parameters.NodeCollectionName}
            FILTER startVertex.name == ""{nodeName}""
          
            FOR v IN 2..2 INBOUND startVertex._id {_parameters.EdgeCollectionName}
                RETURN DISTINCT v
            ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        List<VertexModel> queryResult = new();


        foreach (var record in result.Result)
        {
            var vertex = JsonConvert.DeserializeObject<VertexModel>(record.ToString());
            Console.WriteLine($"Grandchild Node: {vertex.Name}");
            queryResult.Add(vertex);
        }

        return queryResult;
    }

    public async Task<List<VertexModel>> DbCliTask7()
    {
        var query = $@"
                RETURN LENGTH(
                    FOR doc IN {_parameters.NodeCollectionName}
                        COLLECT uniqueName = doc.name
                        RETURN 1
                )
            ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        List<VertexModel> queryResult = new();

        foreach (var record in result.Result)
        {
            var vertex = JsonConvert.DeserializeObject<VertexModel>(record.ToString());
            Console.WriteLine($"Parent Node: {vertex.Name}");
            queryResult.Add(vertex);
        }

        return queryResult;
    }

    public async Task<List<VertexModel>> DbCliTask8()
    {
        var query = $@"
                LET childIds = (
                    FOR e IN TaxonomyEdgeCollection
                        RETURN DISTINCT e._to
                )

                FOR doc IN TaxonomyVertexCollection
                    FILTER doc._id NOT IN childIds
                    RETURN doc
            ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        List<VertexModel> queryResult = new();

        foreach (var record in result.Result)
        {
            var vertex = JsonConvert.DeserializeObject<VertexModel>(record.ToString());
            Console.WriteLine($"Root Node: {vertex.Name}");
            queryResult.Add(vertex);
        }

        return queryResult;
    }

    public async Task<int> DbCliTask9()
    {
        var queryResult = await DbCliTask8();

        return queryResult.Count;
    }

    public async Task<List<VertexCountModel>> DbCliTask10(int depth)
    {
        var query = $@"
                FOR node IN {_parameters.NodeCollectionName}
                    LET childrenCount = COUNT(
                        FOR child IN 1..1 OUTBOUND node {_parameters.EdgeCollectionName}
                            RETURN child
                )
                SORT childrenCount DESC
                LIMIT {depth}
                RETURN {{
                    node: node,
                    count: childrenCount
                }}
            ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        List<VertexCountModel> queryResult = new();

        foreach (var record in result.Result)
        {
            VertexCountModel vertex = JsonConvert.DeserializeObject<VertexCountModel>(record.ToString());
            Console.WriteLine($"Node: {vertex.Vertex.Name}, Count: {vertex.Count}");
            queryResult.Add(vertex);
        }

        return queryResult;
    }

    public async Task<List<VertexCountModel>> DbCliTask11(int depth)
    {
        var query = $@"
                FOR node IN {_parameters.NodeCollectionName}
                    LET childrenCount = COUNT(
                        FOR child IN 1..1 OUTBOUND node {_parameters.EdgeCollectionName}
                            RETURN child
                    )
                    FILTER childrenCount > 0
                    SORT childrenCount ASC
                    LIMIT {depth}

                    RETURN {{
                        node: node,
                        count: childrenCount
                    }}
            ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        List<VertexCountModel> queryResult = new();

        foreach (var record in result.Result)
        {
            VertexCountModel vertex = JsonConvert.DeserializeObject<VertexCountModel>(record.ToString());
            Console.WriteLine($"Node: {vertex.Vertex.Name}, Count: {vertex.Count}");
            queryResult.Add(vertex);
        }

        return queryResult;
    }

    public async Task DbCliTask12(string nodeName, string newNodeName)
    {
        var query = $@"
                FOR doc IN {_parameters.NodeCollectionName}
                    FILTER doc.name == ""{nodeName}""
                    UPDATE doc
                        WITH {{ name: ""{newNodeName}"" }}
                        IN {_parameters.NodeCollectionName}
            ";

        Console.WriteLine($"Query: {query}");
        await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });
    }

    public async Task DbCliTask13(string nodeName, int newPopularityScore)
    {
        var query = $@"
                FOR doc IN {_parameters.NodeCollectionName}
                    FILTER doc.name == ""{nodeName}""
                    UPDATE doc 
                        WITH {{ popularity_score: {newPopularityScore} }} 
                        IN {_parameters.NodeCollectionName}
            ";

        Console.WriteLine($"Query: {query}");
        await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });
    }

    public async Task<VertexEdgeModel> DbCliTask14(string nodeName1, string nodeName2)
    {
        var query = $@"
                LET startNode = FIRST(
                    FOR v IN {_parameters.NodeCollectionName}
                        FILTER v.name == ""{nodeName1}""
                        RETURN v
                )

                LET endNode = FIRST(
                    FOR v IN {_parameters.NodeCollectionName}
                        FILTER v.name == ""{nodeName2}""
                        RETURN v
                )

                FOR path IN OUTBOUND ALL_SHORTEST_PATHS
                    startNode._id
                    TO endNode._id
                    GRAPH ""{_parameters.GraphName}""
                    RETURN path
            ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        VertexEdgeModel? queryResult = null;

        foreach (var record in result.Result)
        {
            queryResult = JsonConvert.DeserializeObject<VertexEdgeModel>(record.ToString());
        }

        foreach (var v in queryResult!.Vertices)
        {
            Console.WriteLine($"Nodes: {v.Name}");
        }

        foreach (var e in queryResult!.Edges)
        {
            Console.WriteLine($"Edges: {e.ToString()}");
        }

        return queryResult;
    }

    public async Task<int> DbCliTask15(string nodeName1, string nodeName2)
    {
        var res = await DbCliTask14(nodeName1, nodeName2);
        return res.Vertices.Count;
    }

    public async Task<int> DbCliTask16(string nodeName, int radius)
    {
        var query = $@"
                FOR node IN TaxonomyVertexCollection
                    FILTER node.name == ""{nodeName}""
                    LET parentPopularity = SUM(
                        FOR parent IN 1..{radius} INBOUND node {_parameters.EdgeCollectionName}
                            RETURN parent.popularity_score
                    )
                    LET childrenPopularity = SUM(
                        FOR child IN 1..{radius} OUTBOUND node {_parameters.EdgeCollectionName}
                            RETURN child.popularity_score
                    )
                    LET totalPopularity = node.popularity_score + parentPopularity + childrenPopularity
                    RETURN {{
                        node: node,
                        count: totalPopularity
                    }}
            ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        VertexCountModel? queryResult = null;

        foreach (var record in result.Result)
        {
            queryResult = JsonConvert.DeserializeObject<VertexCountModel>(record.ToString());
            Console.WriteLine($"Node: {queryResult.Vertex.Name}, Count: {queryResult.Count}");
        }

        return queryResult?.Count ?? -42;
    }

    public async Task<int> DbCliTask17(string nodeName1, string nodeName2)
    {
        var query = $@"
                        LET startNode = FIRST(
                            FOR v IN {_parameters.NodeCollectionName}
                                FILTER v.name == ""{nodeName1}""
                                RETURN v
                        )

                        LET endNode = FIRST(
                            FOR v IN {_parameters.NodeCollectionName}
                                FILTER v.name == ""{nodeName2}""
                                RETURN v
                        )

                        LET shortestPath = (
                            startNode != null AND endNode != null
                            ? FIRST(
                                FOR path IN OUTBOUND SHORTEST_PATH
                                    startNode._id
                                    TO endNode._id
                                    GRAPH ""{_parameters.GraphName}""
                                    RETURN path
                            )
                            : null
                        )

                        LET popularitySum = (
                            shortestPath != null AND LENGTH(shortestPath.vertices) > 0
                            ? SUM(
                                FOR vertex IN shortestPath.vertices
                                    FILTER HAS(vertex, ""popularity_score"") AND vertex.popularity_score != null
                                    RETURN vertex.popularity_score
                            )
                            : null
                        )

                        RETURN shortestPath.popularity_score
            ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        int queryResult = -42;

        foreach (var record in result.Result)
        {
            queryResult = JsonConvert.DeserializeObject<int>(record.ToString());
        }

        return queryResult;
    }

    public async Task<Task18Model> DbCliTask18(string nodeName1, string nodeName2, int depth)
    {
        var query = $@"
                LET startNode = FIRST(
                    FOR v IN {_parameters.NodeCollectionName}
                        FILTER v.name == ""{nodeName1}""
                        RETURN v
                )

                LET endNode = FIRST(
                    FOR v IN {_parameters.NodeCollectionName}
                        FILTER v.name == ""{nodeName2}""
                        RETURN v
                )

                LET allPaths = (
                    startNode != null AND endNode != null
                    ? (
                        FOR path IN OUTBOUND K_SHORTEST_PATHS
                            startNode._id
                            TO endNode._id
                            GRAPH ""{_parameters.GraphName}""
                            LIMIT {depth}
                            RETURN path
                    )
                    : []
                )

                LET pathsWithPopularity = (
                    FOR path IN allPaths
                        LET popularitySum = SUM(
                            FOR vertex IN path.vertices
                                FILTER HAS(vertex, ""popularity_score"") AND vertex.popularity_score != null
                                RETURN vertex.popularity_score
                        )
                        RETURN {{ path, popularitySum }}
                )

                LET bestPath = FIRST(
                    FOR p IN pathsWithPopularity
                        SORT p.popularitySum DESC
                        RETURN p
                )

                RETURN {{
                    bestPath: bestPath != null ? bestPath.path : null,
                    maxPopularity: bestPath != null ? bestPath.popularitySum : null
                }}
         ";

        Console.WriteLine($"Query: {query}");
        var result = await _client.Cursor.PostCursorAsync<dynamic>(new PostCursorBody
        {
            Query = query
        });

        Task18Model? queryResult = null;

        foreach (var record in result.Result)
        {
            queryResult = JsonConvert.DeserializeObject<Task18Model>(record.ToString());
        }

        return queryResult;
    }
}