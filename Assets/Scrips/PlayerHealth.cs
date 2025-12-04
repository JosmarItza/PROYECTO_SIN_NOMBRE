using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    public float invulnerabilityTime = 1f;
    private bool isInvulnerable = false;

    private SpriteRenderer sr;

    // public HeartsUI heartsUI;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();

        /*
        if (heartsUI != null)
        {
            heartsUI.SetMaxHearts(maxHealth);
            heartsUI.UpdateHearts(currentHealth);
        }*/
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        Debug.Log("Vida actual: " + currentHealth);

        /*
        if (heartsUI != null)
            heartsUI.UpdateHearts(currentHealth);
        */

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invulnerability());
        }
        /*
        if (heartsUI != null)
        {
            heartsUI.SetMaxHearts(maxHealth);
            heartsUI.UpdateHearts(currentHealth);
        }
        */
    }

    void Die()
    {
        Debug.Log("El personaje ha muerto.");
        // Aquí puedes desactivar movimiento, reproducir animación, reiniciar nivel, etc.
    }

    System.Collections.IEnumerator Invulnerability()
    {
        isInvulnerable = true;

        // Efecto visual de parpadeo
        for (int i = 0; i < 5; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        isInvulnerable = false;
    }
}
