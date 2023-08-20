using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "Hitbox" and "Hurtbox" layers should only collide with each other
/// </summary>
public class Hurtbox : MonoBehaviour
{
    [field: SerializeField]
    public Health Health { get; private set; }

    static readonly IDGenerator IDManager = new(500);

    public int ID { get; private set; }

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Hurtbox");
        ID = IDManager.Generate();
    }

    private void OnDestroy()
    {
        IDManager.Retrieve(ID);
    }
}
