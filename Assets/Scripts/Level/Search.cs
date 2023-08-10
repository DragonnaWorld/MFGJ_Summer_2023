using System;
using System.Collections.Generic;
using UnityEngine;

public interface IIndexHasher<T>
{
    int Hash(T value);
    T Revert(int index);
}

public interface IHeuristic<T>
{
    float Get(T p1, T p2);
}

public class Search<Coord>
{
    readonly Dictionary<int, float> costs;
    readonly Dictionary<int, HashSet<int>> adjacent;
    readonly IIndexHasher<Coord> indexHasher;

    public Search(IIndexHasher<Coord> indexHasher)
    {
        costs = new();
        adjacent = new();
        this.indexHasher = indexHasher;
    }

    public void Add(Coord coordinate, float cost)
    {
        int index = indexHasher.Hash(coordinate);
        costs.Add(index, cost);
        adjacent.Add(index, new());
    }

    public void Connect(Coord coordinate, List<Coord> adj) 
    {
        int index = indexHasher.Hash(coordinate);
        foreach (var coord in adj)
        {
            int neighbor = indexHasher.Hash(coord);
            if (!costs.ContainsKey(neighbor))
                throw new ArgumentException("Some adjacent nodes are not added yet");
            adjacent[index].Add(neighbor);
            adjacent[neighbor].Add(index);
        }
    }

    public void Clear()
    {
        costs.Clear();
        adjacent.Clear();
    }

    public bool AStarPathfinding(Coord startCoord, Coord endCoord, IHeuristic<Coord> heuristic, out List<Coord> path)
    {
        path = new();

        Utils.PriorityQueue<int, float> toBeVisited = new();
        Dictionary<int, float> visitCost = new();
        Dictionary<int, int> backtrace = new();

        int startIndex = indexHasher.Hash(startCoord);
        int endIndex = indexHasher.Hash(endCoord);

        if (costs.ContainsKey(startIndex) == false || costs.ContainsKey(endIndex) == false)
            throw new ArgumentException("start (or end) coordinates is not within search area");

        toBeVisited.Enqueue(startIndex, 0);
        visitCost.Add(startIndex, 0);

        while (toBeVisited.Count > 0)
        {
            int currentIndex = toBeVisited.Dequeue();
            if (currentIndex == endIndex)
            {
                // backtracking
                path.Add(indexHasher.Revert(endIndex));
                do
                {
                    endIndex = backtrace[endIndex];
                    path.Add(indexHasher.Revert(endIndex));
                }
                while (endIndex != startIndex);
                path.Reverse();
                return true;
            }
            foreach (var next in adjacent[currentIndex])
            {
                bool visited = (next == startIndex) || visitCost.ContainsKey(next);
                // priority = -cost from start - heuristic
                if (!visited)
                {
                    backtrace.Add(next, currentIndex);
                    visitCost.Add(next, visitCost[currentIndex] + costs[next]);
                    toBeVisited.Enqueue(next, visitCost[next] + heuristic.Get(indexHasher.Revert(next), endCoord));
                }
            }
        }
        // no path
        return false;
    }
}