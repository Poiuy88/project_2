using UnityEngine;

public class AggroAI : MonoBehaviour
{
    [Header("Movement Stats")]
    public float speed = 3f;
    public float patrolSpeed = 1.5f;
    public float patrolDistance = 4f;
    public float jumpForce = 8f; // Lực nhảy của quái vật
    

    [Header("Detection Points")]
    // public Transform groundCheck; // << Không cần biến Transform này nữa
    public Transform wallCheck;
    // public float checkRadius = 0.1f; // << Biến này không dùng cho Raycast
    public LayerMask whatIsGround;

    // --- THÔNG SỐ RAYCAST MỚI ---
    public float groundCheckDistance = 0.5f; // Độ dài tia dò đất
    public Vector2 groundCheckOffset; // Vị trí tia dò đất
    // --- KẾT THÚC THÔNG SỐ RAYCAST ---

    private bool isGrounded;
    private bool isTouchingWall;

    public enum AIState { Patrolling, Chasing, Returning }
    private AIState currentState = AIState.Patrolling;

    private Transform playerTarget;
    private Vector3 startPosition;
    private Vector3 leftPatrolPoint, rightPatrolPoint;
    private bool movingRight = true;

    private float timeStuck = 0f;
    private Vector3 lastPosition;

    private Rigidbody2D rb;
    private Transform spriteTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteTransform = transform;
        startPosition = transform.position;
        lastPosition = transform.position;
        leftPatrolPoint = startPosition - new Vector3(patrolDistance, 0, 0);
        rightPatrolPoint = startPosition + new Vector3(patrolDistance, 0, 0);
    }
        
    void FixedUpdate()
    {
        // --- SỬ DỤNG RAYCAST ---
        Vector2 raycastOrigin = (Vector2)transform.position + groundCheckOffset;
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundCheckDistance, whatIsGround);
        isGrounded = hit.collider != null;
        // --- KẾT THÚC RAYCAST ---

        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.05f, whatIsGround); // Giữ nguyên kiểm tra tường

        switch (currentState)
        {
            case AIState.Patrolling: Patrol(); break;
            case AIState.Chasing: ChasePlayer(); break;
            case AIState.Returning: ReturnToStart(); break;
        }
    }

    void Patrol()
    {
        // 1. Kiểm tra kẹt
        if (Vector2.Distance(transform.position, lastPosition) < 0.01f) { timeStuck += Time.fixedDeltaTime; }
        else { timeStuck = 0; lastPosition = transform.position; }

        // 2. Xử lý kẹt hoặc gặp tường
        if (timeStuck > 1f || isTouchingWall)
        {
            if (isGrounded) Jump();
            if(timeStuck > 1.5f) { movingRight = !movingRight; Flip(); timeStuck = 0; }
        }

        // 3. Xử lý sắp rơi
        if (!isGrounded && currentState == AIState.Patrolling) { movingRight = !movingRight; Flip(); }

        // 4. Di chuyển
        float targetSpeed = movingRight ? patrolSpeed : -patrolSpeed;
        // Chỉ di chuyển nếu có đất hoặc đang không bị kẹt nghiêm trọng (để cho phép nhảy qua)
        if(isGrounded || timeStuck < 1.5f) rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
        else rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Dừng lại nếu không có đất và đang không xử lý kẹt


        // 5. Quay đầu cuối đường
        if ((movingRight && transform.position.x >= rightPatrolPoint.x) || (!movingRight && transform.position.x <= leftPatrolPoint.x))
        { movingRight = !movingRight; Flip(); }
    }

    void ChasePlayer()
    {
        if (playerTarget == null) { currentState = AIState.Returning; return; }
        if ((isTouchingWall || !isGrounded) && rb.linearVelocity.y == 0) Jump();

        float direction = playerTarget.position.x > transform.position.x ? 1 : -1;
         // Chỉ di chuyển nếu có đất hoặc đang không ở trên không (để cho phép nhảy đuổi theo)
        if(isGrounded || rb.linearVelocity.y != 0) rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        else rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Dừng lại nếu sắp rơi

        FlipTowards(playerTarget.position);
    }
    void ReturnToStart()
    {
        if (isGrounded || rb.linearVelocity.y != 0) // Cho phép nhảy về nếu cần
        {
            float direction = startPosition.x > transform.position.x ? 1 : -1;
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
            FlipTowards(startPosition);
             // Nhảy nếu gặp tường khi quay về
            if (isTouchingWall && isGrounded) Jump();
        }
        else { rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); }


        if (Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(startPosition.x, 0)) < 0.5f) // Chỉ kiểm tra khoảng cách X
        { currentState = AIState.Patrolling; }
    }
    void Jump()
    {
        if (isGrounded) rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse); 
    }
    // --- CÁC HÀM PHỤ TRỢ ---

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) { playerTarget = other.transform; currentState = AIState.Chasing; } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) { playerTarget = null; currentState = AIState.Returning; } 
    }

    // Hàm lật hướng tự động khi tuần tra
    void Flip()
    { 
        Vector3 scaler = spriteTransform.localScale; scaler.x *= -1; spriteTransform.localScale = scaler; 
    }

    // Hàm lật hướng theo mục tiêu
    void FlipTowards(Vector3 targetPosition)
    {
        if (transform.position.x < targetPosition.x && spriteTransform.localScale.x < 0) Flip();
        else if (transform.position.x > targetPosition.x && spriteTransform.localScale.x > 0) Flip();
    }
    public AIState GetCurrentState()
    {
        return currentState;
    }
    public Transform GetPlayerTarget()
    {
        return playerTarget;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 raycastOrigin = (Vector2)transform.position + groundCheckOffset;
        Gizmos.DrawLine(raycastOrigin, raycastOrigin + Vector2.down * groundCheckDistance);

         if(wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, 0.1f);
        }
    }
           
}