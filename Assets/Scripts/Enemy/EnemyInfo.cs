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
    [field: SerializeField] public Animator Animator { get; private set; }

    public static readonly string IdleName = "Base Layer.Idle";
    public static readonly string MoveName = "Base Layer.Move";
}