using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Properties")]
    public float Speed;
    public float lookAmmount;

    public float maxHealth;
    public float health;

    [Header("References")]
    public Transform cameraObject;
    public Transform cameraTarget;
    public GameObject swordCollider;


    private Rigidbody rB;
    private float attackDirection;


    void Start()
    {
        // Cache References
        rB = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Movement();
        CameraMovement();
        Attack();
    }

    void Attack() 
    {
        swordCollider.transform.eulerAngles = (Vector3.forward * -90 + Vector3.up * attackDirection);
    }

    void CameraMovement()
    {
        Vector3 mousePos = Input.mousePosition;
        var camPosition = new Vector3(
            (Mathf.Clamp(mousePos.x, 0, Screen.width) - Screen.width / 2) / Screen.width,
            0,
            (Mathf.Clamp(mousePos.y, 0, Screen.height) - Screen.height / 2) / Screen.height
        ) * lookAmmount;
        cameraObject.localPosition = camPosition;
        attackDirection = -45 + Mathf.Atan2(camPosition.x, camPosition.z) * Mathf.Rad2Deg;
    }

    void Movement()
    {
        Vector3 movementVector;
        // Apply Input
        movementVector = Input.GetAxis("Horizontal") * Vector3.right;
        movementVector += Input.GetAxis("Vertical") * Vector3.forward;

        // Rotate for isometric view
        movementVector = Quaternion.Euler(0, 45, 0) * movementVector;

        movementVector.Normalize();

        // Apply the movement vector
        rB.AddForce(movementVector*Speed, ForceMode.Impulse);

        if(rB.velocity.magnitude > Speed)
        {
            rB.velocity /=  rB.velocity.magnitude/Speed;
        }
    }

    public void TakeDamage(float damage) 
    {
        health -= damage;
        health = Mathf.Clamp(health, 0f, maxHealth);
    }
}
