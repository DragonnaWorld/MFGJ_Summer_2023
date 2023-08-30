using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    TerrainBuilder builder;

    List<Vector3> path;

    public bool FindPathToPosition(Vector3 destination, out List<Vector3> path)
    {
        path = null;
        if (!builder.Available)
            return false;
        if (builder.AStarPathfinding(transform.position, destination, out path))
        {
            this.path = path;
            return true;
        }
        else
        {
            this.path = null;
            return false;
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        if (path == null)
            return;
        for (int i = 0; i + 1 < path.Count; ++i)
            Debug.DrawLine(path[i], path[i + 1], Color.blue);
    }
#endif
}