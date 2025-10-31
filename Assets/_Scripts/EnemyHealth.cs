using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public int level = 1; // Cấp độ của quái vật
    public int maxHealth = 30;
    public int currentHealth;
    [Header("Cooldown")]
    public float hitCooldown = 0.5f; // Thời gian "bất tử" sau khi bị đánh
    private float lastHitTime;

    [Header("Rewards")]
    public int expValue = 25;
    // public int coinValue = 10; // << Không cần biến này nữa
    public GameObject coinPrefab; // Prefab đồng xu rơi ra

    [Header("Respawn Settings")]
    public float respawnTime = 10f;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

    // Các component cần quản lý
    private Behaviour[] componentsToDisable;
    private Renderer monsterRenderer;
    private Collider2D[] colliders;

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;

        monsterRenderer = GetComponent<Renderer>();
        colliders = GetComponents<Collider2D>();
        // Lấy tất cả các script có thể bật/tắt (bao gồm AI, Damage...)
        componentsToDisable = GetComponents<Behaviour>();
    }

    public void TakeDamage(int damage)
    {
        if (Time.time < lastHitTime + hitCooldown)
        {
            return; // Nếu chưa, không nhận sát thương
        }
        if (currentHealth <= 0) return;
        lastHitTime = Time.time;

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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // Chỉ trao EXP trực tiếp
                playerStats.AddExperience(expValue);

                // --- LOGIC RƠI COIN MỚI ---
                float dropChance = 0.5f; // Tỷ lệ rơi 50%
                if (Random.value <= dropChance && coinPrefab != null) // Thêm kiểm tra coinPrefab != null
                {
                    int coinsToDrop = Random.Range(5 * level, 10 * level + 1);
                    if (coinsToDrop > 0) // Chỉ tạo coin nếu số lượng lớn hơn 0
                    {
                        GameObject coinGO = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                        CoinPickup coinScript = coinGO.GetComponent<CoinPickup>();
                        if (coinScript != null)
                        {
                            coinScript.coinValue = coinsToDrop;
                            Debug.Log($"Dropped {coinsToDrop} coins!");
                        }
                        else
                        {
                            Debug.LogError("Coin Prefab is missing CoinPickup script!", coinGO);
                            Destroy(coinGO); // Hủy coin nếu prefab bị lỗi
                        }
                    }
                }
                // --- KẾT THÚC LOGIC RƠI COIN ---
            }
        }

        // Bắt đầu hồi sinh
        StartCoroutine(RespawnCoroutine()); // << Dòng gây lỗi của bạn sẽ hoạt động lại
    }

    // --- CÁC HÀM HỒI SINH (BỊ THIẾU TRƯỚC ĐÂY) ---
    IEnumerator RespawnCoroutine()
    {
        // Ẩn thanh máu ngay lập tức
        EnemyUIController uiController = GetComponent<EnemyUIController>();
        if (uiController != null)
        {
            uiController.HideHealthBarInstantly();
        }

        // Vô hiệu hóa quái vật
        SetMonsterActive(false);

        // Chờ đợi
        yield return new WaitForSeconds(respawnTime);

        // Hồi sinh
        Debug.Log("Respawning " + gameObject.name);
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startScale;
        currentHealth = maxHealth;

        // Kích hoạt lại
        SetMonsterActive(true);
    }

    void SetMonsterActive(bool isActive)
    {
        if (monsterRenderer != null) monsterRenderer.enabled = isActive;
        foreach (Collider2D col in colliders) { col.enabled = isActive; }

        // Bật/tắt tất cả các script trừ script này
        foreach(Behaviour comp in componentsToDisable)
        {
            if (comp != this) // Đảm bảo không tắt chính script EnemyHealth
            {
                 comp.enabled = isActive;
            }
        }
    }
    // --- KẾT THÚC CÁC HÀM HỒI SINH ---
}