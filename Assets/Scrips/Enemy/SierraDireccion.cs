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

    [Header("Curva de Movimiento")]
    public AnimationCurve curvaMovimiento = AnimationCurve.Linear(0, 0, 1, 1);

    private Transform objetivoActual;
    private bool esperando = false;
    private float t = 0f; // parámetro de interpolación
    private Vector3 inicioPos;

    void Start()
    {
        objetivoActual = puntoA;
        inicioPos = transform.position;
    }

    void Update()
    {
        if (!esperando)
            MoverConCurva();
    }

    void MoverConCurva()
    {
        t += Time.deltaTime * velocidad;

        // Normalizamos t de 0 a 1 según la distancia
        float distancia = Vector3.Distance(inicioPos, objetivoActual.position);
        float tNormalizado = t / distancia;

        // Aplicamos la curva
        float tCurva = curvaMovimiento.Evaluate(Mathf.Clamp01(tNormalizado));

        // Interpolamos posición
        transform.position = Vector3.Lerp(inicioPos, objetivoActual.position, tCurva);

        if (Vector3.Distance(transform.position, objetivoActual.position) < 0.05f)
        {
            StartCoroutine(EsperarYGirar());
        }
    }

    private IEnumerator EsperarYGirar()
    {
        esperando = true;

        yield return new WaitForSeconds(tiempoEspera);

        // Girar por scale (opcional)
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );

        // Cambiar punto
        objetivoActual = (objetivoActual == puntoA) ? puntoB : puntoA;

        // Reiniciar interpolación
        inicioPos = transform.position;
        t = 0f;
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