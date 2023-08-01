using UnityEngine;

/// <summary>
/// Information needed to update and control an NPC.
/// </summary>

public class NPCInfo : MonoBehaviour
{
    public Rigidbody RgBody { get; private set; }
    public Movement Movement { get; private set; }
    [field: SerializeField]
    public SpriteFlipper SpriteFlipper { get; private set; }
    
    private void Start()
    {
        RgBody = GetComponent<Rigidbody>();
        Movement = GetComponent<Movement>();
    }
}