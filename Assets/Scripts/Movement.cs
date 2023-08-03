using UnityEngine;

/// <summary>
/// Components for moving a game object.
/// </summary>

public enum OmniDirection
{
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,
    LU = Left | Up,
    LD = Left | Down,
    RU = Right | Up,
    RD = Right | Down
}

public class Movement : MonoBehaviour
{
    Rigidbody rgbody;

    [field: SerializeField]
    [field: Range(1F, 20F)]
    public float Speed { get; private set; }

    private void Start()
    {
        rgbody = GetComponent<Rigidbody>();
    }

    public void Move(OmniDirection direction)
    {
        Vector3 velocity = new();
        if (direction.HasFlag(OmniDirection.Left)) velocity.x -= 1;
        if (direction.HasFlag(OmniDirection.Right)) velocity.x += 1;
        if (direction.HasFlag(OmniDirection.Up)) velocity.z += 1;
        if (direction.HasFlag(OmniDirection.Down)) velocity.z -= 1;

        velocity = velocity.normalized * Speed;

        rgbody.velocity = new(velocity.x, rgbody.velocity.y, velocity.z);
    }
}