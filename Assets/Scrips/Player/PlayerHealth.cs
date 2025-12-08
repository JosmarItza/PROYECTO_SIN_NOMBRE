using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida del Jugador")]
    public int maxHealth = 5;

    [HideInInspector]
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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