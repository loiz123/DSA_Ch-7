public class AStarAlgorithm : IShortestPathAlgorithm
{
    public (MyList<string>, double) FindPath(Graph g, string start, string end)
    {
        var gScore = new MyDictionary<string, double>();
        var prev = new MyDictionary<string, string>();
        var pq = new MyPriorityQueue<string>();

        foreach (var k in g.Nodes.Keys)
        {
            gScore[k] = double.PositiveInfinity;
            prev[k] = null;
        }

        gScore[start] = 0;
        pq.Enqueue(start, HeuristicHelper.Euclid(g.Nodes[start], g.Nodes[end]));

        while (pq.Count > 0)
        {
            string u = pq.Dequeue();
            if (u == end) break;

            foreach (var (v, w) in g.AdjList[u])
            {
                double tentative = gScore[u] + w;
                if (tentative < gScore[v])
                {
                    gScore[v] = tentative;
                    prev[v] = u;
                    double f = tentative + HeuristicHelper.Euclid(g.Nodes[v], g.Nodes[end]);
                    pq.Enqueue(v, f);
                }
            }
        }
    


        return BuildPath(prev, start, end, gScore[end]);
    }

    private (MyList<string>, double) BuildPath(
        MyDictionary<string, string> prev,
        string start, string end, double dist)
    {
        var path = new MyList<string>();
        string cur = end;
        if (dist == double.PositiveInfinity)
            return (path, 0);

        while (cur != null)
        {
            path.Add(cur);
            cur = prev[cur];
        }
        path.Reverse();
        return (path, dist);
    }
}
