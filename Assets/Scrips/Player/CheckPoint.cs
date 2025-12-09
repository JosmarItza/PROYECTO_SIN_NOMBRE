using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("Sprite Activado")]
    public Sprite activatedSprite; 
    private SpriteRenderer sr;

    [Header("Sonido")]
    public AudioClip activateSound;      // sonido del checkpoint
    public AudioSource audioSource;      // opcional (usar uno existente)

    private bool activated = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated) return; // evita sonar varias veces

        if (collision.CompareTag("Player"))
        {
            // Guardar checkpoint
            PlayerRespawn playerRespawn = collision.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.SetCheckpoint(transform.position);
            }

            // Cambiar sprite
            if (activatedSprite != null && sr != null)
                sr.sprite = activatedSprite;

            // 🔊 Sonido al activar
            if (activateSound != null)
            {
                if (audioSource != null)
                    audioSource.PlayOneShot(activateSound);
                else
                    AudioSource.PlayClipAtPoint(activateSound, transform.position);
            }

            activated = true;
        }
    }
}