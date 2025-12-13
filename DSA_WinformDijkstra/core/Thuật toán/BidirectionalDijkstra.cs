using System;

public class BidirectionalDijkstraAlgorithm : IShortestPathAlgorithm
{
    public (MyList<string> path, double distance)
        FindPath(Graph graph, string start, string end)
    {
        var distF = new MyDictionary<string, double>();
        var distB = new MyDictionary<string, double>();

        var prevF = new MyDictionary<string, string>();
        var prevB = new MyDictionary<string, string>();

        var pqF = new MyPriorityQueue<(double, string)>();
        var pqB = new MyPriorityQueue<(double, string)>();

        foreach (var v in graph.Nodes.Keys)
        {
            distF[v] = double.PositiveInfinity;
            distB[v] = double.PositiveInfinity;
            prevF[v] = null;
            prevB[v] = null;
        }

        distF[start] = 0;
        distB[end] = 0;

        pqF.Enqueue((0, start), 0);
        pqB.Enqueue((0, end), 0);

        string meeting = null;
        double best = double.PositiveInfinity;

        while (pqF.Count > 0 && pqB.Count > 0)
        {
            // ---- Forward ----
            var (df, u) = pqF.Dequeue();
            if (df <= best)
            {
                foreach (var (v, w) in graph.AdjList[u])
                {
                    double alt = df + w;
                    if (alt < distF[v])
                    {
                        distF[v] = alt;
                        prevF[v] = u;
                        pqF.Enqueue((alt, v), alt);

                        if (distB[v] < double.PositiveInfinity &&
                            alt + distB[v] < best)
                        {
                            best = alt + distB[v];
                            meeting = v;
                        }
                    }
                }
            }

            // ---- Backward ----
            var (db, x) = pqB.Dequeue();
            if (db <= best)
            {
                foreach (var (v, w) in graph.AdjList[x])
                {
                    double alt = db + w;
                    if (alt < distB[v])
                    {
                        distB[v] = alt;
                        prevB[v] = x;
                        pqB.Enqueue((alt, v), alt);

                        if (distF[v] < double.PositiveInfinity &&
                            alt + distF[v] < best)
                        {
                            best = alt + distF[v];
                            meeting = v;
                        }
                    }
                }
            }
        }

        if (meeting == null)
            return (new MyList<string>(), 0);

        return BuildPath(prevF, prevB, start, end, meeting, best);
    }

    // --------------------------------------------------
    // GHÉP ĐƯỜNG ĐI
    // --------------------------------------------------
    private (MyList<string>, double) BuildPath(
        MyDictionary<string, string> prevF,
        MyDictionary<string, string> prevB,
        string start,
        string end,
        string meet,
        double dist)
    {
        var path = new MyList<string>();

        // start -> meet
        string cur = meet;
        while (cur != null)
        {
            path.Add(cur);
            cur = prevF[cur];
        }
        path.Reverse();

        // meet -> end
        cur = prevB[meet];
        while (cur != null)
        {
            path.Add(cur);
            cur = prevB[cur];
        }

        return (path, dist);
    }
}
