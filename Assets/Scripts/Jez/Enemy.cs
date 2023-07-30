using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // Enemy attributes
    public int maxHealth = 100;
    public int health;

    // Movement variables
    public float movementSpeed = 2f;

    // Attack variables
    public int attackDamage = 20;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private bool canAttack = true;

    // References
    private Transform playerTransform;
    private Player player;
    private Animator animator;

    public abstract void Attack();
     public abstract void Movement();

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Player player = playerTransform.GetComponent<Player>();
        animator = GetComponent<Animator>();
        
        health = maxHealth;
    }

    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
        else
        {
            Movement();
            TryAttack();
        }
    }
    

    private void TryAttack()
    {
        if (canAttack && Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            Attack();
            // Set attack cooldown
            canAttack = false;
            Invoke("ResetAttackCooldown", attackCooldown);
        }
    }   


    private void ResetAttackCooldown()
    {
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Trigger hurt animation or effects

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Trigger death animation or effects

        gameObject.SetActive(false);
    }
}

