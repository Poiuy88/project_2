using UnityEngine;
using UnityEngine.UI; // Thư viện UI
using TMPro; // Thư viện TextMeshPro

public class PlayerInventory : MonoBehaviour
{
    public int herbalFruitCount = 0;
    public Button useItemButton; // Tham chiếu đến nút bấm
    public TextMeshProUGUI fruitCountText; // Tham chiếu đến Text để hiện số lượng

    private PlayerStats playerStats; // Tham chiếu đến script chỉ số

    void Start()
    {
        // Lấy component PlayerStats từ cùng một đối tượng Player
        playerStats = GetComponent<PlayerStats>();
        UpdateUI();
    }

    // Hàm này sẽ được gọi bởi cây thuốc
    public void AddHerbalFruit(int amount)
    {
        herbalFruitCount += amount;
        Debug.Log("Đã nhận " + amount + " Quả Thảo Dược. Tổng số: " + herbalFruitCount);
        UpdateUI();
    }

    // Hàm này sẽ được gọi bởi nút bấm
    public void UseHerbalFruit()
    {
        // Chỉ sử dụng được khi có ít nhất 1 quả
        if (herbalFruitCount > 0)
        {
            // Trừ đi một quả
            herbalFruitCount--;

            // Gọi hàm hồi máu và năng lượng từ PlayerStats
            playerStats.Heal(25); // Hồi 25 máu
            playerStats.RestoreMana(10); // Hồi 10 năng lượng

            Debug.Log("Đã dùng 1 Quả Thảo Dược. Còn lại: " + herbalFruitCount);
            UpdateUI();
        }
        else
        {
            Debug.Log("Không có Quả Thảo Dược để sử dụng!");
        }
    }

    // Hàm cập nhật giao diện
    private void UpdateUI()
    {
        // Cập nhật số lượng trên Text
        if (fruitCountText != null)
        {
            fruitCountText.text = "x " + herbalFruitCount;
        }
        // Bật/tắt nút bấm dựa trên số lượng
        if (useItemButton != null)
        {
            useItemButton.interactable = herbalFruitCount > 0;
        }
    }
}