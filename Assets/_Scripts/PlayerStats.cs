using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Các chỉ số tối đa
    public int maxHealth = 100;
    public int maxMana = 50;

    // Các chỉ số hiện tại
    public int currentHealth;
    public int currentMana;

    // Hàm Awake được gọi trước cả hàm Start
    void Awake()
    {
        // Khi game bắt đầu, máu và năng lượng sẽ đầy
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    // Hàm để nhận sát thương
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // Ngăn máu giảm xuống dưới 0
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        Debug.Log("Player took " + damage + " damage. Current Health: " + currentHealth);
        // Sau này chúng ta sẽ xử lý việc nhân vật chết khi máu về 0 ở đây
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
}