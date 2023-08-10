using System.Collections.Generic;
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
        return new(Mathf.RoundToInt((pos.x - Center.x) / CellSize.x),
            Mathf.RoundToInt((pos.y - Center.y) / CellSize.y),
            Mathf.RoundToInt((pos.z - Center.z) / CellSize.z));
    }

    Vector3 TileToPosition(Vector3Int tile)
    {
        return new(tile.x * CellSize.x + Center.x, 
            tile.y * CellSize.y + Center.y, 
            tile.z * CellSize.z + Center.z);
    }

    [ContextMenu("Bake terrain")]
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
                    pathfinder.Add(tile, 14);
                }
        // Add pathfinder connections
        for (int x = -Dimension.x / 2; x <= Dimension.x / 2; ++x)
            for (int y = -Dimension.y / 2; y <= Dimension.y / 2; ++y)
                for (int z = -Dimension.z / 2; z <= Dimension.z / 2; ++z)
                {
                    var tile = new Vector3Int(x, y, z);
                    if (!validTileLists.Contains(tile))
                        continue;
                    List<Vector3Int> adjacents = new();
                    for (int osx = -1; osx <= 1; ++osx)
                        for (int osy = -1; osy <= 1; ++osy)
                            for (int osz = -1; osz <= 1; ++osz)
                            {
                                if (osx == 0 && osy == 0 && osz == 0)
                                    continue;
                                var adjacentTile = tile + new Vector3Int(osx, osy, osz);
                                if (validTileLists.Contains(adjacentTile))
                                    adjacents.Add(adjacentTile);
                            }
                    pathfinder.Connect(tile, adjacents);
                }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        allowMask = 0;
        preventMask = 0;
        foreach (var layer in AllowLayers)
            allowMask |= 1 << LayerMask.NameToLayer(layer);
        foreach (var layer in PreventLayers)
            preventMask |= 1 << LayerMask.NameToLayer(layer);
    }

    [ContextMenu("Visualise")]
    void Visualise()
    {
        CalculateCellData();
        ClearChildren();
        for (int x = -Dimension.x / 2; x <= Dimension.x / 2; ++x)
            for (int y = -Dimension.y / 2; y <= Dimension.y / 2; ++y)
                for (int z = -Dimension.z / 2; z <= Dimension.z / 2; ++z)
                {
                    if (!validTileLists.Contains(new(x, y, z)))
                        continue;

                    var cube = new GameObject();
                    cube.name = $"({x},{y},{z})";
                    var collider = cube.AddComponent<BoxCollider>();
                    collider.isTrigger = true;
                    cube.transform.parent = transform;
                    cube.transform.position = TileToPosition(new(x, y, z));
                    cube.transform.localScale = CellSize;
                }
    }

    [ContextMenu("Clear children")]
    void ClearChildren()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
#endif
}