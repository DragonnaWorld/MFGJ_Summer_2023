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

    [Header("Idle")]
    public float movingInterval;
    public float movingTime;
    public float observationInterval;

    public float speedMultiplier;
    public float angleAtEachObservation;

    [Header("Tracking")]
    [HideInInspector]
    public GameObject target;
    public float acceptableRange;

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

public struct EnemyCommand
{
    public enum Type
    {
        Attack
    }

    public Type Command;
    public Vector3 AttackPosition;
}