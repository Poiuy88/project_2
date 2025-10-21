using UnityEngine;
using TMPro;

public class StatsPanelUI : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI speedText;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    // Hàm Update được gọi mỗi frame khi Panel được bật
    void Update()
{
    if (playerStats != null)
    {
        levelText.text = "Cấp độ: " + playerStats.playerLevel;
        // Sử dụng các thuộc tính getter để lấy chỉ số tổng
        healthText.text = "Máu: " + playerStats.currentHealth + " / " + playerStats.maxHealth;
        manaText.text = "Năng lượng: " + playerStats.currentMana + " / " + playerStats.maxMana;
        attackText.text = "Tấn công: " + playerStats.attack;
        defenseText.text = "Phòng thủ: " + playerStats.defense;
        speedText.text = "Tốc chạy: " + playerStats.moveSpeed.ToString("F1"); // "F1" để hiển thị 1 chữ số thập phân
    }
}
}