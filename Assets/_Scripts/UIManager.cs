using UnityEngine;
using UnityEngine.UI; // Thư viện để làm việc với các component UI như Slider

public class UIManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public Slider healthBar;
    public Slider manaBar;

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
    }
}