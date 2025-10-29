using UnityEngine;

public class HitboxDamage : MonoBehaviour
{
    private int damage;
    private PlayerStats playerStats; // Thêm tham chiếu để không phải tìm nhiều lần

    // Dùng Awake để lấy tham chiếu
    void Awake()
    {
        // Chỉ cần tìm PlayerStats 1 lần
        playerStats = GetComponentInParent<PlayerStats>();
    }

    // --- ĐÂY LÀ THAY ĐỔI QUAN TRỌNG ---
    // Đổi 'Start()' thành 'OnEnable()'
    void OnEnable()
    {
        // Hàm này được gọi MỖI KHI hitbox được SetActive(true)
        // Nó sẽ luôn lấy chỉ số tấn công MỚI NHẤT
        if (playerStats != null)
        {
            damage = playerStats.GetTotalAttack();
        }
        else
        {
            Debug.LogError("HitboxDamage: Không tìm thấy PlayerStats!");
            damage = 1; // Đặt sát thương tối thiểu để tránh lỗi
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu va chạm với đối tượng có tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Giờ 'damage' đã là chỉ số mới nhất
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}