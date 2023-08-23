using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [field: SerializeField] public Collider MainCollider { get; private set; }
    [field: SerializeField] public SpriteFlipper SpriteFlipper { get; private set; }
    [field: SerializeField] public Rigidbody Rgbody { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }

    [field: Header("Stats")] 
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float TransitionTime { get; private set; }

    public static readonly string IdleName = "Base Layer.Idle";
    public static readonly string Attack1 = "Base Layer.Attack1";
    public static readonly string Attack2 = "Base Layer.Attack2";
    public static readonly string Attack3 = "Base Layer.Attack3";
}