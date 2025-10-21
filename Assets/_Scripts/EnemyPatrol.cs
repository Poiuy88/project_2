using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement Stats")]
    public float speed = 1.5f; // Tốc độ tuần tra mặc định
    public float patrolDistance = 4f;

    [Header("Ground Detection")]
    public Transform groundCheck; // <<< Ô NÀY SẼ XUẤT HIỆN
    public LayerMask whatIsGround; // <<< Ô NÀY SẼ XUẤT HIỆN
    public float groundCheckDistance = 0.5f; // <<< Ô NÀY SẼ XUẤT HIỆN
    public Vector2 groundCheckOffset; // <<< Ô NÀY SẼ XUẤT HIỆN

    private bool isGrounded;
    private bool movingRight = true;
    private Vector3 startPosition;
    private Vector3 leftPatrolPoint, rightPatrolPoint;

    private Rigidbody2D rb;
    private Transform spriteTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteTransform = transform;
        startPosition = transform.position;
        leftPatrolPoint = startPosition - new Vector3(patrolDistance, 0, 0);
        rightPatrolPoint = startPosition + new Vector3(patrolDistance, 0, 0);
    }

    // Chuyển sang FixedUpdate để xử lý vật lý tốt hơn
    void FixedUpdate()
    {
        // --- KIỂM TRA MẶT ĐẤT BẰNG RAYCAST ---
        Vector2 raycastOrigin = (Vector2)transform.position + groundCheckOffset;
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundCheckDistance, whatIsGround);
        isGrounded = hit.collider != null;

        // --- Logic Tuần tra ---
        Patrol();
    }

    void Patrol()
    {
        // Nếu không có đất phía trước hoặc sắp rơi, quay đầu
        if (!isGrounded)
        {
            movingRight = !movingRight;
            Flip();
        }

        float targetSpeed = movingRight ? speed : -speed;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);

        // Kiểm tra để quay đầu ở cuối đường tuần tra
        if ((movingRight && transform.position.x >= rightPatrolPoint.x) ||
            (!movingRight && transform.position.x <= leftPatrolPoint.x))
        {
            movingRight = !movingRight;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scaler = spriteTransform.localScale;
        scaler.x *= -1;
        spriteTransform.localScale = scaler;
    }

    // Hàm vẽ Gizmos để dễ nhìn
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 raycastOrigin = (Vector2)transform.position + groundCheckOffset;
        Gizmos.DrawLine(raycastOrigin, raycastOrigin + Vector2.down * groundCheckDistance);
    }
}