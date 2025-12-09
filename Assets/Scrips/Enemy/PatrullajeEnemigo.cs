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

    [Header("Sonidos")]
    public AudioClip sonidoCaminar;   // Sonido mientras camina (loop)
    public AudioClip sonidoDetener;   // Sonido al llegar y detenerse
    private AudioSource audioSource;

    private Transform objetivoActual;
    private Animator animator;

    private bool esperando = false;

    void Start()
    {
        objetivoActual = puntoA;

        // Animator en hijos
        animator = GetComponentInChildren<Animator>();

        // AudioSource en este mismo objeto
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    void Update()
    {
        if (!esperando)
            Patrullar();
    }

    void Patrullar()
    {
        animator.SetBool("isMoving", true);

        // 🔊 REPRODUCIR sonido caminar solo si no está ya sonando
        if (!audioSource.isPlaying)
        {
            audioSource.clip = sonidoCaminar;
            audioSource.loop = true;
            audioSource.Play();
        }

        // Movimiento
        transform.position = Vector2.MoveTowards(
            transform.position,
            objetivoActual.position,
            velocidad * Time.deltaTime
        );

        // Llegó al punto
        if (Vector2.Distance(transform.position, objetivoActual.position) < 0.05f)
        {
            StartCoroutine(EsperarYGirar());
        }
    }

    private IEnumerator EsperarYGirar()
    {
        esperando = true;

        // Apagar animación caminar
        animator.SetBool("isMoving", false);

        // 🔊 Parar sonido caminar
        audioSource.Stop();

        // 🔊 Reproducir sonido de detenerse
        audioSource.loop = false;
        audioSource.PlayOneShot(sonidoDetener);

        // Esperar
        yield return new WaitForSeconds(tiempoEspera);

        // Girar
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );

        // Cambiar objetivo
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