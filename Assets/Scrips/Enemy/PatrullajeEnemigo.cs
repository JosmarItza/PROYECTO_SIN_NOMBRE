using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PatrullajeEnemigo : MonoBehaviour
{
    [Header("Puntos de Patrulla")]
    public Transform puntoA;
    public Transform puntoB;

    [Header("Movimiento")]
    public float velocidad = 2f;
    public float tiempoEspera = 1f;

    private Transform objetivoActual;
    private Animator animator;

    private bool esperando = false;

    void Start()
    {
        objetivoActual = puntoA;

        // Buscar Animator aunque esté en un hijo
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!esperando)
            Patrullar();
    }

    void Patrullar()
    {
        // Activar animación de caminar
        animator.SetBool("isMoving", true);

        // Movimiento hacia punto
        transform.position = Vector2.MoveTowards(
            transform.position,
            objetivoActual.position,
            velocidad * Time.deltaTime
        );

        // Si llegó
        if (Vector2.Distance(transform.position, objetivoActual.position) < 0.05f)
        {
            StartCoroutine(EsperarYGirar());
        }
    }

    private System.Collections.IEnumerator EsperarYGirar()
    {
        esperando = true;

        // Activar animación Idle
        animator.SetBool("isMoving", false);

        // Esperar quieto
        yield return new WaitForSeconds(tiempoEspera);

        // 🔄 Girar por escala (no con flipX)
        transform.localScale = new Vector3(
            -transform.localScale.x, 
            transform.localScale.y, 
            transform.localScale.z
        );

        // Cambiar punto de destino
        objetivoActual = (objetivoActual == puntoA) ? puntoB : puntoA;

        esperando = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoA != null && puntoB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(puntoA.position, puntoB.position);
        }
    }
}
