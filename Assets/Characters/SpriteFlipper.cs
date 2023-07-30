using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flip unsymmetrical sprites by using local y-axis rotation
/// </summary>

public class SpriteFlipper : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Initial pose: [0,1]<->[right, left]")]
    bool faceLeft;
    [SerializeField]
    [Tooltip("Sprites that will be enabled when facing left")]
    GameObject[] left;
    [SerializeField]
    [Tooltip("Sprites that will be enabled when facing right")]
    GameObject[] right;
    [SerializeField]
    [Tooltip("The parent of ALL relative bones")]
    Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        TurnLeft();    
    }

    [ContextMenu("Left")]
    public void TurnLeft()
    {
        foreach (var item in left)
            item.SetActive(true);
        foreach (var item in right)
            item.SetActive(false);
        var rotation = parent.localRotation;
        float yRotation = faceLeft ? 0 : 180;
        rotation = Quaternion.Euler(rotation.eulerAngles.x, yRotation, rotation.eulerAngles.z);
        parent.localRotation = rotation;
    }

    [ContextMenu("Right")]
    public void TurnRight()
    {
        foreach (var item in left)
            item.SetActive(false);
        foreach (var item in right)
            item.SetActive(true);
        var rotation = parent.localRotation;
        float yRotation = faceLeft ? 180 : 0;
        rotation = Quaternion.Euler(rotation.eulerAngles.x, yRotation, rotation.eulerAngles.z);
        parent.localRotation = rotation;
    }
}
