using Internal;
using Unity.VisualScripting;
using UnityEngine;

public class AvoidObstacle : MonoBehaviour
{
    Sensor sensor;
    Movement movement;

    [SerializeField]
    [Range(0F, 1F)]
    [Tooltip("How much this component affect the whole steering of the model")]
    float effectWeight;

    private void Start()
    {
        sensor = GetComponent<Sensor>();
        movement = GetComponent<Movement>();
    }

    public void Steer()
    {
        float turnRatio = CalculateTurnStrength();
        movement.AddTurn(turnRatio * effectWeight);
    }

    float CalculateTurnStrength()
    {
        var sensorsInfo = sensor.GetCurrentStatus();
        float ratio = 0;
        foreach (var sensor in sensorsInfo)
        {
            bool isWall = sensor.visibleObject != null && RelationshipTable.Enemy.RelationshipTo(sensor.visibleObject.tag) == Relationship.Ignore;
            if (isWall)
                ratio += sensor.weight * Inverse01(sensor.ratio);
        }
        return ratio;
    }

    float Inverse01(float x)
    {
        x = Mathf.Clamp01(x);
        if (x == 0)
            return 1;
        return Mathf.Min(1F / 3F / x - 1F / 3F, 1F);
    }
}