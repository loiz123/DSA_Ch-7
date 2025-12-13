using System;

public static class HeuristicHelper
{
    public static double Euclid(Node a, Node b)
    {
        double dx = a.X - b.X;
        double dy = a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
