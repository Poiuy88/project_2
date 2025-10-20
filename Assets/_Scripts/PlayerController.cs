using UnityEngine;
using UnityEngine.SceneManagement; // << THÊM THƯ VIỆN NÀY

public class PlayerController : MonoBehaviour
{
    // --- CÁC BIẾN CỦA BẠN (GIỮ NGUYÊN) ---
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    private float moveInput;
    private bool facingRight = true;
    public float jumpForce = 12f;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;
    private Animator anim;


    // --- HÀM AWAKE() CŨ SẼ BỊ XÓA VÀ THAY BẰNG LOGIC MỚI NÀY ---

    // Hàm Awake bây giờ chỉ dùng để lấy component
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
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
    }


    // --- CÁC HÀM CÒN LẠI (UPDATE, FIXEDUPDATE, FLIP...) GIỮ NGUYÊN ---
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if (isGrounded == true && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speed", Mathf.Abs(moveInput));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        CheckDirectionToFace();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * playerStats.moveSpeed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
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

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
    public bool IsFacingRight()
    {
        return facingRight;
    }
}