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
    readonly Dictionary<int, Dictionary<int, float>> adjacent;
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

    public void Connect(Coord coordinate, List<Tuple<Coord, float>> adj) 
    {
        int index = indexHasher.Hash(coordinate);
        foreach (var edge in adj)
        {
            int neighbor = indexHasher.Hash(edge.Item1);
            if (!costs.ContainsKey(neighbor))
                throw new ArgumentException("Some adjacent nodes are not added yet");
            if (!adjacent[index].ContainsKey(neighbor))
                adjacent[index].Add(neighbor, edge.Item2);
            if (!adjacent[neighbor].ContainsKey(index))
                adjacent[neighbor].Add(index, edge.Item2);
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
            foreach (var edge in adjacent[currentIndex])
            {
                bool visited = (edge.Key == startIndex) || visitCost.ContainsKey(edge.Key);
                if (!visited)
                {
                    backtrace.Add(edge.Key, currentIndex);
                    visitCost.Add(edge.Key, visitCost[currentIndex] + costs[edge.Key] + edge.Value);
                    toBeVisited.Enqueue(edge.Key, visitCost[edge.Key] + heuristic.Get(indexHasher.Revert(edge.Key), endCoord));
                }
            }
        }
        // no path
        return false;
    }
}