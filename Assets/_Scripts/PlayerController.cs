// // using UnityEngine;

// // public class PlayerController : MonoBehaviour
// // {
// //     // Biến để điều chỉnh tốc độ di chuyển của nhân vật từ Inspector
// //     public float moveSpeed = 5f; // Biến để lưu trữ thành phần Rigidbody 2D của nhân vật
// //     public float jumpForce = 12f; // Lực nhảy, bạn có thể tùy chỉnh

// //     private bool isGrounded; // Biến kiểm tra xem có chạm đất không
// //     public Transform groundCheck; // Vị trí của điểm kiểm tra mặt đất
// //     public float checkRadius = 0.2f; // Bán kính của vòng tròn kiểm tra
// //     public LayerMask whatIsGround; // LayerMask để xác định đâu là mặt đất
// //     private Rigidbody2D rb; // Biến để lưu trữ hướng di chuyển
// //     private float moveInput; // Biến mới để tham chiếu đến Animator
// //     private Animator anim;
// //     private bool facingRight = true; // Biến để theo dõi hướng của nhân vật

// //     // Hàm Start được gọi một lần khi game bắt đầu
// //     void Start()
// //     {
// //         // Lấy và lưu lại component Rigidbody 2D gắn trên cùng đối tượng Player
// //         rb = GetComponent<Rigidbody2D>();
// //         // Lấy component Animator khi bắt đầu
// //         anim = GetComponent<Animator>();
// //     }

// //     // Hàm Update được gọi mỗi khung hình (frame)
// //     void Update()
// //     {
// //         // Đọc dữ liệu đầu vào từ trục ngang (phím A/D hoặc mũi tên trái/phải)
// //         // Giá trị sẽ là -1 (trái), 1 (phải), hoặc 0 (đứng yên)
// //         moveInput = Input.GetAxisRaw("Horizontal");
// //         // Gửi giá trị tuyệt đối của tốc độ di chuyển cho Animator
// //         // Mathf.Abs() để đảm bảo giá trị luôn dương (vì tốc độ không thể âm)
// //         anim.SetFloat("speed", Mathf.Abs(moveInput));

// //         // Kiểm tra để lật hướng nhân vật
// //         CheckDirectionToFace();
// //     }

// //     // Hàm FixedUpdate được gọi theo một chu kỳ thời gian cố định, tốt cho việc xử lý vật lý
// //     void FixedUpdate()
// //     {
// //         // Di chuyển nhân vật bằng cách thay đổi vận tốc của Rigidbody
// //         // Vận tốc (velocity) = hướng di chuyển * tốc độ
// //         rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
// //     }
// //     void CheckDirectionToFace()
// //     {
// //         // Nếu di chuyển sang trái (moveInput < 0) và đang quay mặt sang phải
// //         if (moveInput < 0 && facingRight)
// //         {
// //             Flip();
// //         }
// //         // Nếu di chuyển sang phải (moveInput > 0) và đang quay mặt sang trái
// //         else if (moveInput > 0 && !facingRight)
// //         {
// //             Flip();
// //         }
// //     }
// //     void Flip()
// //     {
// //         // Đảo ngược trạng thái quay mặt
// //         facingRight = !facingRight;
// //         // Lấy scale hiện tại
// //         Vector3 scaler = transform.localScale;
// //         // Đảo ngược scale trục X
// //         scaler.x *= -1;
// //         // Gán lại scale mới
// //         transform.localScale = scaler;
// //     }   
// // }



// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {
//     // Biến di chuyển
//     public float moveSpeed = 5f;
//     private Rigidbody2D rb;
//     private float moveInput;

//     // Biến lật hướng
//     private bool facingRight = true;

//     // Biến nhảy
//     public float jumpForce = 12f;
//     private bool isGrounded;
//     public Transform groundCheck;
//     public float checkRadius = 0.2f;
//     public LayerMask whatIsGround;

//     // Biến animation
//     private Animator anim;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         anim = GetComponent<Animator>();
//     }

//     void Update()
//     {
//         // --- Input Di chuyển ---
//         moveInput = Input.GetAxisRaw("Horizontal");

//         // --- Input Nhảy ---
//         // Input.GetButtonDown("Jump") mặc định là phím Space (cách)
//         if (isGrounded == true && Input.GetButtonDown("Jump"))
//         {
//             // Dùng velocity để nhảy ngay lập tức, cảm giác sẽ nhạy hơn AddForce
//             rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//         }

//         // --- Gửi thông tin cho Animator ---
//         anim.SetFloat("speed", Mathf.Abs(moveInput));
//         anim.SetBool("isGrounded", isGrounded); // Gửi trạng thái chạm đất
//         anim.SetFloat("yVelocity", rb.linearVelocity.y); // Gửi vận tốc trục Y

//         // --- Kiểm tra để lật hướng nhân vật ---
//         CheckDirectionToFace();
//     }

//     void FixedUpdate()
//     {
//         // --- Xử lý Di chuyển ---
//         rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

//         // --- Xử lý Kiểm tra mặt đất ---
//         // Tạo một vòng tròn vô hình tại vị trí groundCheck để xem nó có chạm vào layer "Ground" không
//         isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
//     }

//     // Hàm lật hướng nhân vật
//     void CheckDirectionToFace()
//     {
//         if (moveInput < 0 && facingRight)
//         {
//             Flip();
//         }
//         else if (moveInput > 0 && !facingRight)
//         {
//             Flip();
//         }
//     }

//     void Flip()
//     {
//         facingRight = !facingRight;
//         Vector3 scaler = transform.localScale;
//         scaler.x *= -1;
//         transform.localScale = scaler;
//     }

//     // Hàm này giúp chúng ta thấy được vòng tròn ground check trong editor
//     private void OnDrawGizmosSelected()
//     {
//         if (groundCheck == null) return;
//         Gizmos.color = Color.yellow;
//         Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
//     }
// }


using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Biến di chuyển
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private float moveInput;

    // Biến lật hướng
    private bool facingRight = true;

    // Biến nhảy
    public float jumpForce = 12f;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    // Biến animation
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Awake()
    {
        Debug.Log("<color=yellow>Player Awake: Checking for spawn point named: " + GameManager.nextSpawnPointName + "</color>");

        // Kiểm tra xem có tên điểm spawn nào được chỉ định không
        if (!string.IsNullOrEmpty(GameManager.nextSpawnPointName))
        {
            // Tìm đối tượng điểm spawn bằng tên
            GameObject spawnPoint = GameObject.Find(GameManager.nextSpawnPointName);
            if (spawnPoint != null)
            {
                // Di chuyển nhân vật đến vị trí của điểm spawn
                transform.position = spawnPoint.transform.position;
                Debug.Log("Player moved to spawn point: " + GameManager.nextSpawnPointName);
            }
            else
            {
                Debug.LogWarning("Spawn point not found: " + GameManager.nextSpawnPointName);
            }

            // Xóa tên điểm spawn để lần sau không dùng lại
            GameManager.nextSpawnPointName = null;
        }
    }
    void Update()
    {
        // --- Input Di chuyển ---
        moveInput = Input.GetAxisRaw("Horizontal");

        // --- Input Nhảy ---
        if (isGrounded == true && Input.GetButtonDown("Jump"))
        {
            // DEBUG: Báo cho chúng ta biết khi nào lệnh nhảy được thực thi
            Debug.Log("<color=green>JUMP COMMAND EXECUTED!</color>"); 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // --- Gửi thông tin cho Animator ---
        // DEBUG: Xem giá trị isGrounded mà Animator nhận được là gì
        Debug.Log("Sending to Animator: isGrounded = " + isGrounded); 
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speed", Mathf.Abs(moveInput));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        // --- Kiểm tra để lật hướng nhân vật ---
        CheckDirectionToFace();
    }

    void FixedUpdate()
    {
        // --- Xử lý Di chuyển ---
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // --- Xử lý Kiểm tra mặt đất ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // DEBUG: Báo cho chúng ta kết quả của việc kiểm tra mặt đất
        Debug.Log("<color=orange>Ground Check Result: isGrounded = " + isGrounded + "</color>"); 
    }

    // ... (Các hàm Flip, CheckDirectionToFace, OnDrawGizmosSelected giữ nguyên) ...
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
}