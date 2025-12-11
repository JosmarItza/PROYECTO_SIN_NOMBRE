using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartFade : MonoBehaviour
{
    [Header("Pantalla Negra (SpriteRenderer)")]
    public SpriteRenderer blackScreen;      // Tu sprite square negro

    [Header("Audio")]
    public AudioSource musicSource;         // AudioSource donde está la música
    public float maxVolume = 1f;            // Volumen final
    public float musicFadeDuration = 2f;    // Tiempo de fade in de música

    [Header("Fade de Pantalla")]
    public float screenFadeDuration = 2f;   // Tiempo del fade del sprite

    private void Start()
    {
        // Asegurar valores iniciales
        
        if (blackScreen != null)
        {
            Color c = blackScreen.color;
            c.a = 1f;                        // Empieza completamente negro
            blackScreen.color = c;
        }

        if (musicSource != null)
            musicSource.volume = 0f;          // Música empieza silenciosa

        StartCoroutine(FadeSequence());
    }

    private System.Collections.IEnumerator FadeSequence()
    {
        float timer = 0f;

        while (timer < screenFadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / screenFadeDuration;

            // Fade pantalla negra
            if (blackScreen != null)
            {
                Color c = blackScreen.color;
                c.a = Mathf.Lerp(1f, 0f, t);
                blackScreen.color = c;
            }

            // Fade música
            if (musicSource != null)
            {
                musicSource.volume = Mathf.Lerp(0f, maxVolume, t / (musicFadeDuration / screenFadeDuration));
            }

            yield return null;
        }

        // Asegurar valores finales
        if (blackScreen != null)
        {
            Color c = blackScreen.color;
            c.a = 0f;
            blackScreen.color = c;
        }

        if (musicSource != null)
            musicSource.volume = maxVolume;
    }
}
