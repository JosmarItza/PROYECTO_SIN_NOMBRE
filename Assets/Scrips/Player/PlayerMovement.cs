using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    private float movimiento;

    [Header("Sonido de pasos")]
    public AudioSource stepSource;     // AudioSource con loop
    public AudioClip stepClip;         
    public float stepVolume = 1f;
    public float footstepFadeTime = 0.5f;
    private Coroutine fadeCoroutine;

    [Header("Sonido de Dash")]
    public AudioSource dashSource;     // Nuevo AudioSource solo para el dash
    public AudioClip dashClip;         // Sonido del dash
    public float dashVolume = 1f;      // Volumen del dash

    [Header("Salto")]
    public float fuerzaSalto = 10f;
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;
    private bool enSuelo;

    [Header("Dash")]
    public float dashForce = 20f;     
    public float dashDuration = 0.15f; 
    public float dashCooldown = 0.5f; 
    private bool isDashing = false;
    private bool canDash = true;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    private float originalGravity;

    private PlayerHealth playerHealth;

    [Header("Salto Variable")]
    public float saltoInicial = 10f;
    public float saltoExtra = 20f;
    public float tiempoSaltoMax = 0.2f;
    private float tiempoSalto;
    private bool saltando;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        originalGravity = rb.gravityScale;

        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        HandleFootsteps();

        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isDashing", false);
            return;
        }

        if (!isDashing)
            movimiento = Input.GetAxisRaw("Horizontal");

        if (movimiento > 0)
            sr.flipX = false;
        else if (movimiento < 0)
            sr.flipX = true;

        enSuelo = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo && !isDashing)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(DoDash());
        }

        animator.SetBool("isDashing", isDashing);

        if (!isDashing)
        {
            if (!enSuelo)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isRunning", false);
            }
            else
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isRunning", movimiento != 0);
            }
        }
    }

    void FixedUpdate()
    {
        if (playerHealth != null && playerHealth.currentHealth <= 0)
            return;

        if (!isDashing)
        {
            rb.velocity = new Vector2(movimiento * velocidad, rb.velocity.y);
        }
    }

    IEnumerator DoDash()
    {
        if (playerHealth != null && playerHealth.currentHealth <= 0)
            yield break;

        isDashing = true;
        canDash = false;

        // 🔊 <<< REPRODUCIR SONIDO DE DASH >>>
        if (dashSource != null && dashClip != null)
        {
            dashSource.volume = dashVolume;
            dashSource.PlayOneShot(dashClip);
        }

        float direction = sr.flipX ? -1 : 1;

        rb.gravityScale = 0;
        rb.velocity = new Vector2(direction * dashForce, 0);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void HandleFootsteps()
    {
        bool shouldPlay =
            movimiento != 0 &&
            enSuelo &&
            !isDashing &&
            playerHealth.currentHealth > 0;

        if (shouldPlay)
        {
            if (!stepSource.isPlaying)
            {
                if (fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);

                stepSource.volume = stepVolume; 
                stepSource.clip = stepClip;
                stepSource.Play();
            }
        }
        else
        {
            if (stepSource.isPlaying && fadeCoroutine == null)
            {
                fadeCoroutine = StartCoroutine(FadeOutFootsteps());
            }
        }
    }

    IEnumerator FadeOutFootsteps()
    {
        float startVolume = stepSource.volume;
        float elapsed = 0f;
    
        while (elapsed < footstepFadeTime)
        {
            float t = elapsed / footstepFadeTime;
            stepSource.volume = Mathf.Lerp(startVolume, 0f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
    
        stepSource.volume = 0f;
        stepSource.Stop();
        fadeCoroutine = null;
    }

}