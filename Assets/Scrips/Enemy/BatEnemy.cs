using System.Collections;
using UnityEngine;

public class BatEnemy : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public float detectionRange = 5f;

    [Header("Zona de patrullaje")]
    public GameObject zoneCenter;
    public Vector2 zoneSize = new Vector2(5f, 3f);
    public float moveSpeed = 2f;

    private float normalSpeed;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private Vector2 targetPosition;

    [Header("Animaciones")]
    public float attackDelay = 1f;
    private Animator anim;

    private bool playerDetected = false;
    private bool isFollowingPlayer = false;
    private bool canFollow = true;

    [Header("Seguir jugador")]
    public float followCooldown = 0.3f;
    public float stopDistance = 0.5f;

    void Start()
    {
        anim = GetComponent<Animator>();
        transform.localScale = new Vector3(-1, 1, 1);

        normalSpeed = moveSpeed;

        if (zoneCenter != null)
        {
            Vector2 center = zoneCenter.transform.position;
            minBounds = center - zoneSize / 2f;
            maxBounds = center + zoneSize / 2f;
        }

        SetRandomTarget();
        StartCoroutine(PatrolInsideZone());
    }

    void Update()
    {
        if (player == null) return;

        // Si el jugador está muerto, no hacer nada
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            anim.SetBool("attack", false);
            anim.SetBool("isMoving", false);
            return;
        }

        DetectPlayer();

        if (!playerDetected)
            FlipTowardsTarget();
        else if (isFollowingPlayer)
            FlipTowardsPlayerDirect();

        if (isFollowingPlayer && canFollow)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer > stopDistance)
                MoveTowards(player.position);
            else
                StartCoroutine(FollowCooldown());
        }
    }

    void DetectPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (!playerDetected && distance < detectionRange)
        {
            playerDetected = true;
            StopCoroutine(PatrolInsideZone());
            StartCoroutine(AttackThenMove());
        }
        else if (playerDetected && distance > detectionRange)
        {
            playerDetected = false;
            isFollowingPlayer = false;
            moveSpeed = normalSpeed;
            SetRandomTarget();
            StartCoroutine(PatrolInsideZone());
        }
    }

    IEnumerator PatrolInsideZone()
    {
        while (!playerDetected)
        {
            MoveTowards(targetPosition);
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
                SetRandomTarget();
            yield return null;
        }
    }

    void SetRandomTarget()
    {
        float x = Random.Range(minBounds.x, maxBounds.x);
        float y = Random.Range(minBounds.y, maxBounds.y);
        targetPosition = new Vector2(x, y);
    }

    void MoveTowards(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    IEnumerator AttackThenMove()
    {
        // Verificamos vida antes de atacar
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.currentHealth <= 0)
            yield break;

        anim.SetBool("attack", true);
        anim.SetBool("isMoving", false);
        yield return new WaitForSeconds(attackDelay);
        anim.SetBool("attack", false);
        anim.SetBool("isMoving", true);

        moveSpeed += 2f;
        isFollowingPlayer = true;
    }

    IEnumerator FollowCooldown()
    {
        canFollow = false;
        yield return new WaitForSeconds(followCooldown);
        canFollow = true;
    }

    void FlipTowardsTarget()
    {
        if (playerDetected) return;

        float dir = targetPosition.x - transform.position.x;
        if (dir > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (dir < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void FlipTowardsPlayerDirect()
    {
        float dir = player.position.x - transform.position.x;
        if (dir > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (dir < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void OnDrawGizmos()
    {
        if (zoneCenter != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(zoneCenter.transform.position, zoneSize);
        }

        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }

        Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}