using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Biến để điều chỉnh tốc độ di chuyển của nhân vật từ Inspector
    public float moveSpeed = 5f; // Biến để lưu trữ thành phần Rigidbody 2D của nhân vật
    private Rigidbody2D rb; // Biến để lưu trữ hướng di chuyển
    private float moveInput; // Biến mới để tham chiếu đến Animator
    private Animator anim;
    private bool facingRight = true; // Biến để theo dõi hướng của nhân vật

    // Hàm Start được gọi một lần khi game bắt đầu
    void Start()
    {
        // Lấy và lưu lại component Rigidbody 2D gắn trên cùng đối tượng Player
        rb = GetComponent<Rigidbody2D>();
        // Lấy component Animator khi bắt đầu
        anim = GetComponent<Animator>();
    }

    // Hàm Update được gọi mỗi khung hình (frame)
    void Update()
    {
        // Đọc dữ liệu đầu vào từ trục ngang (phím A/D hoặc mũi tên trái/phải)
        // Giá trị sẽ là -1 (trái), 1 (phải), hoặc 0 (đứng yên)
        moveInput = Input.GetAxisRaw("Horizontal");
        // Gửi giá trị tuyệt đối của tốc độ di chuyển cho Animator
        // Mathf.Abs() để đảm bảo giá trị luôn dương (vì tốc độ không thể âm)
        anim.SetFloat("speed", Mathf.Abs(moveInput));

        // Kiểm tra để lật hướng nhân vật
        CheckDirectionToFace();
    }

    // Hàm FixedUpdate được gọi theo một chu kỳ thời gian cố định, tốt cho việc xử lý vật lý
    void FixedUpdate()
    {
        // Di chuyển nhân vật bằng cách thay đổi vận tốc của Rigidbody
        // Vận tốc (velocity) = hướng di chuyển * tốc độ
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
    void CheckDirectionToFace()
    {
        // Nếu di chuyển sang trái (moveInput < 0) và đang quay mặt sang phải
        if (moveInput < 0 && facingRight)
        {
            Flip();
        }
        // Nếu di chuyển sang phải (moveInput > 0) và đang quay mặt sang trái
        else if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
    }
    void Flip()
    {
        // Đảo ngược trạng thái quay mặt
        facingRight = !facingRight;
        // Lấy scale hiện tại
        Vector3 scaler = transform.localScale;
        // Đảo ngược scale trục X
        scaler.x *= -1;
        // Gán lại scale mới
        transform.localScale = scaler;
    }   
}