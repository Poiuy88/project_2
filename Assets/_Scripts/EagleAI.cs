using UnityEngine;

public class EagleAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 4f;
    public float patrolSpeed = 2f;
    public float patrolDistance = 6f;

    [Header("Detection")]
    public float detectRadius = 8f;
    public LayerMask playerLayer;

    public enum AIState { Patrolling, Chasing, Returning }
    private AIState currentState = AIState.Patrolling;

    private Transform playerTarget;
    private Vector3 startPosition;
    private Vector3 leftPoint, rightPoint;
    private bool movingRight = true;

    void Start()
    {
        startPosition = transform.position;
        leftPoint = startPosition - Vector3.right * patrolDistance;
        rightPoint = startPosition + Vector3.right * patrolDistance;
    }

    void Update()
    {
        DetectPlayer();

        switch (currentState)
        {
            case AIState.Patrolling: Patrol(); break;
            case AIState.Chasing: Chase(); break;
            case AIState.Returning: ReturnToStart(); break;
        }
    }

    void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRadius, playerLayer);
        if (hit != null && hit.CompareTag("Player"))
        {
            playerTarget = hit.transform;
            currentState = AIState.Chasing;
            Debug.Log("EAGLE CHASING!");
        }
        else if (currentState == AIState.Chasing && playerTarget != null)
        {
            // Player ra khỏi tầm
            if (Vector2.Distance(transform.position, playerTarget.position) > detectRadius + 2f)
            {
                playerTarget = null;
                currentState = AIState.Returning;
                Debug.Log("EAGLE RETURNING!");
            }
        }
    }

    void Patrol()
    {
        float move = movingRight ? patrolSpeed : -patrolSpeed;
        transform.Translate(Vector2.right * move * Time.deltaTime, Space.World);

        if ((movingRight && transform.position.x >= rightPoint.x) ||
            (!movingRight && transform.position.x <= leftPoint.x))
        {
            movingRight = !movingRight;
            Flip();
        }
    }

    void Chase()
    {
        if (playerTarget == null) return;

        Vector2 direction = (playerTarget.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        FlipTowards(playerTarget.position);
    }

    void ReturnToStart()
    {
        Vector2 direction = (startPosition - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        FlipTowards(startPosition);

        if (Vector2.Distance(transform.position, startPosition) < 1f)
        {
            currentState = AIState.Patrolling;
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void FlipTowards(Vector3 target)
    {
        if (transform.position.x < target.x && transform.localScale.x < 0) Flip();
        else if (transform.position.x > target.x && transform.localScale.x > 0) Flip();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}