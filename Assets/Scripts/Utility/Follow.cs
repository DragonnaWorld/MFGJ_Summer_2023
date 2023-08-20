using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    Transform follower;

    [SerializeField]
    [Range(1F, 10F)]
    float approachingSpeed;

    void FixedUpdate()
    {
        Vector3 newPosition = Vector3.Lerp(follower.position, player.position, approachingSpeed * Time.fixedDeltaTime);  
        follower.position = newPosition;
    }
}
