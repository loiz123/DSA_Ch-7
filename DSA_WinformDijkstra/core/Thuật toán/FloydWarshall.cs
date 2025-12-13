using System;

public class FloydWarshallAlgorithm : IShortestPathAlgorithm
{
    public (MyList<string> path, double distance)
        FindPath(Graph graph, string start, string end)
    {
        var nodes = graph.GetAllNodes();
        int n = nodes.Count;

        // Map node name -> index
        var index = new MyDictionary<string, int>();
        for (int i = 0; i < n; i++)
            index[nodes[i].Name] = i;

        double[,] dist = new double[n, n];
        int[,] next = new int[n, n];

        // Khởi tạo
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                dist[i, j] = (i == j) ? 0 : double.PositiveInfinity;
                next[i, j] = -1;
            }
        }

        // Gán trọng số cạnh
        foreach (var edge in graph.GetEdges())
        {
            int u = index[edge.Node1.Name];
            int v = index[edge.Node2.Name];

            dist[u, v] = edge.Weight;
            dist[v, u] = edge.Weight;

            next[u, v] = v;
            next[v, u] = u;
        }

        // ---------------------------
        // FLOYD–WARSHALL CORE
        // ---------------------------
        for (int k = 0; k < n; k++)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (dist[i, k] + dist[k, j] < dist[i, j])
                    {
                        dist[i, j] = dist[i, k] + dist[k, j];
                        next[i, j] = next[i, k];
                    }
                }
            }
        }

        int s = index[start];
        int t = index[end];

        if (double.IsInfinity(dist[s, t]))
            return (new MyList<string>(), 0);

        // ---------------------------
        // TRUY VẾT ĐƯỜNG ĐI
        // ---------------------------
        var path = new MyList<string>();
        int current = s;

        path.Add(nodes[current].Name);
        while (current != t)
        {
            current = next[current, t];
            path.Add(nodes[current].Name);
        }

        return (path, dist[s, t]);
    }
}
