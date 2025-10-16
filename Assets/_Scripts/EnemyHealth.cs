using System.Collections; // Cần thiết để sử dụng Coroutine
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    public int currentHealth; // << Sửa lại để có thể truy cập từ script khác (nếu cần)

    [Header("Rewards")]
    public int expValue = 25;
    public int coinValue = 10;

    [Header("Respawn Settings")]
    public float respawnTime = 10f; // Thời gian chờ để hồi sinh (10 giây)
    private Vector3 startPosition; // Vị trí ban đầu
    private Quaternion startRotation; // Hướng ban đầu
    private Vector3 startScale; // Kích thước ban đầu

    // Mảng để lưu trữ tất cả các component cần bật/tắt
    private Behaviour[] componentsToDisable;
    private Renderer monsterRenderer;
    private Collider2D[] colliders;

    void Start()
    {
        currentHealth = maxHealth;
        // Lưu lại trạng thái ban đầu khi game bắt đầu
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;

        // Tự động tìm tất cả các component cần thiết
        monsterRenderer = GetComponent<Renderer>();
        colliders = GetComponents<Collider2D>();
        // Tìm các script AI như AggroAI, EnemyPatrol
        componentsToDisable = GetComponents<Behaviour>(); 
    }

    public void TakeDamage(int damage)
    {
        // Không nhận sát thương nếu đã "chết"
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died!");

        // Trao thưởng cho người chơi
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.AddExperience(expValue);
                playerStats.AddCoins(coinValue);
            }
        }

        // Bắt đầu quá trình hồi sinh
        StartCoroutine(RespawnCoroutine());
    }
    // Coroutine để xử lý việc hồi sinh
    IEnumerator RespawnCoroutine()
    {
        // 1. "Vô hiệu hóa" quái vật (làm cho nó biến mất)
        SetMonsterActive(false);

        // 2. Chờ đợi trong một khoảng thời gian
        yield return new WaitForSeconds(respawnTime);

        // 3. "Hồi sinh" quái vật
        Debug.Log("Respawning " + gameObject.name);
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startScale;
        currentHealth = maxHealth;

        // 4. Kích hoạt lại quái vật
        SetMonsterActive(true);
    }
    // Hàm tiện ích để bật/tắt các bộ phận của quái vật
    void SetMonsterActive(bool isActive)
    {
        // Bật/tắt hình ảnh
        if (monsterRenderer != null) monsterRenderer.enabled = isActive;
        
        // Bật/tắt tất cả các collider
        foreach (Collider2D col in colliders)
        {
            col.enabled = isActive;
        }

        // Bật/tắt tất cả các script (bao gồm cả AI, Health, Damage...)
        // Chúng ta trừ 1 để không tắt chính script EnemyHealth này
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            if (componentsToDisable[i] != this) // Không tắt chính mình
            {
                 componentsToDisable[i].enabled = isActive;
            }
        }
    }
}