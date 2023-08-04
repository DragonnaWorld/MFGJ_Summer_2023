using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Each sensor has a weight associated to it, which is interpolated from the spread angle
/// </summary>

[ExecuteInEditMode]
public class Sensor : MonoBehaviour
{
    public struct HitInfo
    {
        public float weight;
        public bool hit;
        public float ratio;
        public GameObject visibleObject;
        
        public HitInfo(float weight, bool hit, float ratio, GameObject visibleObject)
        {
            this.weight = Mathf.Clamp01(weight);
            this.hit = hit;
            this.ratio = ratio;
            this.visibleObject = visibleObject;
        }
    }

    [SerializeField]
    string[] visibleLayers;
    int layerMask;

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
    [field: Range(0F, 360F)]
    [field: Tooltip("Angle of spread")]
    public float Angle { get; private set; }

    readonly List<Vector3> sensorsLocalSpace = new();
    readonly List<Vector3> sensorsGlobalSpace = new();
    readonly List<HitInfo> sensorsStatus = new();
    Vector3 emissionPoint;
    public float RotationAroundYAxis { get; set; }

    private void Start()
    {
        sensorsLocalSpace.Capacity = Count;
        sensorsGlobalSpace.Capacity = Count;
        sensorsStatus.Capacity = Count;

        InitializeLocalSensors();
        CalculateLayerMask();
    }

    void Update()
    {
        var rotation = origin.rotation.eulerAngles + new Vector3(0, RotationAroundYAxis, 0);
        origin.rotation = Quaternion.Euler(rotation);
        emissionPoint = origin.position + origin.TransformDirection(offset);
        rotation.y -= RotationAroundYAxis;
        origin.rotation = Quaternion.Euler(rotation);

        UpdateGlobalSensors();
    }

    void UpdateGlobalSensors()
    {
        sensorsGlobalSpace.Clear();
        var rotation = origin.rotation.eulerAngles + new Vector3(0, RotationAroundYAxis, 0);
        origin.rotation = Quaternion.Euler(rotation);
        foreach (var sensor in sensorsLocalSpace)
        {
            sensorsGlobalSpace.Add(origin.TransformDirection(sensor));
        }
        rotation.y -= RotationAroundYAxis;
        origin.rotation = Quaternion.Euler(rotation);
    }

    public HitInfo[] GetCurrentStatus()
    {
        sensorsStatus.Clear();
        for (int sensorIndex = 0; sensorIndex < Count; ++sensorIndex)
        {
            Vector3 sensor = sensorsGlobalSpace[sensorIndex];
            HitInfo info;
            float weight = 2F * (sensorIndex / (float)(Count - 1) - 0.5F);
            if (Physics.Raycast(emissionPoint, sensor, out RaycastHit hitInfo, Length, layerMask))
            {
                info = new(weight, true, hitInfo.distance / Length, hitInfo.collider.gameObject);
            }
            else
                info = new(weight, false, -1, null);
            sensorsStatus.Add(info);
        }

        return sensorsStatus.ToArray();
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

    void CalculateLayerMask()
    {
        layerMask = 0;
        foreach (string layer in visibleLayers)
            layerMask |= LayerMask.NameToLayer(layer);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        InitializeLocalSensors();
        CalculateLayerMask();
    }
    private void OnDrawGizmosSelected()
    {
        VisualizeSensors();
    }
#endif
}
