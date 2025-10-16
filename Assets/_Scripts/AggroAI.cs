using UnityEngine;

public class AggroAI : MonoBehaviour
{
    [Header("Movement Stats")]
    public float speed = 3f;
    public float patrolSpeed = 1.5f;
    public float patrolDistance = 4f;
    public float jumpForce = 8f; // Lực nhảy của quái vật

    [Header("Detection Points")]
    public Transform groundCheck;
    public Transform wallCheck; // Điểm dò tường mới
    public float checkRadius = 0.1f;
    public LayerMask whatIsGround;

    private bool isGrounded;
    private bool isTouchingWall;

    private enum AIState { Patrolling, Chasing, Returning }
    private AIState currentState = AIState.Patrolling;

    private Transform playerTarget;
    private Vector3 startPosition;
    private Vector3 leftPatrollingPoint, rightPatrollingPoint;
    private bool movingRight = true;
    
    // --- BIẾN MỚI ĐỂ XỬ LÝ KẸT ---
    private float timeStuck = 0f; // Thời gian đã bị kẹt
    private Vector3 lastPosition; // Vị trí ở frame trước

    private Rigidbody2D rb;
    private Transform spriteTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteTransform = transform;
        startPosition = transform.position;
        lastPosition = transform.position;

        leftPatrollingPoint = startPosition - new Vector3(patrolDistance, 0, 0);
        rightPatrollingPoint = startPosition + new Vector3(patrolDistance, 0, 0);
    }
        
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsGround);

        // --- Logic AI chính ---
        switch (currentState)
        {
            case AIState.Patrolling:
                Patrol();
                break;
            case AIState.Chasing:
                ChasePlayer();
                break;
            case AIState.Returning:
                ReturnToStart();
                break;
        }
    }
    // void Update()
    // {
    //     // --- SỬ DỤNG RAYCAST ĐỂ KIỂM TRA MẶT ĐẤT ---
    //     Vector2 raycastOrigin = (Vector2)transform.position + groundCheckOffset;
    //     RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundCheckDistance, whatIsGround);

    //     // Nếu tia laser chạm vào thứ gì đó, isGrounded là true
    //     isGrounded = hit.collider != null;

    //     // --- Logic AI giữ nguyên ---
    //     switch (currentState)
    //     {
    //         case AIState.Patrolling: Patrol(); break;
    //         case AIState.Chasing: ChasePlayer(); break;
    //         case AIState.Returning: ReturnToStart(); break;
    //     }
    // }

    // --- CÁC HÀNH VI CỦA AI ---

    void Patrol()
    {
        // 1. Kiểm tra xem có bị kẹt không
        if (Vector2.Distance(transform.position, lastPosition) < 0.01f)
        {
            timeStuck += Time.fixedDeltaTime;
        }
        else
        {
            timeStuck = 0;
            lastPosition = transform.position;
        }

        // 2. Nếu bị kẹt quá 1 giây hoặc gặp tường, hãy thử nhảy
        if (timeStuck > 1f || isTouchingWall)
        {
            if (isGrounded)
            {
                Jump();
            }
            // Nếu nhảy mà vẫn kẹt, thì quay đầu
            if(timeStuck > 1.5f)
            {
                movingRight = !movingRight;
                Flip();
                timeStuck = 0;
            }
        }
        
        // 3. Nếu sắp rơi xuống vực, quay đầu
        if (!isGrounded && currentState == AIState.Patrolling)
        {
            movingRight = !movingRight;
            Flip();
        }

        // 4. Di chuyển tuần tra
        float targetSpeed = movingRight ? patrolSpeed : -patrolSpeed;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);

        // 5. Kiểm tra để quay đầu ở cuối đường tuần tra
        if ((movingRight && transform.position.x >= rightPatrollingPoint.x) || 
            (!movingRight && transform.position.x <= leftPatrollingPoint.x))
        {
            movingRight = !movingRight;
            Flip();
        }
    }

    void ChasePlayer()
    {
        if (playerTarget == null)
        {
            currentState = AIState.Returning;
            return;
        }

        // Nếu gặp tường hoặc sắp rơi, nhảy lên để đuổi theo
        if ((isTouchingWall || !isGrounded) && rb.velocity.y == 0)
        {
            Jump();
        }

        // Di chuyển về phía người chơi
        float direction = playerTarget.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        FlipTowards(playerTarget.position);
    }

    void ReturnToStart()
    {
        // Logic quay về vị trí cũ (có thể nâng cấp tương tự)
        float direction = startPosition.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        FlipTowards(startPosition);

        if (Vector2.Distance(transform.position, startPosition) < 0.5f)
        {
            currentState = AIState.Patrolling;
        }
    }
    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }
    // --- CÁC HÀM PHỤ TRỢ ---

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = other.transform;
            currentState = AIState.Chasing;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = null;
            currentState = AIState.Returning;
        }
    }

    // Hàm lật hướng tự động khi tuần tra
    void Flip()
    {
        Vector3 scaler = spriteTransform.localScale;
        scaler.x *= -1;
        spriteTransform.localScale = scaler;
    }

    // Hàm lật hướng theo mục tiêu
    void FlipTowards(Vector3 targetPosition)
    {
        if (transform.position.x < targetPosition.x)
        {
            spriteTransform.localScale = new Vector3(Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
        }
        else if (transform.position.x > targetPosition.x)
        {
            spriteTransform.localScale = new Vector3(-Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
        }
    }
    
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Vector2 raycastOrigin = (Vector2)transform.position + groundCheckOffset;
    //     Gizmos.DrawLine(raycastOrigin, raycastOrigin + Vector2.down * groundCheckDistance);
    // }
}