using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

public class WebApiService
{
    private GraphManager manager;
    private Random rand = new Random();

    public WebApiService()
    {
        manager = new GraphManager();
        InitRandomGraph(10, 15); 
    }

    // -------------------------------------------------------
    // KHỞI TẠO ĐỒ THỊ NGẪU NHIÊN
    // -------------------------------------------------------
    private void InitRandomGraph(int nodeCount, int edgeCount)
    {
        // Tạo node
        for (int i = 0; i < nodeCount; i++)
        {
            string name = ((char)('A' + i)).ToString();
            double x = rand.Next(50, 600);
            double y = rand.Next(50, 400);
            manager.AddNode(name, x, y);
        }

        // Tạo edge ngẫu nhiên
        var nodeNames = manager.Graph.GetAllNodes()
                                      .Select(n => n.Name)
                                      .ToList();

        for (int i = 0; i < edgeCount; i++)
        {
            string a = nodeNames[rand.Next(nodeNames.Count)];
            string b = nodeNames[rand.Next(nodeNames.Count)];

            if (a != b)
            {
                try
                {
                    manager.AddEdge(a, b);
                }
                catch { }
            }
        }
    }

    // -------------------------------------------------------
    // START API
    // -------------------------------------------------------
    public async void Start()
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:5000/");
        listener.Start();

        Console.WriteLine("API chạy tại http://localhost:5000/");

        while (true)
        {
            var context = await listener.GetContextAsync();
            var req = context.Request;
            var res = context.Response;

            try
            {
                switch (req.Url.AbsolutePath.ToLower())
                {
                    // ---------------- NODES ----------------
                    case "/nodes":
                        SendJson(res, manager.Graph.GetAllNodes()
                            .Select(n => new
                            {
                                n.Name,
                                n.X,
                                n.Y
                            }).ToList());
                        break;

                    // ---------------- EDGES ----------------
                    case "/edges":
                        SendJson(res, manager.Graph.GetEdges()
                            .Select(e => new
                            {
                                From = e.Node1.Name,
                                To = e.Node2.Name,
                                e.Weight
                            }).ToList());
                        break;

                    // ---------------- PATH ----------------
                    case "/path":
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
                            pathString = result.path == null ? "" :
                                         string.Join(" -> ", result.path),
                            distance = result.distance,
                            milliseconds = result.milliseconds
                        });
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
    }

    // -------------------------------------------------------
    // GỬI JSON
    // -------------------------------------------------------
    private void SendJson(HttpListenerResponse response, object data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        byte[] buffer = Encoding.UTF8.GetBytes(json);

        response.ContentType = "application/json";
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
    }
}

