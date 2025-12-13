using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenreButton : MonoBehaviour
{
    [Header("Datos de la escena")]
    public string sceneToLoad = "NombreDeLaEscena";

    [Header("Delay del cambio de escena")]
    public float delay = 2f;

    [Header("Fade visual (Square negro con Sprite Renderer)")]
    public SpriteRenderer fadeObject;
    public float fadeDuration = 1f;

    [Header("Sonido del botón")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    [Header("Música de fondo (Objeto con 2 AudioSource)")]
    public GameObject musicObject;
    private AudioSource[] musicSources;

    [Header("Fade de audio")]
    public float musicFadeDuration = 1.5f;

    private void Start()
    {
        if (musicObject != null)
            musicSources = musicObject.GetComponents<AudioSource>();
    }

    public void ChangeSceneWithDelay()
    {
        // 🔊 Reproducir sonido clic
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        // 🎵 Iniciar fade de música
        if (musicSources != null)
            StartCoroutine(FadeOutMusic());

        // Iniciar proceso general
        StartCoroutine(ChangeSceneCoroutine());
    }

    private System.Collections.IEnumerator ChangeSceneCoroutine()
    {
        // Fade visual a negro
        if (fadeObject != null)
            StartCoroutine(FadeToBlack());

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneToLoad);
    }

    private System.Collections.IEnumerator FadeOutMusic()
    {
        float elapsed = 0f;

        // Guardamos volúmenes iniciales
        float[] startVolumes = new float[musicSources.Length];
        for (int i = 0; i < musicSources.Length; i++)
            startVolumes[i] = musicSources[i].volume;

        while (elapsed < musicFadeDuration)
        {
            float t = elapsed / musicFadeDuration;

            for (int i = 0; i < musicSources.Length; i++)
            {
                if (musicSources[i] != null)
                    musicSources[i].volume = Mathf.Lerp(startVolumes[i], 0f, t);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurar volúmenes en 0 y detener audio
        for (int i = 0; i < musicSources.Length; i++)
        {
            if (musicSources[i] != null)
            {
                musicSources[i].volume = 0f;
                musicSources[i].Stop();
            }
        }
    }

    private System.Collections.IEnumerator FadeToBlack()
    {
        float elapsed = 0f;
        Color c = fadeObject.color;

        float startA = c.a;
        float endA = 1f;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            c.a = Mathf.Lerp(startA, endA, t);
            fadeObject.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }

        c.a = 1f;
        fadeObject.color = c;
    }
}
