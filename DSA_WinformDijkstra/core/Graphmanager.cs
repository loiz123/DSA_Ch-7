using System;
using System.Diagnostics;

public class GraphManager
{
    public Graph Graph { get; private set; } = new Graph();

    public void AddNode(string name, double x, double y)
        => Graph.AddNode(name, x, y);

    public void AddEdge(string n1, string n2)
        => Graph.AddEdge(n1, n2);

    // ---------------------------------------------------------
    // TÌM ĐƯỜNG ĐI NGẮN NHẤT + ĐO THỜI GIAN
    // ---------------------------------------------------------
    public (MyList<string> path, double distance, long milliseconds)
        GetShortest(string start, string end, string algorithm)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        MyList<string> path = null;
        double distance = 0;

        switch (algorithm.ToLower())
        {
            // ---------------- BFS ----------------
            case "bfs":
                path = BFSPath.FindPath(Graph, start, end);
                distance = Graph.CalculateDistance(path);
                break;

            // ---------------- DFS ----------------
            case "dfs":
                path = DFSPath.FindPath(Graph, start, end);
                distance = Graph.CalculateDistance(path);
                break;

            // ---------------- DIJKSTRA ----------------
            case "dijkstra":
                {
                    IShortestPathAlgorithm algo = new DijkstraAlgorithm();
                    var r = algo.FindPath(Graph, start, end);
                    path = r.path;
                    distance = r.distance;
                    break;
                }

            // ---------------- A* ----------------
            case "a*":
                {
                    IShortestPathAlgorithm algo = new AStarAlgorithm();
                    var r = algo.FindPath(Graph, start, end);
                    path = r.path;
                    distance = r.distance;
                    break;
                }

            // ---------------- BELLMAN–FORD ----------------
            case "bellman-ford":
                {
                    IShortestPathAlgorithm algo = new BellmanFordAlgorithm();
                    var r = algo.FindPath(Graph, start, end);
                    path = r.path;
                    distance = r.distance;
                    break;
                }

            // ---------------- BIDIRECTIONAL DIJKSTRA ----------------
            case "bidirectional dijkstra":
                {
                    IShortestPathAlgorithm algo = new BidirectionalDijkstraAlgorithm();
                    var r = algo.FindPath(Graph, start, end);
                    path = r.path;
                    distance = r.distance;
                    break;
                }
            case "floyd-warshall":
                {
                    IShortestPathAlgorithm algo = new FloydWarshallAlgorithm();
                    var r = algo.FindPath(Graph, start, end);
                    path = r.path;
                    distance = r.distance;
                    break;
                }

            default:
                throw new ArgumentException("Thuật toán không hợp lệ");
        }

        sw.Stop();
        return (path, distance, sw.ElapsedMilliseconds);
    }
}
