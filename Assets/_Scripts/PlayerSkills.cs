// using UnityEngine;

// public class PlayerSkills : MonoBehaviour
// {
//     public GameObject fireballPrefab; // Prefab quả cầu lửa
//     public Transform firePoint; // Điểm bắn ra quả cầu lửa
//     public int manaCost = 1; // Năng lượng tiêu tốn

//     private PlayerStats playerStats;
//     private PlayerController playerController;

//     void Start()
//     {
//         playerStats = GetComponent<PlayerStats>();
//         playerController = GetComponent<PlayerController>(); // (Chúng ta sẽ sửa PlayerController một chút)
//     }

//     void Update()
//     {
//         // Nếu người chơi đã học chiêu VÀ nhấn chuột phải VÀ đủ năng lượng
//         if (GameManager.hasLearnedFireball && Input.GetMouseButtonDown(1) && playerStats.currentMana >= manaCost)
//         {
//             CastFireball();
//         }
//     }

//     void CastFireball()
//     {
//         // Trừ năng lượng
//         playerStats.UseMana(manaCost);

//         // Lấy hướng của nhân vật để bắn cho đúng
//         float angle = playerController.IsFacingRight() ? 0f : 180f;
//         Quaternion rotation = Quaternion.Euler(0, 0, angle);

//         // Tạo ra quả cầu lửa tại điểm bắn với hướng bắn chính xác
//         Instantiate(fireballPrefab, firePoint.position, rotation);
//     }
// }
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [Header("Fireball Skill")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public int manaCost = 15;
    public float fireballCooldown = 5f; // Thời gian hồi chiêu

    // Biến private để theo dõi thời gian hồi chiêu
    private float currentCooldown = 0f;

    private PlayerStats playerStats;
    private PlayerController playerController;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Liên tục giảm thời gian hồi chiêu
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    // Hàm này bây giờ là public để nút có thể gọi
    public void CastFireball()
    {
        // Kiểm tra tất cả các điều kiện: đã học, hết hồi chiêu, đủ năng lượng
        if (GameManager.hasLearnedFireball && currentCooldown <= 0 && playerStats.currentMana >= manaCost)
        {
            playerStats.UseMana(manaCost);

            float angle = playerController.IsFacingRight() ? 0f : 180f;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Instantiate(fireballPrefab, firePoint.position, rotation);

            // Đặt lại thời gian hồi chiêu
            currentCooldown = fireballCooldown;
        }
    }

    // Hàm public để UI có thể lấy thông tin
    public float GetCurrentCooldown() { return currentCooldown; }
}