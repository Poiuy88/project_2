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