using System;

public class Graph
{
    private MyDictionary<string, Node> nodes = new MyDictionary<string, Node>();
    private MyDictionary<string, MyList<(string neighbor, double weight)>> adjList
        = new MyDictionary<string, MyList<(string neighbor, double weight)>>();

    public void AddNode(string name, double x, double y)
    {
        if (!nodes.ContainsKey(name))
        {
            Node node = new Node(name, x, y);
            nodes[name] = node;
            adjList[name] = new MyList<(string neighbor, double weight)>();
        }
        else throw new ArgumentException("Node đã tồn tại.");
    }

    public void AddEdge(string node1Name, string node2Name)
    {
        // 1. Kiểm tra sự tồn tại của cả hai Node
        if (nodes.ContainsKey(node1Name) && nodes.ContainsKey(node2Name))
        {
            Node n1 = nodes[node1Name];
            Node n2 = nodes[node2Name];

            // 2. Tạo Edge và tính trọng số
            Edge edge = new Edge(n1, n2);
            double weight = edge.Weight;

            // 3. Thêm vào danh sách kề (Đồ thị vô hướng)
            adjList[node1Name].Add((node2Name, weight));
            adjList[node2Name].Add((node1Name, weight));
        }
        else
        {
            // Báo lỗi rõ ràng hơn
            string missingNode = "";
            if (!nodes.ContainsKey(node1Name)) missingNode += node1Name + " ";
            if (!nodes.ContainsKey(node2Name)) missingNode += node2Name;

            throw new ArgumentException($"Lỗi: Node không tồn tại: {missingNode.Trim()}.");
        }
    }

    public MyList<Node> GetAllNodes()
    {
        var list = new MyList<Node>();
        foreach (var key in nodes.Keys)
            list.Add(nodes[key]);

        return list;
    }

    public MyList<(string neighbor, double weight)> GetNeighbors(string nodeName)
    {
        if (adjList.ContainsKey(nodeName))
        {
            return adjList[nodeName];
        }
        else
        {
            // Trả về lỗi khi Node không tồn tại
            throw new ArgumentException($"Node '{nodeName}' không tồn tại.");
        }
    }

    public double CalculateDistance(MyList<string> path)
    {
        double total = 0;

        for (int i = 0; i < path.Count - 1; i++)
        {
            string u = path[i];
            string v = path[i + 1];

            bool edgeFound = false;

            // Lấy danh sách kề của u
            // Sử dụng try/catch để bắt lỗi nếu node không tồn tại
            try
            {
                var neighbors = GetNeighbors(u);

                foreach (var obj in neighbors)
                {
                    var (nei, weight) = obj; // Thay vì cast phức tạp

                    if (nei == v)
                    {
                        total += weight;
                        edgeFound = true;
                        break;
                    }
                }
            }
            catch (ArgumentException)
            {
                // Nếu node không tồn tại trên đường đi, trả về lỗi hoặc Infinity
                return double.PositiveInfinity;
            }

            if (!edgeFound)
            {
                // Nếu cạnh không tồn tại, đường đi không hợp lệ
                return double.PositiveInfinity;
            }
        }

        return total;
    }

    // (Giữ nguyên phương thức Dijkstra của bạn, nó có vẻ đúng về mặt logic)
    

    public MyList<Edge> GetEdges()
    {
        var list = new MyList<Edge>();

        foreach (var n1 in nodes.Keys)
        {
            // Kiểm tra xem node có trong adjList không trước khi truy cập
            if (!adjList.ContainsKey(n1)) continue;

            var neighbors = adjList[n1];

            for (int i = 0; i < neighbors.Count; i++)
            {
                string n2 = neighbors[i].neighbor;

                // Chỉ thêm cạnh một lần (để tránh lặp lại Edge(A, B) và Edge(B, A))
                if (string.Compare(n1, n2) < 0)
                    list.Add(new Edge(nodes[n1], nodes[n2]));
            }
        }

        return list;
    }
   
    public MyDictionary<string, Node> Nodes
    {
        get { return nodes; }
    }

    public MyDictionary<string, MyList<(string, double)>> AdjList
    {
        get { return adjList; }
    }
}