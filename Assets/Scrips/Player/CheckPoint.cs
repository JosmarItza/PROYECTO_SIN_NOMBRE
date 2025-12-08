using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Sprite activatedSprite; // Sprite opcional cuando el checkpoint se activa
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Guardar posición del checkpoint
            PlayerRespawn playerRespawn = collision.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.SetCheckpoint(transform.position);
            }

            // Cambiar sprite para mostrar que está activado
            if (activatedSprite != null && sr != null)
                sr.sprite = activatedSprite;
        }
    }
}
