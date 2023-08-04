using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [HideInInspector]
    public Sensor Sensor;
    [HideInInspector]
    public Movement Movement;

    [Header("Idle")]
    public float movingInterval;
    public float observationInterval;

    public float speedMultiplier;
    public float angleAtEachObservation;

    [Header("Tracking")]
    [HideInInspector]
    public GameObject target;

    private void Start()
    {
        Sensor = GetComponent<Sensor>();
        Movement = GetComponent<Movement>();
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