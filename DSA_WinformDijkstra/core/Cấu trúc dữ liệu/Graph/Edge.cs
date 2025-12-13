using System;

public class Edge
{
    public Node Node1 { get; set; }
    public Node Node2 { get; set; }
    public double Weight { get; set; }

    public Edge(Node node1, Node node2)
    {
        Node1 = node1;
        Node2 = node2;
        Weight = Math.Sqrt(Math.Pow(node2.X - node1.X, 2) + Math.Pow(node2.Y - node1.Y, 2));
    }
}