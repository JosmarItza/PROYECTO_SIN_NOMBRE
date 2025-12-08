using System.Collections;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    public float targetAngle = 20f;
    public float rotationTime = 0.5f;

    [Header("Movimiento Vertical")]
    public float upHeight = 1f;
    public float downHeight = -1f;

    [Header("Curvas de Easing")]
    public AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve fallCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("General")]
    public float destroyDelay = 0.1f;
    public float fadeOutTime = 0.3f;

    [Header("Curación")]
    public int healAmount = 1; // 🔥 CUÁNTOS PUNTOS DE VIDA AGREGA

    private bool triggered = false;
    private Animator anim;
    private SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
    
        if (other.CompareTag("Player"))
        {    
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.Heal(healAmount);
            }
        }
    
        triggered = true;
    
        if (anim != null)
            anim.enabled = false;
    
        StartCoroutine(RotateAndFall());
    }

    IEnumerator RotateAndFall()
    {
        float elapsed = 0f;

        float startAngle = 0f;
        float endAngle = targetAngle;

        Vector3 startPos = transform.position;
        Vector3 upPos = startPos + Vector3.up * upHeight;

        while (elapsed < rotationTime)
        {
            float t = elapsed / rotationTime;
            float eval = rotationCurve.Evaluate(t);

            float angle = Mathf.Lerp(startAngle, endAngle, eval);
            transform.rotation = Quaternion.Euler(0, 0, angle);

            transform.position = Vector3.Lerp(startPos, upPos, eval);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, endAngle);
        transform.position = upPos;

        Vector3 endPos = startPos + Vector3.up * downHeight;

        float fallTime = 0.3f;
        float fallElapsed = 0f;

        float fadeElapsed = 0f;
        Color originalColor = sr != null ? sr.color : Color.white;

        while (fallElapsed < fallTime)
        {
            float tFall = fallElapsed / fallTime;
            float evalFall = fallCurve.Evaluate(tFall);

            transform.position = Vector3.Lerp(upPos, endPos, evalFall);

            if (sr != null)
            {
                float tFade = Mathf.Clamp01(fadeElapsed / fadeOutTime);
                sr.color = new Color(
                    originalColor.r,
                    originalColor.g,
                    originalColor.b,
                    Mathf.Lerp(1f, 0f, tFade)
                );
                fadeElapsed += Time.deltaTime;
            }

            fallElapsed += Time.deltaTime;
            yield return null;
        }

        if (sr != null)
        {
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }

        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}