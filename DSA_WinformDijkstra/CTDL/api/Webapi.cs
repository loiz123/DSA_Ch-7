using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class WebApiService
{
    private GraphManager manager;
    private Random rand = new Random();

    public WebApiService()
    {
        manager = new GraphManager();

        // ===== CHỌN 1 TRONG 2 =====
        //InitTestGraph();                // graph test BFS/DFS sai
         InitRandomGraph(200, 2);      // graph performance lớn
    }

    // =====================================================
    // GRAPH TEST: BFS/DFS SAI – DIJKSTRA/A* ĐÚNG
    // =====================================================
    private void InitTestGraph()
    {
        // ===== NODES =====
        manager.AddNode("A", 50, 200);
        manager.AddNode("B", 250, 50);
        manager.AddNode("C", 450, 200);
        manager.AddNode("J", 650, 200);

        manager.AddNode("D", 120, 230);
        manager.AddNode("E", 190, 230);
        manager.AddNode("F", 260, 230);
        manager.AddNode("G", 330, 230);
        manager.AddNode("H", 400, 230);
        manager.AddNode("I", 470, 230);

        // ===== EDGES =====
        // Đường ÍT cạnh nhưng DÀI (bẫy BFS/DFS)
        manager.AddEdge("A", "B");
        manager.AddEdge("B", "C");
        manager.AddEdge("C", "J");

        // Đường NHIỀU cạnh nhưng NGẮN (đúng)
        manager.AddEdge("A", "D");
        manager.AddEdge("D", "E");
        manager.AddEdge("E", "F");
        manager.AddEdge("F", "G");
        manager.AddEdge("G", "H");
        manager.AddEdge("H", "I");
        manager.AddEdge("I", "J");
    }

    // =====================================================
    // GRAPH RANDOM PERFORMANCE (AN TOÀN)
    // =====================================================
    private void InitRandomGraph(int nodeCount, int maxEdgesPerNode)
    {
        // ----- NODES -----
        for (int i = 0; i < nodeCount; i++)
        {
            string name = "N" + i;
            double x = rand.Next(50, 900);
            double y = rand.Next(50, 600);
            manager.AddNode(name, x, y);
        }

        var nodes = manager.Graph.GetAllNodes().Select(n => n.Name).ToList();

        // ----- EDGES -----
        foreach (var u in nodes)
        {
            int edges = rand.Next(1, maxEdgesPerNode + 1);
            for (int i = 0; i < edges; i++)
            {
                string v = nodes[rand.Next(nodes.Count)];
                if (u == v) continue;

                try
                {
                    manager.AddEdge(u, v);
                }
                catch { }
            }
        }
    }

    // =====================================================
    // START WEB API
    // =====================================================
    public async Task Start()
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:5000/");
        listener.Start();

        while (true)
        {
            var context = await listener.GetContextAsync();
            _ = Task.Run(() => HandleRequest(context));
        }
    }

    // =====================================================
    // HANDLE REQUEST
    // =====================================================
    private void HandleRequest(HttpListenerContext context)
    {
        var req = context.Request;
        var res = context.Response;

        try
        {
            switch (req.Url.AbsolutePath.ToLower())
            {
                case "/nodes":
                    SendJson(res, manager.Graph.GetAllNodes()
                        .Select(n => new
                        {
                            n.Name,
                            n.X,
                            n.Y
                        }).ToList());
                    break;

                case "/edges":
                    SendJson(res, manager.Graph.GetEdges()
                        .Select(e => new
                        {
                            From = e.Node1.Name,
                            To = e.Node2.Name,
                            Weight = Math.Round(e.Weight, 2)
                        }).ToList());
                    break;

                case "/path":
                    HandlePath(req, res);
                    break;

                default:
                    res.StatusCode = 404;
                    SendJson(res, new { error = "Endpoint không tồn tại" });
                    break;
            }
        }
        catch (Exception ex)
        {
            res.StatusCode = 500;
            SendJson(res, new { error = ex.Message });
        }
        finally
        {
            res.Close();
        }
    }

    // =====================================================
    // PATH API
    // =====================================================
    private void HandlePath(HttpListenerRequest req, HttpListenerResponse res)
    {
        string start = req.QueryString["start"];
        string end = req.QueryString["end"];
        string algo = req.QueryString["algo"];

        if (string.IsNullOrEmpty(start) ||
            string.IsNullOrEmpty(end) ||
            string.IsNullOrEmpty(algo))
            throw new Exception("Thiếu start, end hoặc algo");

        var result = manager.GetShortest(start, end, algo);

        SendJson(res, new
        {
            algorithm = algo,
            path = result.path?.ToArray(),
            pathString = result.path == null || result.path.Count == 0
                ? "Không có đường đi"
                : string.Join(" -> ", result.path),
            distance = double.IsPositiveInfinity(result.distance)
    ? "INF"
    : Math.Round(result.distance, 2).ToString(),
            milliseconds = result.milliseconds
        });
    }

    // =====================================================
    // SEND JSON
    // =====================================================
    private void SendJson(HttpListenerResponse response, object data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        byte[] buffer = Encoding.UTF8.GetBytes(json);

        response.ContentType = "application/json";
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
    }
}

