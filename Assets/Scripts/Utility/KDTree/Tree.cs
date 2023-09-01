#nullable enable
using System;
using System.Collections.Generic;

namespace KDTree
{
    public class Tree<T>
    {
        readonly int dimension;
        readonly Node<T>? root;
        Node<T>? best;
        readonly Dictionary<Point<T>, Node<T>> pointToNode;
        float bestDistance = 0F;
        int visited = 0;
        
        public Tree(int dimension, List<Point<T>> points)
        {
            this.dimension = dimension;
            pointToNode = new Dictionary<Point<T>, Node<T>>();
            root = MakeTree(points, 0, points.Count, 0);
        }

        Node<T>? MakeTree(List<Point<T>> nodes, int begin, int end, int index)
        {
            if (end <= begin)
                return null;
            int n = begin + (end - begin) / 2;
            Node<T> p = new Node<T>(QuickSelect.Select(nodes, begin, end - 1, n, new PointComparer<T>(index)));
            index = (index + 1) % dimension;
            p.Left = MakeTree(nodes, begin, n, index);
            p.Right = MakeTree(nodes, n + 1, end, index);
            pointToNode[p.point] = p;
            return p;
        }

        private void Nearest(Node<T>? root, Point<T> target, int index)
        {
            if (root == null) 
                return;
            visited++;
            float d = root.point.DistanceSquared(target);
            if (best == null || d < bestDistance)
            {
                bestDistance = d;
                best = root;
            }
            if (bestDistance <= 1e-3F)
                return;
            float dx = root.point.GetFloat(index) - target.GetFloat(index);
            index = (index + 1) % dimension;
            Nearest(dx > 0 ? root.Left : root.Right, target, index);
            if (dx * dx >= bestDistance)
                return;
            Nearest(dx > 0 ? root.Right : root.Left, target, index);
        }

        public Point<T> FindNearest(Point<T> target)
        {
            if (root == null)
                throw new Exception("KDTree is empty");
            best = null;
            visited = 0;
            bestDistance = 0F;
            Nearest(root, target, 0);
            if (best == null)
                throw new Exception("KDTree unexpected error");
            return best.point;
        }

        public int Visited()
        {
            return visited;
        }

        public double Distance()
        {
            return Math.Sqrt(bestDistance);
        }
    }
}
