using UnityEngine;

public class FlyingAggroAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 4f;
    public float patrolSpeed = 2f;
    public float patrolDistance = 6f;

    [Header("Detection")]
    public float detectRadius = 8f;

    public enum AIState { Patrolling, Chasing, Returning }
    private AIState currentState = AIState.Patrolling;

    private Transform playerTarget;
    private Vector3 startPosition;
    private Vector3 leftPatrolPoint, rightPatrolPoint;
    private bool movingRight = true;
    private float moveAmount = 0f;

    void Start()
    {
        startPosition = transform.position;
        leftPatrolPoint = startPosition - new Vector3(patrolDistance, 0, 0);
        rightPatrolPoint = startPosition + new Vector3(patrolDistance, 0, 0);
    }

    void Update()
    {
        // DI CHUYỂN BẰNG TRANSFORM - ỔN ĐỊNH 100%
        switch (currentState)
        {
            case AIState.Patrolling: Patrol(); break;
            case AIState.Chasing: ChasePlayer(); break;
            case AIState.Returning: ReturnToStart(); break;
        }
    }

    void Patrol()
    {
        moveAmount = movingRight ? patrolSpeed : -patrolSpeed;
        transform.Translate(Vector2.right * moveAmount * Time.deltaTime, Space.World);

        // Quay đầu cuối đường
        if ((movingRight && transform.position.x >= rightPatrolPoint.x) ||
            (!movingRight && transform.position.x <= leftPatrolPoint.x))
        {
            movingRight = !movingRight;
            Flip();
        }
    }

    void ChasePlayer()
    {
        if (playerTarget == null) { currentState = AIState.Returning; return; }

        float direction = playerTarget.position.x > transform.position.x ? 1f : -1f;
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime, Space.World);
        FlipTowards(playerTarget.position);
    }

    void ReturnToStart()
    {
        float direction = startPosition.x > transform.position.x ? 1f : -1f;
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime, Space.World);
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

    // DETECT PLAYER BẰNG OVERLAPCIRCLE
    void FixedUpdate()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectRadius, LayerMask.GetMask("Player"));
        if (player != null)
        {
            playerTarget = player.transform;
            currentState = AIState.Chasing;
        }
        else if (currentState == AIState.Chasing)
        {
            playerTarget = null;
            currentState = AIState.Returning;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}