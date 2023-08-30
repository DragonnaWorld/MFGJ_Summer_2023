using UnityEngine;
using System.Collections.Generic;
using Internal;

public class EnemyInfo : MonoBehaviour
{
    public RelationshipTable RelationshipTable = RelationshipTable.Enemy;

    [field: Header("Components")]
    [field: SerializeField] public Sensor Sensor { get; private set; }
    [field: SerializeField] public Movement Movement { get; private set; }
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public SpriteFlipper SpriteFlipper { get; private set; }
    [field: SerializeField] public AnimatorWrapper Animator { get; private set; }
    [field: SerializeField] public Pathfinding Pathfinder { get; private set; }

    [field: Header("Tracking")]
    [field: SerializeField] [field: Range(1F, 100F)] public float AcceptedRange { get; private set; }
    [field: SerializeField] [field: Range(1F, 5F)] public float NodeRange { get; private set; }
    [field: SerializeField] [field: Range(0F, 5F)] public float PathfindingTime { get; private set; }
    [field: SerializeField] [field: Range(0F, 5F)] public float FailWaitTime { get; private set; }


    public static readonly string IdleName = "Base Layer.Idle";
    public static readonly string MoveName = "Base Layer.Move";
    public static readonly string AttackName = "Base Layer.Attack";

    public GameObject TargetOfInterest;

    public bool TargetInSight(out Sensor.HitInfo hitInfo)
    {
        foreach (var hit in Sensor.GetCurrentStatus())
        {
            if (hit.hit && hit.visibleObject == TargetOfInterest)
            {
                hitInfo = hit;
                return true;
            }
        }
        hitInfo = new();
        return false;
    }
}