using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int expValue = 25; // Lượng EXP nhận được khi tiêu diệt
    public int coinValue = 10; // Lượng tiền nhận được
    public int maxHealth = 30;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took " + damage + " damage. Current health: " + currentHealth);

        // Xử lý khi quái vật bị tiêu diệt
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Tìm đối tượng người chơi bằng Tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // Nếu tìm thấy người chơi
        if (player != null)
        {
            // Lấy component PlayerStats và gọi các hàm trao thưởng
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.AddExperience(expValue);
                playerStats.AddCoins(coinValue);
            }
        }

        // Hủy đối tượng quái vật
        Destroy(gameObject);
    }
}