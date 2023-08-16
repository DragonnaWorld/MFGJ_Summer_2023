using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    TerrainBuilder builder;

    [SerializeField]
    Transform target;

    [SerializeField]
    [Range(1F, 10F)]
    float refreshInterval = 5f;
    float remaining;

    List<Vector3> path = new();

    private void Update()
    {
        remaining -= Time.deltaTime;
        if (remaining < 0)
        {
            FindPath();
            remaining = refreshInterval;
        }
    }

    [ContextMenu("Visualise")]
    public void FindPath()
    {
        if (builder.Available && builder.AStarPathfinding(transform.position, target.position, out List<Vector3> path))
        {
            this.path = path;
        }
    }
#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        for (int i = 0; i + 1 < path.Count; ++i)
            Debug.DrawLine(path[i], path[i + 1], Color.blue);
    }
#endif
}