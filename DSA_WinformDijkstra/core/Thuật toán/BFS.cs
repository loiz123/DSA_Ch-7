using System;

public class BFSPath
{
    public static MyList<string> FindPath(Graph graph, string start, string end)
    {
        var visited = new MyDictionary<string, bool>();
        var parent = new MyDictionary<string, string>(); // để reconstruct path
        var queue = new MyList<string>();

        // Khởi tạo visited
        foreach (var node in graph.GetAllNodes())
        {
            visited[node.Name] = false;
            parent[node.Name] = null;
        }

        // Bắt đầu BFS
        queue.Add(start);
        visited[start] = true;

        while (queue.Count > 0)
        {
            string current = queue[0];
            queue.RemoveAt(0);

            if (current == end)
                break; // đã tìm thấy đích

            foreach (var obj in graph.GetNeighbors(current))
            {
                var (neighbor, weight) = ((string, double))obj;

                if (!visited[neighbor])
                {
                    visited[neighbor] = true;
                    parent[neighbor] = current;
                    queue.Add(neighbor);
                }
            }
        }

        // Nếu không có đường → trả về rỗng
        if (!visited[end])
            return new MyList<string>();

        // reconstruct path: end → start
        var path = new MyList<string>();
        string nodePath = end;
        while (nodePath != null)
        {
            path.Add(nodePath);
            nodePath = parent[nodePath];
        }

        // đảo lại vì đang từ end về start
        path.Reverse();

        return path;
    }
}
