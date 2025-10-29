using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemActionPanel : MonoBehaviour
{
    // Các biến private
    private GameObject panel;
    private Image itemIcon;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI statsText;
    private Button equipButton;
    private Button unequipButton;
    private Button closeButton;
    private Button useButton; // <-- THÊM BIẾN CHO NÚT MỚI

    private ItemData currentItem;
    private bool isInitialized = false;

    // Hàm này sẽ tìm các component và gán sự kiện
    void Initialize()
    {
        if (isInitialized) return;

        Debug.Log("<color=cyan>ItemActionPanel is Initializing...</color>");

        panel = gameObject; 
        
        // Tìm các thành phần con
        itemIcon = transform.Find("ItemIcon").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        statsText = transform.Find("StatsText").GetComponent<TextMeshProUGUI>();
        equipButton = transform.Find("EquipButton").GetComponent<Button>();
        unequipButton = transform.Find("UnequipButton").GetComponent<Button>();
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        useButton = transform.Find("UseButton").GetComponent<Button>(); // <-- TÌM NÚT "UseButton"

        // Gán sự kiện (Add Listeners)
        if (equipButton != null) equipButton.onClick.AddListener(EquipItem);
        if (unequipButton != null) unequipButton.onClick.AddListener(UnequipItem);
        if (closeButton != null) closeButton.onClick.AddListener(ClosePanel);
        if (useButton != null) useButton.onClick.AddListener(UseCurrentItem); // <-- GÁN SỰ KIỆN CHO NÚT MỚI
        
        isInitialized = true;
    }

    // --- HÀM NÀY ĐƯỢC NÂNG CẤP HOÀN TOÀN ---
    public void OpenPanelForInventoryItem(ItemData item)
    {
        Initialize(); 

        currentItem = item;
        panel.SetActive(true);

        itemIcon.sprite = item.icon; 
        nameText.text = item.itemName;
        
        // Mặc định ẩn tất cả các nút hành động
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);
        useButton.gameObject.SetActive(false);

        // Kiểm tra loại vật phẩm để hiện nút tương ứng
        if (item.itemType == ItemType.Equipment)
        {
            statsText.text = "Tấn công: " + item.attackBonus + "\nPhòng thủ: " + item.defenseBonus;
            equipButton.gameObject.SetActive(true); // Hiện nút Trang bị
        }
        else if (item.itemType == ItemType.Consumable)
        {
            statsText.text = "Hồi " + item.healthRestore + " Máu\nHồi " + item.manaRestore + " Năng lượng";
            useButton.gameObject.SetActive(true); // Hiện nút Sử dụng
        }
        else // Vật phẩm loại khác (Material...)
        {
            statsText.text = item.description; // Hiện mô tả
        }
    }

    public void OpenPanelForEquipmentItem(ItemData item)
    {
        Initialize(); 

        currentItem = item;
        panel.SetActive(true);

        itemIcon.sprite = item.icon;
        nameText.text = item.itemName;
        statsText.text = "Tấn công: " + item.attackBonus + "\nPhòng thủ: " + item.defenseBonus;

        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(true); // Chỉ hiện nút Tháo gỡ
        useButton.gameObject.SetActive(false);
    }

    // --- HÀM MỚI ĐỂ SỬ DỤNG VẬT PHẨM ---
    void UseCurrentItem()
    {
        if (currentItem != null)
        {
            // Gọi đến hàm UseItem trong túi đồ
            PlayerInventory.instance.UseItem(currentItem);
        }
        ClosePanel();
    }

    // Các hàm còn lại
    void EquipItem()
    {
        if (currentItem.itemType == ItemType.Equipment)
        {
            EquipmentManager.instance.Equip(currentItem);
        }
        ClosePanel();
    }

    void UnequipItem()
    {
        EquipmentManager.instance.Unequip((int)currentItem.equipSlot);
        ClosePanel();
    }
    
    public void ClosePanel()
    {
        if (panel != null) 
        {
            panel.SetActive(false);
        }
    }
}