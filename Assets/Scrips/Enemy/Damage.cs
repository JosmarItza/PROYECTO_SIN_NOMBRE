using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Daño")]
    public int damageAmount = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Revisa que sea el Player por layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();

            if (ph != null)
            {
                // Aplica solo el daño
                ph.TakeDamage(damageAmount);
            }
        }
    }
}