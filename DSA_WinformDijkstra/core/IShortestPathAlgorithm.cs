public interface IShortestPathAlgorithm
{
    (MyList<string> path, double distance)
        FindPath(Graph graph, string start, string end);
}