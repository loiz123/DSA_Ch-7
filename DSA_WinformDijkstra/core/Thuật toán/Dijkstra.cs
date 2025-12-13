public class DijkstraAlgorithm : IShortestPathAlgorithm
{
    public (MyList<string>, double) FindPath(Graph graph, string start, string end)
    {
        var dist = new MyDictionary<string, double>();
        var prev = new MyDictionary<string, string>();
        var pq = new MyPriorityQueue<(double, string)>();

        foreach (var k in graph.Nodes.Keys)
        {
            dist[k] = double.PositiveInfinity;
            prev[k] = null;
        }

        dist[start] = 0;
        pq.Enqueue((0, start), 0);

        while (pq.Count > 0)
        {
            var (d, u) = pq.Dequeue();
            if (u == end) break;

            foreach (var (v, w) in graph.AdjList[u])
            {
                double alt = d + w;
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                    pq.Enqueue((alt, v), alt);
                }
            }
        }

        return BuildPath(prev, start, end, dist[end]);
    }

    private (MyList<string>, double) BuildPath(
        MyDictionary<string, string> prev,
        string start, string end, double dist)
    {
        var path = new MyList<string>();
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
