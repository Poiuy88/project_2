using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    // --- CÁC BIẾN CỦA BẠN (GIỮ NGUYÊN) ---
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    private float moveInput;
    private bool facingRight = true;
    public float jumpForce = 12f;
    private bool isGrounded;
    private Animator anim;
    [Header("Ground Check Settings")] // Thêm Header để gom nhóm
    public Transform groundCheck; // Vẫn dùng vị trí này làm tâm
    public float checkRadius = 0.5f; // Đổi tên biến này thành groundCheckDistance nếu muốn rõ hơn
    public LayerMask whatIsGround;
    public float footOffset = 0.3f; // Khoảng cách lệch sang hai bên của tia Raycast
    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip runSound;
    private AudioSource audioSource;


    // --- HÀM AWAKE() CŨ SẼ BỊ XÓA VÀ THAY BẰNG LOGIC MỚI NÀY ---

    // Hàm Awake bây giờ chỉ dùng để lấy component
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        audioSource = GetComponent<AudioSource>();
    }

    // Đăng ký lắng nghe sự kiện khi script được bật
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Hủy đăng ký để tránh lỗi
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Hàm này sẽ được gọi mỗi khi một scene mới được tải xong
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("<color=yellow>NEW SCENE LOADED. Checking spawn logic for Player...</color>");

        // Ưu tiên 1: Kiểm tra xem có phải đang trong trạng thái "quay về" không
        if (GameManager.returningToPreviousScene)
        {
            transform.position = GameManager.playerPositionOnExit;
            Debug.Log("<color=lime>Return logic triggered. Player moved to: " + GameManager.playerPositionOnExit + "</color>");
            GameManager.returningToPreviousScene = false; // Reset lá cờ
        }
        // Ưu tiên 2: Nếu không phải "quay về", kiểm tra xem có điểm spawn cố định không
        else if (!string.IsNullOrEmpty(GameManager.nextSpawnPointName))
        {
            Debug.Log("<color=orange>Named spawn logic triggered. Searching for: " + GameManager.nextSpawnPointName + "</color>");
            GameObject spawnPoint = GameObject.Find(GameManager.nextSpawnPointName);
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
            }
            else
            {
                Debug.LogError("FATAL: Spawn point object named '" + GameManager.nextSpawnPointName + "' was NOT FOUND in the scene!");
            }
            GameManager.nextSpawnPointName = null;
        }
        playerStats.ResetPlayerStateAfterDeath();
    }


    // --- CÁC HÀM CÒN LẠI (UPDATE, FIXEDUPDATE, FLIP...) GIỮ NGUYÊN ---
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveInput) > 0.1f && isGrounded)
        {
            // Nếu AudioSource chưa phát gì (hoặc đang phát clip khác)
            if (!audioSource.isPlaying || audioSource.clip != runSound)
            {
                audioSource.clip = runSound; // Đặt clip là tiếng chạy
                audioSource.loop = true;     // Bật lặp lại
                audioSource.Play();
            }
        }
        else
        {
            // Nếu đứng im HOẶC đang nhảy
            // Chỉ tắt nếu nó đang phát đúng âm thanh CHẠY
            if (audioSource.clip == runSound)
            {
                audioSource.Stop();
            }
        }
        if (isGrounded == true && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            if (jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }
        
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speed", Mathf.Abs(moveInput));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        CheckDirectionToFace();
    }

    void FixedUpdate()
    {
        // --- Di chuyển ---
        rb.linearVelocity = new Vector2(moveInput * playerStats.moveSpeed, rb.linearVelocity.y);

        // --- KIỂM TRA MẶT ĐẤT BẰNG HAI TIA RAYCAST ---

        // Tính toán vị trí bắt đầu cho hai tia
        Vector2 raycastOriginLeft = (Vector2)groundCheck.position - new Vector2(footOffset, 0);
        Vector2 raycastOriginRight = (Vector2)groundCheck.position + new Vector2(footOffset, 0);

        // Bắn hai tia Raycast ngắn xuống dưới
        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.down, checkRadius, whatIsGround);
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOriginRight, Vector2.down, checkRadius, whatIsGround);

        // Chỉ cần một trong hai tia chạm đất là coi như đang đứng trên đất
        isGrounded = hitLeft.collider != null || hitRight.collider != null;

        // In ra Console để kiểm tra (có thể xóa sau)
        // Debug.Log("isGrounded (Dual Raycast): " + isGrounded);
    }

    void CheckDirectionToFace()
    {
        if (moveInput < 0 && facingRight) Flip();
        else if (moveInput > 0 && !facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // --- HÀM VẼ GIZMOS ĐỂ DỄ NHÌN ---
    // Thay thế hàm OnDrawGizmosSelected() cũ bằng hàm này
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Vector2 raycastOriginLeft = (Vector2)groundCheck.position - new Vector2(footOffset, 0);
        Vector2 raycastOriginRight = (Vector2)groundCheck.position + new Vector2(footOffset, 0);
        // Vẽ hai đường thẳng biểu thị tia Raycast
        Gizmos.DrawLine(raycastOriginLeft, raycastOriginLeft + Vector2.down * checkRadius);
        Gizmos.DrawLine(raycastOriginRight, raycastOriginRight + Vector2.down * checkRadius);
    }
    public bool IsFacingRight()
    {
        return facingRight;
    }
}