using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    private float movimiento;

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

    private PlayerHealth playerHealth; // Referencia a la vida del jugador

    [Header("Salto Variable")]
    public float saltoInicial = 10f;       // Tu fuerza normal
    public float saltoExtra = 20f;         // Fuerza extra mientras mantienes PRESIONADO Space
    public float tiempoSaltoMax = 0.2f;    // Cuánto tiempo puede seguir aumentando
    private float tiempoSalto;             // Timer interno
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
        // Si la vida es 0, bloquear movimiento y animaciones
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
        // Bloquear movimiento si vida es 0
        if (playerHealth != null && playerHealth.currentHealth <= 0)
            return;

        if (!isDashing)
        {
            rb.velocity = new Vector2(movimiento * velocidad, rb.velocity.y);
        }
    }

    IEnumerator DoDash()
    {
        // Bloquear dash si vida es 0
        if (playerHealth != null && playerHealth.currentHealth <= 0)
            yield break;

        isDashing = true;
        canDash = false;

        float direction = sr.flipX ? -1 : 1;

        rb.gravityScale = 0;
        rb.velocity = new Vector2(direction * dashForce, 0);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}