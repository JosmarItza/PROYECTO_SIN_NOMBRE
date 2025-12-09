using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida del Jugador")]
    public int maxHealth = 5;

    [HideInInspector]
    public int currentHealth;

    [Header("Sonido de Daño")]
    public AudioSource audioSource;   // Arrastra un AudioSource del jugador
    public AudioClip damageClip;      // Sonido de daño
    public float damageVolume = 1f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // 🎧 Reproducir sonido de daño
        if (audioSource != null && damageClip != null)
        {
            audioSource.PlayOneShot(damageClip, damageVolume);
        }

        FindObjectOfType<UI>().ShakeHearts();
    }

    // 🔥 FUNCIÓN PARA CURAR VIDA
    public void Heal(int amount)
    {
        int before = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}