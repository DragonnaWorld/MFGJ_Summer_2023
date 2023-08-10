using UnityEngine;
using System.Collections.Generic;
using Internal;

public class EnemyInfo : MonoBehaviour
{
    public RelationshipTable RelationshipTable = RelationshipTable.Enemy;

    [Header("Components")]

    [HideInInspector]
    public Sensor Sensor;

    [HideInInspector]
    public Movement Movement;

    [HideInInspector]
    public Rigidbody Rigidbody;

    public SpriteFlipper SpriteFlipper;

    [Header("Animation")]

    public Animator Animator;

    // [Header("Idle")]

    // [Header("Tracking")]

    private void Start()
    {
        Sensor = GetComponent<Sensor>();
        Movement = GetComponent<Movement>();
        Rigidbody = GetComponent<Rigidbody>();
    }
}

public enum EnemyState
{
    Idle, Track,
}