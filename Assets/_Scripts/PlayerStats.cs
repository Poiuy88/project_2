using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseMoveSpeed = 5f;
    public int baseMaxHealth = 100;
    public int baseMaxMana = 50;
    public int baseAttack = 5;
    public int baseDefense = 0;

    // Biến lưu trữ chỉ số cộng thêm từ trang bị
    private int healthBonus;
    private int manaBonus;
    private int attackBonus;
    private int defenseBonus;
    private float speedBonus;

    [Header("Live Stats")]
    public int currentHealth;
    public int currentMana;
    private bool isDead = false;

    // Các thuộc tính (Getter) để lấy chỉ số tổng cuối cùng
    public int maxHealth { get { return baseMaxHealth + healthBonus; } }
    public int maxMana { get { return baseMaxMana + manaBonus; } }
    public int attack { get { return baseAttack + attackBonus; } }
    public int defense { get { return baseDefense + defenseBonus; } }
    public float moveSpeed { get { return baseMoveSpeed + speedBonus; } }

    [Header("Level & Experience")]
    public int playerLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    [Header("Currency")]
    public int coins = 0;


    // Hàm Awake được gọi trước cả hàm Start
    void Awake()
    {
        // Khởi tạo máu/mana dựa trên chỉ số gốc
        currentHealth = baseMaxHealth;
        currentMana = baseMaxMana;
    }
    // Hàm để lấy tổng sát thương (cơ bản + trang bị)
    public int GetTotalAttack()
    {
        return baseAttack + attackBonus;
    }

    // Hàm để tính toán sát thương nhận vào (có trừ giáp)
    public void TakeDamage(int damage)
    {
        damage -= defense; // Dùng chỉ số phòng thủ tổng
        if (damage < 1) damage = 1;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    // Hàm để hồi máu
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth; // Dùng maxHealth tổng
    }

    // Hàm để sử dụng năng lượng
    public void UseMana(int amount)
    {
        currentMana -= amount;
        if (currentMana < 0)
        {
            currentMana = 0;
        }
        Debug.Log("Player used " + amount + " mana. Current Mana: " + currentMana);
    }

    // Hàm để hồi năng lượng
    public void RestoreMana(int amount)
    {
        currentMana += amount;
        if (currentMana > maxMana) currentMana = maxMana; // Dùng maxMana tổng
    }
    // Hàm để nhận tiền
    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Player received " + amount + " coins. Total: " + coins);
    }

    // Hàm để nhận kinh nghiệm
    public void AddExperience(int expGained)
    {
        currentExp += expGained;
        Debug.Log("Player gained " + expGained + " EXP. Total: " + currentExp);

        // Kiểm tra xem đã đủ EXP để lên cấp chưa
        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    // Hàm xử lý việc lên cấp
    void LevelUp()
    {
        playerLevel++;
        currentExp -= expToNextLevel;

        baseMaxHealth += 20; // Chỉ tăng base
        baseMaxMana += 10;   // Chỉ tăng base
        // Bạn có thể tăng cả baseAttack, baseDefense... ở đây nếu muốn
        currentHealth = maxHealth; // Hồi đầy dựa trên maxHealth mới
        currentMana = maxMana;

        expToNextLevel = (int)(expToNextLevel * 1.5f);
    }
    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            Debug.Log("Spent " + amount + " coins. Remaining: " + coins);
            return true; // Giao dịch thành công
        }
        else
        {
            Debug.Log("Not enough coins! Required: " + amount + ", Have: " + coins);
            return false; // Giao dịch thất bại
        }
    }
    // Hàm được gọi bởi EquipmentManager để cập nhật chỉ số
    public void UpdateEquipmentStats(int attackMod, int defenseMod)
    {
        attackBonus = attackMod;
        defenseBonus = defenseMod;
    }
    public void UpdateEquipmentBonuses(int hp, int mp, int atk, int def, float spd)
    {
        healthBonus = hp;
        manaBonus = mp;
        attackBonus = atk;
        defenseBonus = def;
        speedBonus = spd;

        // Điều chỉnh máu/mana hiện tại nếu max thay đổi
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        currentMana = Mathf.Min(currentMana, maxMana);
    }
    void Die()
    {
        isDead = true; // Đánh dấu là đã chết để tránh gọi hàm này 2 lần
        Debug.Log("Player has died. Respawning at VillageMap.");

        // 1. Đặt tên điểm hồi sinh mà PlayerController sẽ tìm kiếm
        // (Chúng ta sẽ tạo đối tượng này ở Bước 3)
        GameManager.nextSpawnPointName = "SpawnPoint_FromDeath";

        // 2. Tải lại cảnh ngôi làng
        SceneManager.LoadScene("VillageMap");
    }
    public void ResetPlayerStateAfterDeath()
    {
        // 1. Hồi đầy máu và mana
        currentHealth = maxHealth;
        currentMana = maxMana;

        // 2. Reset cờ 'isDead' để người chơi có thể chết lần nữa
        isDead = false;

        Debug.Log("Player state has been reset after respawning.");
    }
}