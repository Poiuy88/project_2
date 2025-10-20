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
            healthText.text = "Máu: " + playerStats.currentHealth + " / " + playerStats.maxHealth;
            manaText.text = "Năng lượng: " + playerStats.currentMana + " / " + playerStats.maxMana;

            // Lấy chỉ số tổng (cơ bản + trang bị)
            attackText.text = "Tấn công: " + playerStats.GetTotalAttack();
            // (Bạn có thể thêm hàm GetTotalDefense() tương tự trong PlayerStats)
            defenseText.text = "Phòng thủ: " + playerStats.baseDefense; // Tạm thời hiển thị giáp cơ bản

            speedText.text = "Tốc chạy: " + playerStats.moveSpeed;
        }
    }
}