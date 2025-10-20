using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Các chỉ số tối đa
    public int maxHealth = 100;
    public int maxMana = 50;
    public int baseAttack = 5; // Sát thương cơ bản
    public int baseDefense = 0; // Phòng thủ cơ bản
    

    // Các chỉ số được cộng thêm từ trang bị
    [Header("Base Stats")]
    public float moveSpeed = 5f;
    private int attackBonus;
    private int defenseBonus;
    [Header("Level & Experience")]
    public int playerLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    [Header("Currency")]
    public int coins = 0;

    // Các chỉ số hiện tại
    [Header("Live Stats")]
    public int currentHealth;
    public int currentMana;


    // Hàm Awake được gọi trước cả hàm Start
    void Awake()
    {
        // Khi game bắt đầu, máu và năng lượng sẽ đầy
        currentHealth = maxHealth;
        currentMana = maxMana;
    }
    // Hàm để lấy tổng sát thương (cơ bản + trang bị)
    public int GetTotalAttack()
    {
        return baseAttack + attackBonus;
    }

    // Hàm để tính toán sát thương nhận vào (có trừ giáp)
    public void TakeDamage(int damage)
    {
        int totalDefense = baseDefense + defenseBonus;
        damage -= totalDefense;
        if (damage < 1) damage = 1; // Luôn nhận ít nhất 1 sát thương

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log("Player took " + damage + " damage. Current Health: " + currentHealth);
    }

    // Hàm để hồi máu
    public void Heal(int amount)
    {
        currentHealth += amount;
        // Ngăn máu hồi vượt quá mức tối đa
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Player healed " + amount + " HP. Current Health: " + currentHealth);
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
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        Debug.Log("Player restored " + amount + " mana. Current Mana: " + currentMana);
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
        playerLevel++; // Tăng level
        currentExp -= expToNextLevel; // Trừ đi EXP đã dùng để lên cấp, giữ lại phần dư

        // Tăng chỉ số cho nhân vật
        maxHealth += 20;
        currentHealth = maxHealth; // Hồi đầy máu khi lên cấp
        maxMana += 10;
        currentMana = maxMana; // Hồi đầy năng lượng

        // Tăng lượng EXP cần cho level tiếp theo (ví dụ: tăng 50%)
        expToNextLevel = (int)(expToNextLevel * 1.5f);

        Debug.Log("<color=yellow>LEVEL UP! Reached Level " + playerLevel + "</color>");
        // Sau này có thể thêm hiệu ứng lên cấp tại đây
    }
    // Hàm để tiêu tiền, trả về true nếu thành công, false nếu không đủ tiền
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
}