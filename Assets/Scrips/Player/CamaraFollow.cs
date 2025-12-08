using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadCamara = 0.125f;
    public Vector3 desplazamiento;

    // Límites del mapa
    public Vector2 limiteMin;   // esquina inferior izquierda
    public Vector2 limiteMax;   // esquina superior derecha

    private void LateUpdate()
    {
        if (objetivo == null) return;

        // Posición que la cámara QUERRÍA tener
        Vector3 posicionDeseada = objetivo.position + desplazamiento;

        // Limitamos la posición para que no se salga del mapa
        posicionDeseada.x = Mathf.Clamp(posicionDeseada.x, limiteMin.x, limiteMax.x);
        posicionDeseada.y = Mathf.Clamp(posicionDeseada.y, limiteMin.y, limiteMax.y);

        // suavizado
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);

        transform.position = posicionSuavizada;
    }
}
