using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerHealth playerHealth;      // Script de vida del jugador
    public GameObject heartPrefab;         // Prefab de corazón
    public Transform heartsContainer;      // Panel donde se mostrarán los corazones

    [Header("Shake")]
    public float shakeDuration = 0.3f;     // Duración del temblor
    public float shakeMagnitude = 5f;      // Intensidad del temblor

    private List<Image> hearts = new List<Image>();
    private List<Vector3> originalPositions = new List<Vector3>();

    void Start()
    {
        GenerateHearts();
    }

    void Update()
    {
        UpdateHearts();
    }

    void GenerateHearts()
    {
        foreach (Transform child in heartsContainer)
            Destroy(child.gameObject);

        hearts.Clear();
        originalPositions.Clear();

        for (int i = 0; i < playerHealth.maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsContainer);
            hearts.Add(heart.GetComponent<Image>());
            originalPositions.Add(heart.transform.localPosition);
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < playerHealth.currentHealth)
                hearts[i].color = Color.white; // Corazón lleno
            else
                hearts[i].color = Color.gray;  // Corazón vacío
        }
    }

    // Llamar esto desde PlayerHealth cuando recibe daño
    public void ShakeHearts()
    {
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        // Guardamos posiciones originales por si alguna cambia
        for (int i = 0; i < hearts.Count; i++)
            originalPositions[i] = hearts[i].transform.localPosition;

        while (elapsed < shakeDuration)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                float x = Random.Range(-shakeMagnitude, shakeMagnitude);
                float y = Random.Range(-shakeMagnitude, shakeMagnitude);
                hearts[i].transform.localPosition = originalPositions[i] + new Vector3(x, y, 0);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Volvemos a la posición original
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].transform.localPosition = originalPositions[i];
        }

        // Actualizamos colores según vida actual
        UpdateHearts();
    }
}
