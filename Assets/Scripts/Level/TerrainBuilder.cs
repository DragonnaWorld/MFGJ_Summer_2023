using System.Collections.Generic;
using System;
using UnityEngine;

public class Vec3IHasher : IIndexHasher<Vector3Int>
{
    readonly int Bound = 512;

    public int Hash(Vector3Int v)
    {
        v += Bound * Vector3Int.one;
        return v.x + v.y * 2 * Bound + v.z * 4 * Bound * Bound;
    }

    public Vector3Int Revert(int i)
    {
        int z = i / (4 * Bound * Bound);
        int y = (i - z * 4 * Bound * Bound) / (2 * Bound);
        int x = i - y * 2 * Bound - z * 4 * Bound * Bound;
        return new Vector3Int(x - Bound, y - Bound, z - Bound);
    }
}

public class Heuristic3D : IHeuristic<Vector3Int>
{
    enum Type
    {
        Euclid, Manhattan
    }

    Type type;

    public static Heuristic3D Euclid = new() { type = Type.Euclid };
    public static Heuristic3D Manhattan = new() { type = Type.Manhattan };

    public float Get(Vector3Int p1, Vector3Int p2)
    {
        switch (type)
        {
            case Type.Euclid:
                return (p1 - p2).magnitude;
            case Type.Manhattan:
                return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y);
            default:
                return 0;
        }
    }
}

public class TerrainBuilder : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Number of blocks each dimension")]
    Vector3Int Dimension;

    [SerializeField]
    [Tooltip("The center of the terrain")]
    Vector3 Center;

    [SerializeField]
    [Tooltip("Size of each cell")]
    Vector3 CellSize;

    [SerializeField]
    [Tooltip("Layers that will be taken into account")]
    string[] PreventLayers;
    int preventMask;
    [SerializeField]
    string[] AllowLayers;
    int allowMask;

    readonly HashSet<Vector3Int> validTileLists = new();
    readonly Search<Vector3Int> pathfinder = new(new Vec3IHasher());
    public bool Available { get; private set; } = false;

    private void Start()
    {
        allowMask = 0;
        preventMask = 0;
        foreach (var layer in AllowLayers)
            allowMask |= 1 << LayerMask.NameToLayer(layer);
        foreach (var layer in PreventLayers)
            preventMask |= 1 << LayerMask.NameToLayer(layer);
    }

    private void Update()
    {
        if (!Available)
        {
            CalculateCellData();
            Available = true;
        }
    }

    public bool AStarPathfinding(Vector3 start, Vector3 end, out List<Vector3> path)
    {
        path = new();
        if (!pathfinder.AStarPathfinding(PositionToTile(start), PositionToTile(end),
            Heuristic3D.Euclid, out List<Vector3Int> pathInTile))
                return false;
        foreach (var tile in pathInTile)
            path.Add(TileToPosition(tile));
        return true;
    }

    Vector3Int PositionToTile(Vector3 pos)
    {
        return new(Mathf.FloorToInt((pos.x - Center.x) / CellSize.x),
            Mathf.FloorToInt((pos.y - Center.y) / CellSize.y),
            Mathf.FloorToInt((pos.z - Center.z) / CellSize.z));
    }

    Vector3 TileToPosition(Vector3Int tile)
    {
        return CellSize / 2F + new Vector3(tile.x * CellSize.x + Center.x, 
            tile.y * CellSize.y + Center.y, 
            tile.z * CellSize.z + Center.z);
    }

    void CalculateCellData()
    {
        pathfinder.Clear();
        validTileLists.Clear();
        // Update list by order + add valid tile to pathfinder
        for (int x = -Dimension.x / 2; x <= Dimension.x / 2; ++x)
            for (int y = -Dimension.y / 2; y <= Dimension.y / 2; ++y)
                for (int z = -Dimension.z / 2; z <= Dimension.z / 2; ++z)
                {
                    var tile = new Vector3Int(x, y, z);
                    var position = TileToPosition(tile);
                    bool isValid = Physics.CheckBox(position, CellSize / 2F, Quaternion.identity, allowMask)
                                && !Physics.CheckBox(position, CellSize / 2F, Quaternion.identity, preventMask);
                    if (!isValid)
                        continue;

                    validTileLists.Add(tile);
                    pathfinder.Add(tile, 1);
                }
        // Add pathfinder connections
        for (int x = -Dimension.x / 2; x <= Dimension.x / 2; ++x)
            for (int y = -Dimension.y / 2; y <= Dimension.y / 2; ++y)
                for (int z = -Dimension.z / 2; z <= Dimension.z / 2; ++z)
                {
                    var tile = new Vector3Int(x, y, z);
                    if (!validTileLists.Contains(tile))
                        continue;
                    List<Tuple<Vector3Int, float>> adjacents = new();
                    for (int osx = -1; osx <= 1; ++osx)
                        for (int osz = -1; osz <= 1; ++osz)
                        {
                            if (osx == 0 && osz == 0)
                                continue;
                            var offset = new Vector3Int(osx, 0, osz);
                            var adjacentTile = tile + offset;
                            if (validTileLists.Contains(adjacentTile))
                                adjacents.Add(new (adjacentTile, 2F * offset.magnitude));
                        }
                    pathfinder.Connect(tile, adjacents);
                }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!Available)
            return;
        for (int x = -Dimension.x / 2; x <= Dimension.x / 2; ++x)
            for (int y = -Dimension.y / 2; y <= Dimension.y / 2; ++y)
                for (int z = -Dimension.z / 2; z <= Dimension.z / 2; ++z)
                {
                    Vector3Int coordinate = new(x, y, z);
                    if (!validTileLists.Contains(coordinate))
                        continue;

                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(TileToPosition(coordinate), CellSize);
                }
    }  
#endif
}