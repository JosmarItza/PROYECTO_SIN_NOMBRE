using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
[Header("Prefab a spawnear")]
    public GameObject prefab;

    [Header("Puntos de movimiento")]
    public Transform pointA;
    public Transform pointB;

    [Header("Opciones de Spawn")]
    public float spawnTime = 1.5f; 
    public float moveSpeed = 3f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    void SpawnObject()
    {
        // Crear objeto
        GameObject obj = Instantiate(prefab, pointA.position, Quaternion.identity);

        // Añadir componente que lo mueve A→B
        MovingPiece movement = obj.AddComponent<MovingPiece>();
        movement.target = pointB.position;
        movement.speed = moveSpeed;
    }
}

public class MovingPiece : MonoBehaviour
{
    [HideInInspector] public Vector3 target;
    [HideInInspector] public float speed;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
            Destroy(gameObject);
    }
}
