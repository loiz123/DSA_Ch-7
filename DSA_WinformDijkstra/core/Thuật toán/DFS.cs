using System;

public class DFSPath
{
    public static MyList<string> FindPath(Graph graph, string start, string end)
    {
        var visited = new MyDictionary<string, bool>();
        var path = new MyList<string>();
        bool found = false;

        foreach (var node in graph.GetAllNodes())
        {
            visited[node.Name] = false;
        }

        DFS(graph, start, end, visited, path, ref found);

        return path;
    }

    private static void DFS(Graph graph, string current, string end,
                            MyDictionary<string, bool> visited,
                            MyList<string> path,
                            ref bool found)
    {
        if (found) return;

        visited[current] = true;
        path.Add(current);

        if (current == end)
        {
            found = true;
            return;
        }

        foreach (var obj in graph.GetNeighbors(current))
        {
            var (neighbor, weight) = ((string, double))obj;

            if (!visited[neighbor])
            {
                DFS(graph, neighbor, end, visited, path, ref found);
                if (found) return;
            }
        }

        // backtrack
        path.RemoveAt(path.Count - 1);
    }
}
