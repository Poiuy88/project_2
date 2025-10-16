// using UnityEngine;

// public class EnemyDamage : MonoBehaviour
// {
//     public int damageAmount = 10; // Sát thương gây ra
//     public float damageCooldown = 1.5f; // Thời gian chờ giữa mỗi lần gây sát thương
//     private float lastDamageTime;

//     // Dùng OnCollisionEnter2D cho va chạm vật lý
//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         // Kiểm tra xem có phải là Player không và đã hết thời gian chờ chưa
//         if (collision.gameObject.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
//         {
//             // Lấy component PlayerStats từ đối tượng va chạm
//             PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
//             if (playerStats != null)
//             {
//                 playerStats.TakeDamage(damageAmount);
//                 lastDamageTime = Time.time; // Cập nhật lại mốc thời gian gây sát thương
//             }
//         }
//     }
// }
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 10;
    public float damageCooldown = 1.5f;
    private float lastDamageTime;

    // Đổi từ OnCollisionEnter2D sang OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem có phải là Player không và đã hết thời gian chờ chưa
        if (other.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }
}