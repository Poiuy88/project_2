using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    // --- SỬA ĐỔI: Chuyển các tham chiếu Scene sang private ---
    private GameObject inventoryPanel;
    private Transform slotHolder;
    // --------------------------------------------------------

    // GIỮ LẠI: Prefab này được kéo từ Project nên sẽ không bị mất
    public GameObject slotPrefab;

    private PlayerInventory inventory;

    // Đăng ký và hủy đăng ký sự kiện (Giữ nguyên)
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // --- HÀM NÀY ĐƯỢC NÂNG CẤP HOÀN TOÀN ---
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Bỏ qua nếu ở màn Login
        if (scene.name == "LoginScene") return;

        // 1. Tìm PlayerInventory (như cũ)
        if (inventory == null)
        {
            inventory = PlayerInventory.instance;
        }

        // 2. Tìm các thành phần UI trong Scene mới
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            // Tìm InventoryPanel (phải khớp tên)
            Transform panelTransform = canvas.transform.Find("InventoryPanel");
            if (panelTransform != null)
            {
                inventoryPanel = panelTransform.gameObject;

                // Tìm SlotHolder (khung chứa slot) BÊN TRONG InventoryPanel
                slotHolder = inventoryPanel.transform.Find("SlotHolder"); // <-- Tên này phải khớp
                if (slotHolder == null)
                {
                    Debug.LogError("InventoryUI: Không tìm thấy 'SlotHolder' bên trong 'InventoryPanel'!");
                }
            }
            else
            {
                Debug.LogError("InventoryUI: Không tìm thấy 'InventoryPanel' trong Canvas!");
            }
        }
        else
        {
            Debug.LogError("InventoryUI: Không tìm thấy Canvas!");
        }


        // 3. Đăng ký sự kiện (như cũ)
        inventory.onInventoryChangedCallback -= UpdateUI;
        inventory.onInventoryChangedCallback += UpdateUI;

        // 4. Cập nhật UI ngay lập tức
        UpdateUI();
    }
    // --- KẾT THÚC NÂNG CẤP ---


    // Hàm UpdateUI() không cần sửa, nhưng hãy đảm bảo bạn có kiểm tra null
    void UpdateUI()
    {
        // Kiểm tra để đảm bảo các tham chiếu không bị rỗng
        // Giờ đây slotHolder sẽ được tìm thấy
        if (slotHolder == null || slotPrefab == null || inventory == null)
        {
            // Lỗi này vẫn có thể xuất hiện nếu tên trong Hierarchy của bạn không khớp
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