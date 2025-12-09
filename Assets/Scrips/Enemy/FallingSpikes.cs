using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : MonoBehaviour
{
    [Header("Detección")]
    public float rayLength = 5f;           
    public LayerMask playerLayer;          
    public float detectionDelay = 0.2f;    
    public float rayAngle = 0f;

    [Header("Temblor natural")]
    public float shakeIntensity = 0.2f;    
    public float shakeDuration = 0.6f;     
    public float fallDelay = 0.1f;         

    private Rigidbody2D rb;
    private Vector3 originalPos;
    private bool triggered = false;

    [Header("Sonido al caer")]
    public LayerMask groundLayer;          // Layer del suelo
    public AudioSource audioSource;
    public AudioClip hitSound;
    public float hitVolume = 1f;
    private bool soundPlayed = false;      // Para evitar repetir el sonido

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        originalPos = transform.position;
    }

    void Update()
    {
        if (triggered)
            return;

        // Raycast según el ángulo
        float angleRad = rayAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Sin(angleRad), -Mathf.Cos(angleRad));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, playerLayer);
        Debug.DrawRay(transform.position, direction * rayLength, Color.red);

        if (hit.collider != null)
        {
            triggered = true;
            StartCoroutine(ShakeAndFall());
        }
    }

    IEnumerator ShakeAndFall()
    {
        yield return new WaitForSeconds(detectionDelay);

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float currentIntensity = shakeIntensity * (1 - (elapsed / shakeDuration));
            float xOffset = Random.Range(-currentIntensity, currentIntensity);
            transform.position = new Vector3(originalPos.x + xOffset, transform.position.y, transform.position.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
        yield return new WaitForSeconds(fallDelay);

        rb.isKinematic = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si ya sonó, no repetir
        if (soundPlayed) return;
    
        // Verifica si chocó con el layer suelo
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            if (audioSource != null && hitSound != null)
                audioSource.PlayOneShot(hitSound, hitVolume);
    
            soundPlayed = true;
        }
    }

}