using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sensor : MonoBehaviour
{
    [SerializeField]
    Transform origin;
    [SerializeField]
    [Tooltip("Offset of the root from origin")]
    Vector3 offset;

    [field: SerializeField]
    [field: Range(1F, 100F)]
    [field: Tooltip("Length of each sensor")]
    public float Length { get; private set; }

    [field: SerializeField]
    [field: Range(1, 10)]
    [field: Tooltip("Number of sensors")]
    public int Count { get; private set; }

    [field: SerializeField]
    [field: Range(0, 360)]
    [field: Tooltip("Angle of spread")]
    public uint Angle { get; private set; }

    readonly List<Vector3> sensorsLocalSpace = new();
    readonly List<Vector3> sensorsGlobalSpace = new();
    readonly List<float> sensorRatios = new();
    Vector3 emissionPoint;


    private void Start()
    {
        sensorsLocalSpace.Capacity = Count;
        sensorsGlobalSpace.Capacity = Count;
        sensorRatios.Capacity = Count;

        InitializeLocalSensors();
        Update();
    }

    void Update()
    {
        emissionPoint = transform.position + offset;
        sensorsGlobalSpace.Clear();
        foreach (var sensor in sensorsLocalSpace)
        {
            sensorsGlobalSpace.Add(origin.TransformDirection(sensor));
        }
    }

    public float[] GetCurrentStatus()
    {
        return null;
    }

    private void InitializeLocalSensors()
    {
        sensorsLocalSpace.Clear();

        if (Count == 1)
        {
            sensorsLocalSpace.Add(Vector3.forward * Length);
            return;
        }

        float leftAngleToOx = (90 + Angle / 2F) * Mathf.Deg2Rad;
        float rightAngleToOx = (90 - Angle / 2F) * Mathf.Deg2Rad;

        for (int sensorIndex = 0; sensorIndex < Count; ++sensorIndex)
        {
            float angleToOx = Mathf.Lerp(leftAngleToOx, rightAngleToOx, sensorIndex / (float)(Count - 1));
            sensorsLocalSpace.Add(new Vector3(Mathf.Cos(angleToOx), 0, Mathf.Sin(angleToOx)) * Length);
        }
    }

    void VisualizeSensors()
    {
        foreach (var sensor in sensorsGlobalSpace)
            Debug.DrawRay(emissionPoint, sensor, Color.green);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        InitializeLocalSensors();
    }
    private void OnDrawGizmosSelected()
    {
        VisualizeSensors();
    }
#endif
}
