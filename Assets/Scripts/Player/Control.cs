using UnityEngine;

public class Control : MonoBehaviour
{
    Rigidbody rgbody;
    [SerializeField]
    [Range(1F, 100F)]
    float speed;
    [SerializeField]
    Animator animator;

    private void Start()
    {
        rgbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = new();
        if (Input.GetKey(KeyCode.A))
            direction.x -= 1F;
        if (Input.GetKey(KeyCode.S))
            direction.z -= 1F;
        if (Input.GetKey(KeyCode.D))
            direction.x += 1F;
        if (Input.GetKey(KeyCode.W))
            direction.z += 1F;
        Vector3 velocity = direction.normalized * speed;
        rgbody.velocity = new Vector3(velocity.x, rgbody.velocity.y, velocity.z);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("QueryAttack", true);
        }
        else
        {
            animator.SetBool("QueryAttack", false);
        }
    }
}