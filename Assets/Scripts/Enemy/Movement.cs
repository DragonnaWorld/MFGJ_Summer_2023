using UnityEngine;

/// <summary>
/// Components for moving NPC
/// </summary>

public static class AngleNormalizer
{
    public static float Normalize360(float angle)
    {
        return angle - Mathf.Floor(angle / 360F) * 360F;
    }

    public static float ShortestDifference360(float begin, float end)
    {
        float res = Normalize360(end) - Normalize360(begin);
        if (res > 180F)
            return res - 360F;
        if (res < -180F)
            return res + 360F;
        return res;
    }
}

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
    public float MaxAngularSpeed { get; private set; }

    public float CurrentSpeed { get; private set; } = 0;
    public float CurrentAngle { get; private set; } = 0;
    public float CurrentAngularSpeed { get; private set; } = 0;


    private void FixedUpdate()
    {
        CurrentAngle = AngleNormalizer.Normalize360(CurrentAngle + CurrentAngularSpeed * Time.fixedDeltaTime);
        float angleInRad = CurrentAngle * Mathf.Deg2Rad;
        rgbody.velocity = new(CurrentSpeed * Mathf.Sin(angleInRad), rgbody.velocity.y, CurrentSpeed * Mathf.Cos(angleInRad));
    }

    public void SetTurn(float ratioToMaxAngularSpeed)
    {
        ratioToMaxAngularSpeed = Mathf.Clamp(ratioToMaxAngularSpeed, -1, 1);
        CurrentAngularSpeed = MaxAngularSpeed * ratioToMaxAngularSpeed;
    }

    public void AddTurn(float ratioToMaxAngularSpeed)
    {
        SetTurn(CurrentAngularSpeed / MaxAngularSpeed + ratioToMaxAngularSpeed);
    }

    public void SetSpeed(float ratioToMaxSpeed)
    {
        CurrentSpeed = MaxSpeed * Mathf.Clamp01(ratioToMaxSpeed);
    }
}