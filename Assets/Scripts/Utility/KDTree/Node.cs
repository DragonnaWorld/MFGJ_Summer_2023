#nullable enable
using System.Collections.Generic;

namespace KDTree
{
    public class Node<CoordinateType>
    {
        public Point<CoordinateType> point;
        public Node<CoordinateType>? Left = null;
        public Node<CoordinateType>? Right = null;
        public bool IsLeaf
        {
            get { return Left == null && Right == null;}
        }

        public Node(Point<CoordinateType> point)
        {
            this.point = point;
        }
    }

    public class PointComparer<CoordinateType> : IComparer<Point<CoordinateType>>
    {
        readonly int indexToCompare;

        public PointComparer(int indexToCompare)
        {
            this.indexToCompare = indexToCompare;
        }

        public int Compare(Point<CoordinateType> x, Point<CoordinateType> y)
        {
            float xx = x.GetFloat(indexToCompare);
            float yy = y.GetFloat(indexToCompare);
            return xx == yy ? 0 : (xx > yy ? 1 : -1);
        }
    }    
}