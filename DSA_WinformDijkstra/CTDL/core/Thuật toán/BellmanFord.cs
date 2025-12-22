using System;

public class BellmanFordAlgorithm : IShortestPathAlgorithm
{
    public (MyList<string> path, double distance)
        FindPath(Graph graph, string start, string end)
    {
        var dist = new MyDictionary<string, double>();
        var prev = new MyDictionary<string, string>();

        // 1. Khởi tạo
        foreach (var v in graph.Nodes.Keys)
        {
            dist[v] = double.PositiveInfinity;
            prev[v] = null;
        }

        dist[start] = 0;

        int V = graph.Nodes.Count;

        // 2. Relax |V|-1 lần
        for (int i = 0; i < V - 1; i++)
        {
            foreach (var u in graph.Nodes.Keys)
            {
                foreach (var (v, w) in graph.AdjList[u])
                {
                    if (dist[u] + w < dist[v])
                    {
                        dist[v] = dist[u] + w;
                        prev[v] = u;
                    }
                }
            }
        }

        // 3. Dựng đường đi
        return BuildPath(prev, start, end, dist[end]);
    }

    private (MyList<string>, double) BuildPath(
        MyDictionary<string, string> prev,
        string start,
        string end,
        double dist)
    {
        var path = new MyList<string>();

        if (dist == double.PositiveInfinity)
            return (path, 0);

        string cur = end;
        while (cur != null)
        {
            path.Add(cur);
            cur = prev[cur];
        }

        path.Reverse();
        return (path, dist);
    }
}
