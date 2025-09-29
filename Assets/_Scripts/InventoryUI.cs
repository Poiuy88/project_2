// // using UnityEngine;
// // using UnityEngine.UI;

// // public class InventoryUI : MonoBehaviour
// // {
// //     public GameObject inventoryPanel;
// //     public Transform slotHolder;
// //     public GameObject slotPrefab;

// //     private PlayerInventory inventory;

// //     void Start()
// //     {
// //         inventory = PlayerInventory.instance; // (Chúng ta sẽ sửa PlayerInventory một chút)
// //         inventory.onInventoryChangedCallback += UpdateUI;
// //         inventoryPanel.SetActive(false);
// //     }

// //     void Update()
// //     {
// //         // Nhấn phím 'I' để bật/tắt túi đồ
// //         if (Input.GetKeyDown(KeyCode.T))
// //         {
// //             inventoryPanel.SetActive(!inventoryPanel.activeSelf);
// //         }
// //     }

// //     void UpdateUI()
// //     {
// //         // Xóa các slot cũ đi
// //         foreach (Transform child in slotHolder)
// //         {
// //             Destroy(child.gameObject);
// //         }

// //         // Tạo slot mới cho từng vật phẩm trong túi đồ
// //         foreach (ItemData item in inventory.items)
// //         {
// //             GameObject slot = Instantiate(slotPrefab, slotHolder);
// //             Image icon = slot.GetComponent<Image>();
// //             icon.sprite = item.icon;
// //         }
// //     }
// // }


// using UnityEngine;

// public class InventoryUI : MonoBehaviour
// {
//     public GameObject inventoryPanel;
//     public Transform slotHolder;
//     public GameObject slotPrefab;

//     private PlayerInventory inventory;

//     void Start()
//     {
//         inventory = PlayerInventory.instance;
//         inventory.onInventoryChangedCallback += UpdateUI; // Lắng nghe sự kiện
//         inventoryPanel.SetActive(false);
//         UpdateUI(); // Cập nhật UI lần đầu
//     }

//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.T))
//         {
//             inventoryPanel.SetActive(!inventoryPanel.activeSelf);
//         }
//     }

//     void UpdateUI()
//     {
//         Debug.Log("Updating Inventory UI");
//         // Xóa các slot cũ đi
//         foreach (Transform child in slotHolder)
//         {
//             Destroy(child.gameObject);
//         }

//         // Tạo slot mới cho từng cặp (vật phẩm, số lượng) trong túi đồ
//         foreach (var itemEntry in inventory.items)
//         {
//             // itemEntry.Key là ItemData
//             // itemEntry.Value là số lượng (int)

//             GameObject slotGO = Instantiate(slotPrefab, slotHolder);
//             InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();

//             // Gửi thông tin vật phẩm và số lượng cho slot để nó tự hiển thị
//             slotUI.DisplayItem(itemEntry.Key, itemEntry.Value);
//         }
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Thêm thư viện để quản lý Scene

public class InventoryUI : MonoBehaviour
{
    // Các biến này không cần public nữa vì script sẽ tự tìm
    private GameObject inventoryPanel;
    private Transform slotHolder;
    
    // Vẫn cần prefab để tạo slot mới
    public GameObject slotPrefab;

    private PlayerInventory inventory;

    // --- LOGIC MỚI ĐỂ XỬ LÝ CHUYỂN CẢNH ---

    // Đăng ký lắng nghe sự kiện khi script được bật
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Hủy đăng ký khi script bị tắt để tránh lỗi
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Hàm này sẽ được gọi mỗi khi một scene mới được tải xong
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Chạy hàm thiết lập và tìm kiếm UI cho scene mới
        SetupInventoryForNewScene();
    }

    // --- KẾT THÚC LOGIC MỚI ---

    // void SetupInventoryForNewScene()
    // {
    //     // Không chạy logic này ở màn hình Login
    //     if (SceneManager.GetActiveScene().name == "LoginScene") return;

    //     // Cố gắng tìm Canvas trong scene mới
    //     Canvas canvas = FindObjectOfType<Canvas>();
    //     if (canvas != null)
    //     {
    //         // Tìm các thành phần UI bằng tên của chúng
    //         inventoryPanel = canvas.transform.Find("InventoryPanel").gameObject;
    //         slotHolder = inventoryPanel.transform.Find("SlotHolder");

    //         // Nếu đây là lần đầu tiên, hãy thiết lập kết nối với PlayerInventory
    //         if (inventory == null)
    //         {
    //             inventory = PlayerInventory.instance;
    //             inventory.onInventoryChangedCallback += UpdateUI;
    //         }

    //         // Đảm bảo túi đồ luôn tắt khi vào map mới
    //         inventoryPanel.SetActive(false);
    //         // Cập nhật lại UI một lần để chắc chắn nó đúng
    //         UpdateUI();
    //     }
    //     else
    //     {
    //         Debug.LogWarning("InventoryUI could not find a Canvas in the new scene. Inventory will not work.");
    //     }
    // }
void SetupInventoryForNewScene()
{
    // Không chạy logic này ở màn hình Login
    if (SceneManager.GetActiveScene().name == "LoginScene") return;

    // Cố gắng tìm Canvas trong scene mới
    Canvas canvas = FindObjectOfType<Canvas>();
    if (canvas == null)
    {
        Debug.LogError("InventoryUI: Could not find a Canvas in this scene! UI will not work.");
        return; // Dừng lại nếu không có Canvas
    }

    // --- KIỂM TRA TỪNG BƯỚC MỘT ---

    // Tìm InventoryPanel
    Transform inventoryPanelTransform = canvas.transform.Find("InventoryPanel");
    if (inventoryPanelTransform == null)
    {
        Debug.LogError("InventoryUI: Could not find GameObject named 'InventoryPanel' as a child of the Canvas!");
        return;
    }
    inventoryPanel = inventoryPanelTransform.gameObject;

    // Tìm SlotHolder
    slotHolder = inventoryPanel.transform.Find("SlotHolder");
    if (slotHolder == null)
    {
        Debug.LogError("InventoryUI: Could not find GameObject named 'SlotHolder' as a child of the 'InventoryPanel'!");
        return;
    }

    // Tìm PlayerInventory
    if (inventory == null)
    {
        inventory = PlayerInventory.instance;
        if (inventory == null)
        {
             Debug.LogError("InventoryUI: Could not find PlayerInventory instance!");
             return;
        }
        inventory.onInventoryChangedCallback += UpdateUI;
    }

    // Nếu mọi thứ được tìm thấy, thiết lập trạng thái ban đầu
    inventoryPanel.SetActive(false);
    UpdateUI();
}
    void Update()
    {
        // Thêm kiểm tra 'inventoryPanel != null' để đảm bảo an toàn
        if (inventoryPanel != null && Input.GetKeyDown(KeyCode.T))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    void UpdateUI()
    {
        // Thêm kiểm tra để đảm bảo slotHolder đã được tìm thấy
        if (slotHolder == null) return;

        Debug.Log("Updating Inventory UI");
        foreach (Transform child in slotHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (var itemEntry in inventory.items)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotHolder);
            InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.DisplayItem(itemEntry.Key, itemEntry.Value);
        }
    }
}