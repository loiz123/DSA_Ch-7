using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;
using static TimeUtils;

public partial class Form1 : Form
{
    private Panel panelMap;
    private ComboBox cbStart;
    private ComboBox cbEnd;
    private Button btnLoad;
    private Button btnFind;
    private Label lblResult;

    private List<ApiNode> nodes = new List<ApiNode>();
    private List<ApiEdge> edges = new List<ApiEdge>();

    float scale = 1;
    float minX = 0, minY = 0;
    private Graph graph = new Graph();

    public Form1()
    {
        InitializeComponent();
        this.panelMap.Resize += (s, e) => panelMap.Invalidate();
    }

    private void InitializeComponent()
    {
            this.panelMap = new System.Windows.Forms.Panel();
            this.cbStart = new System.Windows.Forms.ComboBox();
            this.cbEnd = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxSteps = new System.Windows.Forms.ListBox();
            this.listAlgorithms = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // panelMap
            // 
            this.panelMap.BackColor = System.Drawing.Color.White;
            this.panelMap.Location = new System.Drawing.Point(30, 12);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(678, 456);
            this.panelMap.TabIndex = 0;
            this.panelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMap_Paint);
            // 
            // cbStart
            // 
            this.cbStart.Location = new System.Drawing.Point(728, 138);
            this.cbStart.Name = "cbStart";
            this.cbStart.Size = new System.Drawing.Size(150, 24);
            this.cbStart.TabIndex = 1;
            // 
            // cbEnd
            // 
            this.cbEnd.Location = new System.Drawing.Point(728, 197);
            this.cbEnd.Name = "cbEnd";
            this.cbEnd.Size = new System.Drawing.Size(150, 24);
            this.cbEnd.TabIndex = 2;
            this.cbEnd.SelectedIndexChanged += new System.EventHandler(this.cbEnd_SelectedIndexChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(728, 227);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 30);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Load Data";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(723, 318);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(100, 30);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "Find Path";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lblResult
            // 
            this.lblResult.Location = new System.Drawing.Point(720, 351);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(300, 30);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "Kết quả tìm đường ...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(725, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Chọn điểm bắt đầu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(725, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Chọn điểm kết thúc";
            // 
            // listBoxSteps
            // 
            this.listBoxSteps.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.listBoxSteps.FormattingEnabled = true;
            this.listBoxSteps.ItemHeight = 16;
            this.listBoxSteps.Location = new System.Drawing.Point(723, 384);
            this.listBoxSteps.Name = "listBoxSteps";
            this.listBoxSteps.Size = new System.Drawing.Size(165, 84);
            this.listBoxSteps.TabIndex = 8;
            this.listBoxSteps.SelectedIndexChanged += new System.EventHandler(this.listBoxSteps_SelectedIndexChanged);
            // 
            // listAlgorithms
            // 
            this.listAlgorithms.FormattingEnabled = true;
            this.listAlgorithms.Location = new System.Drawing.Point(728, 275);
            this.listAlgorithms.Name = "listAlgorithms";
            this.listAlgorithms.Size = new System.Drawing.Size(121, 24);
            this.listAlgorithms.TabIndex = 9;
            this.listAlgorithms.SelectedIndexChanged += new System.EventHandler(this.listAlgorithms_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(900, 500);
            this.Controls.Add(this.listAlgorithms);
            this.Controls.Add(this.listBoxSteps);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.cbStart);
            this.Controls.Add(this.cbEnd);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.lblResult);
            this.Name = "Form1";
            this.Text = "Dijkstra Path Finder";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }
    private void AddWrappedArrowPath(
        ListBox listBox,
        List<string> path,
        int maxCharsPerLine = 30)
    {
        if (path == null || path.Count == 0)
            return;

        string currentLine = path[0];

        for (int i = 1; i < path.Count; i++)
        {
            string next = " -> " + path[i];

            if ((currentLine + next).Length > maxCharsPerLine)
            {
                listBox.Items.Add(currentLine);
                currentLine = "-> " + path[i];
            }
            else
            {
                currentLine += next;
            }
        }

        listBox.Items.Add(currentLine);
    }


    private async void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Load NODE
                string nodeResponse = await client.GetStringAsync("http://localhost:5000/nodes");
                nodes = JsonConvert.DeserializeObject<List<ApiNode>>(nodeResponse);

                // Load EDGE
                string edgeResponse = await client.GetStringAsync("http://localhost:5000/edges");
                edges = JsonConvert.DeserializeObject<List<ApiEdge>>(edgeResponse);
            }

            // --- Thêm nodes và edges vào Graph ---
            graph = new Graph(); // reset graph
            foreach (var n in nodes)
                graph.AddNode(n.Name, n.X, n.Y);

            foreach (var edge in edges)
                graph.AddEdge(edge.From, edge.To);

            // --- Cập nhật comboBox ---
            cbStart.Items.Clear();
            cbEnd.Items.Clear();
            foreach (var n in nodes)
            {
                cbStart.Items.Add(n.Name);
                cbEnd.Items.Add(n.Name);
            }

            // --- Auto scale ---
            if (nodes.Count > 0)
            {
                minX = (float)nodes.Min(n => n.X);
                minY = (float)nodes.Min(n => n.Y);
                float maxX = (float)nodes.Max(n => n.X);
                float maxY = (float)nodes.Max(n => n.Y);

                float w = panelMap.Width - 40;
                float h = panelMap.Height - 40;

                float rangeX = maxX - minX;
                float rangeY = maxY - minY;
                if (rangeX == 0) rangeX = 1;
                if (rangeY == 0) rangeY = 1;

                scale = Math.Min(w / rangeX, h / rangeY);
            }

            panelMap.Invalidate(); // vẽ lại panel
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
        }
    }
    private List<string> tempPath = null;

    private async void btnFind_Click(object sender, EventArgs e)
    {
        if (cbStart.SelectedItem == null || cbEnd.SelectedItem == null)
        {
            MessageBox.Show("Chọn điểm bắt đầu và kết thúc!");
            return;
        }

        if (listAlgorithms.SelectedItem == null)
        {
            MessageBox.Show("Chọn thuật toán!");
            return;
        }

        string start = cbStart.SelectedItem.ToString();
        string end = cbEnd.SelectedItem.ToString();
        string algorithm = listAlgorithms.SelectedItem.ToString();

        listBoxSteps.Items.Clear();
        listBoxSteps.Items.Add($"Thuật toán: {algorithm}");

        try
        {
            using (HttpClient client = new HttpClient())
            {
                string url =
                    $"http://localhost:5000/path" +
                    $"?start={start}&end={end}&algo={algorithm}";

                string json = await client.GetStringAsync(url);

                ApiPathResult result =
                    JsonConvert.DeserializeObject<ApiPathResult>(json);

                // ---- HIỂN THỊ ----
                listBoxSteps.Items.Add(
                    $"Thời gian chạy: {result.milliseconds} ms");

                if (result.distance > 0)
                    listBoxSteps.Items.Add(
                        $"Độ dài đường đi: {result.distance:F2}");

                if (result.path != null && result.path.Length > 0)
                {
                    tempPath = result.path.ToList();
                    AddWrappedArrowPath(listBoxSteps, tempPath, 30);
                }
                else
                {
                    tempPath = null;
                    listBoxSteps.Items.Add("Không tìm thấy đường đi!");
                }

                panelMap.Invalidate();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi tìm đường: " + ex.Message);
        }
    }


    private void panelMap_Paint(object sender, PaintEventArgs e)
    {
        if (nodes == null || nodes.Count == 0) return;

        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // --- Vẽ EDGE trước ---
        if (edges != null)
        {
            foreach (var edge in edges)
            {
                var from = nodes.Find(n => n.Name == edge.From);
                var to = nodes.Find(n => n.Name == edge.To);

                if (from != null && to != null)
                {
                    int x1 = (int)((from.X - minX) * scale) + 20;
                    int y1 = (int)((from.Y - minY) * scale) + 20;
                    int x2 = (int)((to.X - minX) * scale) + 20;
                    int y2 = (int)((to.Y - minY) * scale) + 20;

                    // Vẽ đường edge
                    g.DrawLine(new Pen(Color.DarkGray, 2), x1, y1, x2, y2);

                    // Vẽ trọng số
                    string wText = edge.Weight.ToString("F2");
                    g.DrawString(wText, new Font("Arial", 8), Brushes.Blue,
                        (x1 + x2) / 2 - 10, (y1 + y2) / 2 - 10);
                }
            }
        }

        // --- Vẽ đường đi ngắn nhất ---
        if (tempPath != null && tempPath.Count > 1)
        {
            Pen pathPen = new Pen(Color.Blue, 3);
            for (int i = 0; i < tempPath.Count - 1; i++)
            {
                var from = nodes.Find(n => n.Name == tempPath[i]);
                var to = nodes.Find(n => n.Name == tempPath[i + 1]);

                if (from != null && to != null)
                {
                    int x1 = (int)((from.X - minX) * scale) + 20;
                    int y1 = (int)((from.Y - minY) * scale) + 20;
                    int x2 = (int)((to.X - minX) * scale) + 20;
                    int y2 = (int)((to.Y - minY) * scale) + 20;

                    g.DrawLine(pathPen, x1, y1, x2, y2);
                }
            }
        }

        // --- Vẽ NODE ---
        foreach (var p in nodes)
        {
            int x = (int)((p.X - minX) * scale) + 20;
            int y = (int)((p.Y - minY) * scale) + 20;

            g.FillEllipse(Brushes.Red, x - 5, y - 5, 12, 12);
            g.DrawString($"{p.Name} ({p.X},{p.Y})", new Font("Arial", 9, FontStyle.Bold),
                         Brushes.Black, x + 10, y - 15);
        }
    }
    // --- Class Sub ---
    public class ApiNode
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class ApiEdge
    {
        public string From { get; set; }
        public string To { get; set; }
        public double Weight { get; set; }
    }
    public class ApiPath
    {
        public List<string> Path { get; set; }
        public double Distance { get; set; }
    }
    public class ApiPathResult
    {
        public string algorithm { get; set; }
        public string[] path { get; set; }
        public double distance { get; set; }
        public long milliseconds { get; set; }
    }


    private Label label1;
    private Label label2;

    private void listBoxSteps_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private ListBox listBoxSteps;

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void Form1_Load_1(object sender, EventArgs e)
    {

    }

    private void cbEnd_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private ComboBox listAlgorithms;

    private void listAlgorithms_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private void Form1_Load(object sender, EventArgs e)
    {
        // Khắc phục lỗi: Thêm các thuật toán vào ComboBox (listAlgorithms)
        listAlgorithms.Items.Clear(); // Đảm bảo làm sạch nếu có gọi lại

        listAlgorithms.Items.Add("BFS");
        listAlgorithms.Items.Add("DFS");
        listAlgorithms.Items.Add("Dijkstra");
        listAlgorithms.Items.Add("Bellman-Ford");
        listAlgorithms.Items.Add("Floyd-Warshall");
        listAlgorithms.Items.Add("A*");

        // Chọn mặc định thuật toán đầu tiên
        if (listAlgorithms.Items.Count > 0)
        {
            listAlgorithms.SelectedIndex = 0;
        }
    }

}