using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    private GameObject inventoryPanel;
    private Transform slotHolder;
    public GameObject slotPrefab;

    private PlayerInventory inventory; // Đây là inventory "xịn"

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // HÀM NÀY ĐƯỢC SỬA LỖI HOÀN TOÀN
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LoginScene") return;

        // --- BẮT ĐẦU SỬA LỖI (LOGIC TÌM KIẾM) ---
        if (PlayerPersistence.instance != null)
        {
            // Hủy đăng ký sự kiện khỏi inventory CŨ (nếu có)
            // (Phòng trường hợp đăng ký nhầm)
            if (inventory != null)
            {
                inventory.onInventoryChangedCallback -= UpdateUI;
            }

            // Lấy inventory CHÍNH XÁC từ Player "xịn"
            inventory = PlayerPersistence.instance.playerInventory;

            // Đăng ký sự kiện với inventory "xịn"
            inventory.onInventoryChangedCallback += UpdateUI;
        }
        else
        {
            Debug.LogError("InventoryUI: Không tìm thấy PlayerPersistence!");
            return;
        }
        // --- KẾT THÚC SỬA LỖI ---

        // Tìm các thành phần UI (Code này đã đúng)
        GameObject canvasGO = GameObject.FindGameObjectWithTag("MainCanvas");
        if (canvasGO != null)
        {
            Transform panelTransform = canvasGO.transform.Find("InventoryPanel");
            if (panelTransform != null)
            {
                inventoryPanel = panelTransform.gameObject;
                slotHolder = inventoryPanel.transform.Find("SlotHolder"); 
                if (slotHolder == null) Debug.LogError("InventoryUI: Không tìm thấy 'SlotHolder'!");
            }
            else Debug.LogError("InventoryUI: Không tìm thấy 'InventoryPanel'!");
        }
        else Debug.LogError("InventoryUI: Không tìm thấy Canvas!");

        // Cập nhật UI ngay lập tức (giờ sẽ dùng đúng inventory Lv 2)
        UpdateUI();
    }


    // HÀM NÀY GIỮ NGUYÊN
    void UpdateUI()
    {
        if (slotHolder == null || slotPrefab == null || inventory == null)
        {
            Debug.LogWarning("InventoryUI missing references, cannot update.");
            return;
        }

        // Xóa các slot cũ đi
        foreach (Transform child in slotHolder)
        {
            Destroy(child.gameObject);
        }

        // Tạo slot mới cho từng vật phẩm trong túi đồ "xịn"
        foreach (var itemEntry in inventory.items)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotHolder);
            InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.DisplayItem(itemEntry.Key, itemEntry.Value);
        }
    }
}