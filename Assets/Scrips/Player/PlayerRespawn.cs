using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{

    private Vector3 respawnPoint;
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;

    void Start()
    {
        respawnPoint = transform.position; // Respawn inicial
        playerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(playerHealth != null && playerHealth.currentHealth <= 0)
        {
            Respawn();
            playerHealth.currentHealth = playerHealth.maxHealth; // restaurar vida
        }
    }

    public void SetCheckpoint(Vector3 point)
    {
        respawnPoint = point;
    }

    private void Respawn()
    {
        transform.position = respawnPoint;
        if(rb != null)
            rb.velocity = Vector2.zero;

        // Reinicia animaciones si tienes Animator
        Animator animator = GetComponent<Animator>();
        if(animator != null)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isDashing", false);
        }
    }
}
