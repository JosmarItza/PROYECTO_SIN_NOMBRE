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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
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
        if (!isDashing)
        {
            rb.velocity = new Vector2(movimiento * velocidad, rb.velocity.y);
        }
    }

    IEnumerator DoDash()
    {
        isDashing = true;
        canDash = false;

        float direction = sr.flipX ? -1 : 1;

        
        rb.velocity = new Vector2(direction * dashForce, 0);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}