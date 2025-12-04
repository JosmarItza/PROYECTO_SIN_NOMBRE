using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth player = collision.collider.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
    
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    */
}