using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EsceneTransition : MonoBehaviour
{
    [Header("Transición")]
    public Transform spriteObj;
    public float slideSpeed = 10f;
    public float targetX = 0f;
    public float waitTime = 1f;

    [Header("Escena")]
    public string sceneToLoad;

    [Header("Sonido Botón")]
    public AudioSource audioSource;   
    public AudioClip clickSound;

    [Header("Músicas a apagar (ambas en el mismo objeto)")]
    public GameObject musicObject;       
    private AudioSource[] musicSources;

    [Header("Fade Out")]
    public float fadeDuration = 1f;   // 🔥 Tiempo del desvanecimiento

    private bool isSliding = false;

    void Start()
    {
        if (musicObject != null)
            musicSources = musicObject.GetComponents<AudioSource>();
    }

    public void PlaySound()
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    public void StartSceneChange()
    {
        if (!isSliding)
            StartCoroutine(SlideAndChangeScene());
    }

    private IEnumerator SlideAndChangeScene()
    {
        isSliding = true;

        while (spriteObj.position.x > targetX)
        {
            spriteObj.position += Vector3.left * slideSpeed * Time.deltaTime;
            yield return null;
        }

        // 🔥 Iniciar fade de volúmenes
        if (musicSources != null)
            StartCoroutine(FadeOutAudioSources());

        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(sceneToLoad);
    }

    // 🔥 Método que hace fade a todos los AudioSource
    private IEnumerator FadeOutAudioSources()
    {
        float elapsed = 0f;
        float[] startVolumes = new float[musicSources.Length];

        // Guardar volúmenes originales
        for (int i = 0; i < musicSources.Length; i++)
        {
            startVolumes[i] = musicSources[i].volume;
        }

        // Fade
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;

            for (int i = 0; i < musicSources.Length; i++)
            {
                if (musicSources[i] != null)
                    musicSources[i].volume = Mathf.Lerp(startVolumes[i], 0f, t);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurar que queden en volumen 0 y se apaguen
        for (int i = 0; i < musicSources.Length; i++)
        {
            if (musicSources[i] != null)
            {
                musicSources[i].volume = 0f;
                musicSources[i].Stop();
            }
        }
    }
}