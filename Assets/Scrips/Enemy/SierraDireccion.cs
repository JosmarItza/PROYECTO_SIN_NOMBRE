using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SierraDireccion : MonoBehaviour
{
    [Header("Puntos de Movimiento")]
    public Transform puntoA;
    public Transform puntoB;

    [Header("Movimiento")]
    public float velocidad = 4f;
    public float tiempoEspera = 0f; // si la sierra NO debe parar, pon 0

    private Transform objetivoActual;
    private bool esperando = false;

    void Start()
    {
        objetivoActual = puntoA;
    }

    void Update()
    {
        if (!esperando)
            Mover();
    }

    void Mover()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            objetivoActual.position,
            velocidad * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, objetivoActual.position) < 0.05f)
        {
            StartCoroutine(EsperarYGirar());
        }
    }

    private System.Collections.IEnumerator EsperarYGirar()
    {
        esperando = true;

        // Si quieres que se quede quieta unos segundos
        yield return new WaitForSeconds(tiempoEspera);

        // Girar por scale (opcional)
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );

        // Cambiar el punto al que va
        objetivoActual = (objetivoActual == puntoA) ? puntoB : puntoA;

        esperando = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoA != null && puntoB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(puntoA.position, puntoB.position);
        }
    }
}
