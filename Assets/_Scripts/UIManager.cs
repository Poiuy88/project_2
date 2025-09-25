using UnityEngine;
using UnityEngine.UI; // Thư viện để làm việc với các component UI như Slider
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public Slider healthBar;
    public Slider manaBar;
    public Slider expBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI coinText;

    // Update được gọi mỗi khung hình
    void Update()
    {
        // Đảm bảo các đối tượng đã được gán trước khi cập nhật
        if (playerStats != null && healthBar != null && manaBar != null)
        {
            // Cập nhật giá trị tối đa của thanh slider (phòng khi maxHP/maxMP thay đổi)
            healthBar.maxValue = playerStats.maxHealth;
            manaBar.maxValue = playerStats.maxMana;

            // Cập nhật giá trị hiện tại của thanh slider
            healthBar.value = playerStats.currentHealth;
            manaBar.value = playerStats.currentMana;
        }
        if (playerStats != null && expBar != null && levelText != null && coinText != null)
        {
            expBar.maxValue = playerStats.expToNextLevel;
            expBar.value = playerStats.currentExp;

            levelText.text = "Level: " + playerStats.playerLevel;
            coinText.text = "Coins: " + playerStats.coins.ToString(); // .ToString() để chuyển số thành chữ
        }
    }
}