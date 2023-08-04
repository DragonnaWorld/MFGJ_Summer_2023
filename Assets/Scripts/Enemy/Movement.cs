using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Components for moving NPC
/// </summary>

public class Movement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The rigidbody this component will control")]
    Rigidbody rgbody;

    [field: SerializeField]
    [field: Range(1F, 30F)]
    public float MaxSpeed { get; private set; }

    [field: SerializeField]
    [field: Range(10F, 1000F)]
    [field: Tooltip("Angular speed in degree")]
    public float AngularSpeed { get; private set; }

    readonly Queue<float> turnsQueued = new();
    float currentSpeed = 0;
    public float CurrentAngleToOx { get; private set; } = 90F;
    float currentAngleDelta = 0;
    float targetAngleDelta = 0;
    bool turningClockwise;


    private void FixedUpdate()
    {
        UpdateTurn();
        float angleInRad = CurrentAngleToOx * Mathf.Deg2Rad;
        rgbody.velocity = new(currentSpeed * Mathf.Cos(angleInRad), rgbody.velocity.y, currentSpeed * Mathf.Sin(angleInRad));
    }

    private void UpdateTurn()
    {
        bool turnFinished = targetAngleDelta < currentAngleDelta;
        if (!turnFinished)
        {
            CurrentAngleToOx += AngularSpeed * Time.deltaTime * (turningClockwise ? -1 : 1);
            currentAngleDelta += AngularSpeed * Time.deltaTime;
        }
        else if (turnsQueued.Count != 0)
        {
            currentAngleDelta = 0;
            float nextTurn = turnsQueued.Dequeue();
            targetAngleDelta = Mathf.Abs(nextTurn);
            turningClockwise = nextTurn > 0;
        }
        else
        {
            targetAngleDelta = 0;
        }
    }

    public void AddTurn(float degreeAntiClockwise)
    {
        turnsQueued.Enqueue(degreeAntiClockwise);
    }   

    public void AbortAllTurn()
    {
        turnsQueued.Clear();
    }

    public void SetSpeed(float ratioToMaxSpeed)
    {
        currentSpeed = MaxSpeed * Mathf.Clamp01(ratioToMaxSpeed);
    }
}