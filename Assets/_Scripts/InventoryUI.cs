using UnityEngine;
using UnityEngine.UI; // Thư viện này có thể cần nếu bạn dùng các component UI khác
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    // Các biến public để kéo thả trong Inspector
    public GameObject inventoryPanel;
    public Transform slotHolder;
    public GameObject slotPrefab;

    private PlayerInventory inventory;

    // Đăng ký và hủy đăng ký sự kiện
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Hàm được gọi mỗi khi scene mới tải xong
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Khi vào scene mới, luôn tìm lại PlayerInventory
        // và đăng ký lại sự kiện để đảm bảo kết nối
        if (inventory == null)
        {
            inventory = PlayerInventory.instance;
        }
        
        // Hủy đăng ký cũ (nếu có) và đăng ký lại
        inventory.onInventoryChangedCallback -= UpdateUI;
        inventory.onInventoryChangedCallback += UpdateUI;

        // Cập nhật lại UI ngay khi vào scene mới
        UpdateUI();
    }


    void Update()
    {
        
    }

    void UpdateUI()
    {
        // Kiểm tra để đảm bảo các tham chiếu không bị rỗng
        if (slotHolder == null || slotPrefab == null || inventory == null)
        {
            // In ra một cảnh báo nếu có gì đó bị thiếu
            Debug.LogWarning("InventoryUI is missing references (SlotHolder, SlotPrefab, or Inventory). UI cannot be updated.");
            return;
        }

        // Xóa các slot cũ đi
        foreach (Transform child in slotHolder)
        {
            Destroy(child.gameObject);
        }

        // Tạo slot mới cho từng vật phẩm trong túi đồ
        foreach (var itemEntry in inventory.items)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotHolder);
            InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.DisplayItem(itemEntry.Key, itemEntry.Value);
        }
    }
}