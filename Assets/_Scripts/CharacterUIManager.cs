using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // <-- THÊM THƯ VIỆN NÀY ĐỂ DÙNG "Button"

public class CharacterUIManager : MonoBehaviour
{
    public static CharacterUIManager instance;
    
    // Các tham chiếu panel (vẫn giữ private)
    private GameObject characterHubPanel;
    private GameObject inventoryPanel;
    private GameObject equipmentPanel;
    private GameObject statsPanel;
    
    public ItemActionPanel itemActionPanel; 

    void Awake() 
    { 
        instance = this; 
    }

    // Đăng ký lắng nghe sự kiện tải scene
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
        if (scene.name == "LoginScene") return; 
        
        FindUIElements(); // Tìm UI
        CloseAllPanels(); // Ẩn tất cả khi mới vào scene
    }


    // HÀM QUAN TRỌNG ĐÃ ĐƯỢC NÂNG CẤP
    void FindUIElements()
    {
        // 1. Tìm Canvas
        // Canvas canvas = FindObjectOfType<Canvas>();
        // if (canvas == null)
        // {
        //     Debug.LogError("CharacterUIManager: Không tìm thấy Canvas trong scene!");
        //     return;
        // }
        GameObject canvasGO = GameObject.FindGameObjectWithTag("MainCanvas");
        if (canvasGO == null)
        {
            Debug.LogError("CharacterUIManager: Không tìm thấy Canvas với tag 'MainCanvas'!");
            return;
        }
        Canvas canvas = canvasGO.GetComponent<Canvas>();

        // 2. Tìm các panel con bằng tên
        characterHubPanel = FindChildGameObject(canvas.transform, "CharacterHubPanel");
        inventoryPanel = FindChildGameObject(canvas.transform, "InventoryPanel");
        equipmentPanel = FindChildGameObject(canvas.transform, "EquipmentPanel");
        statsPanel = FindChildGameObject(canvas.transform, "StatsPanel");
        
        itemActionPanel = canvas.GetComponentInChildren<ItemActionPanel>(true); 

        // --- BẮT ĐẦU PHẦN NÂNG CẤP ---
        // 3. Tự động kết nối lại các nút (Button) trong CharacterHubPanel
        if (characterHubPanel != null)
        {
            // TÌM CÁC NÚT BÊN TRONG HUB
            // LƯU Ý: Tên "InventoryButton", "EquipmentButton" phải khớp với tên đối tượng Button của bạn
            Button invButton = FindChildGameObject(characterHubPanel.transform, "InventoryButton")?.GetComponent<Button>();
            Button equipButton = FindChildGameObject(characterHubPanel.transform, "EquipmentButton")?.GetComponent<Button>();
            Button statsButton = FindChildGameObject(characterHubPanel.transform, "StatsButton")?.GetComponent<Button>();

            // Kết nối nút Túi đồ
            if (invButton != null)
            {
                invButton.onClick.RemoveAllListeners(); // Xóa listener cũ
                invButton.onClick.AddListener(OpenInventory); // Thêm listener mới
            } else { Debug.LogError("CharacterUIManager: Không tìm thấy 'InventoryButton' trong 'CharacterHubPanel'!"); }

            // Kết nối nút Trang bị
            if (equipButton != null)
            {
                equipButton.onClick.RemoveAllListeners();
                equipButton.onClick.AddListener(OpenEquipment);
            } else { Debug.LogError("CharacterUIManager: Không tìm thấy 'EquipmentButton' trong 'CharacterHubPanel'!"); }
            
            // Kết nối nút Chỉ số
            if (statsButton != null)
            {
                statsButton.onClick.RemoveAllListeners();
                statsButton.onClick.AddListener(OpenStatsPanel);
            } else { Debug.LogError("CharacterUIManager: Không tìm thấy 'StatsButton' trong 'CharacterHubPanel'!"); }
        }
        else { Debug.LogError("CharacterUIManager: Không tìm thấy 'CharacterHubPanel'!"); }

        // 4. Tự động kết nối các nút "Back" (nếu có)
        // (Giả sử các panel kia đều có nút "BackButton" quay về Hub)
        Button backFromInv = FindChildGameObject(inventoryPanel.transform, "BackButton")?.GetComponent<Button>();
        Button backFromEquip = FindChildGameObject(equipmentPanel.transform, "BackButton")?.GetComponent<Button>();
        Button backFromStats = FindChildGameObject(statsPanel.transform, "BackButton")?.GetComponent<Button>();
        
        if(backFromInv) { backFromInv.onClick.RemoveAllListeners(); backFromInv.onClick.AddListener(BackToHubPanel); }
        if(backFromEquip) { backFromEquip.onClick.RemoveAllListeners(); backFromEquip.onClick.AddListener(BackToHubPanel); }
        if(backFromStats) { backFromStats.onClick.RemoveAllListeners(); backFromStats.onClick.AddListener(BackToHubPanel); }
        // --- KẾT THÚC PHẦN NÂNG CẤP ---

        if (inventoryPanel == null) Debug.LogError("CharacterUIManager: Không tìm thấy 'InventoryPanel'!");
        if (equipmentPanel == null) Debug.LogError("CharacterUIManager: Không tìm thấy 'EquipmentPanel'!");
        if (statsPanel == null) Debug.LogError("CharacterUIManager: Không tìm thấy 'StatsPanel'!");
        if (itemActionPanel == null) Debug.LogError("CharacterUIManager: Không tìm thấy script 'ItemActionPanel'!");
    }

    // Hàm tiện ích để tìm GameObject con (Giữ nguyên)
    GameObject FindChildGameObject(Transform parent, string name)
    {
        if (parent == null) return null; // Thêm kiểm tra null
        
        Transform child = parent.Find(name);
        if (child != null)
        {
            return child.gameObject;
        }
        
        // Tìm kiếm sâu hơn (Giữ nguyên)
        foreach (Transform t in parent)
        {
            GameObject result = FindChildGameObject(t, name);
            if (result != null) return result;
        }
        
        return null;
    }
    
    // Các hàm còn lại của file giữ nguyên
    void Start() { }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (characterHubPanel != null) 
            {
                characterHubPanel.SetActive(!characterHubPanel.activeSelf);
                if (!characterHubPanel.activeSelf)
                {
                    CloseAllPanels();
                }
            }
        }
    }

    public void OpenInventory()
    {
        if(inventoryPanel) inventoryPanel.SetActive(true);
        if(equipmentPanel) equipmentPanel.SetActive(false);
        if(statsPanel) statsPanel.SetActive(false);
        if(characterHubPanel) characterHubPanel.SetActive(false);
    }

    public void OpenEquipment()
    {
        if(inventoryPanel) inventoryPanel.SetActive(false);
        if(equipmentPanel) equipmentPanel.SetActive(true);
        if(statsPanel) statsPanel.SetActive(false);
        if(characterHubPanel) characterHubPanel.SetActive(false);
    }

    public void CloseAllPanels()
    {
        if(characterHubPanel) characterHubPanel.SetActive(false);
        if(inventoryPanel) inventoryPanel.SetActive(false);
        if(equipmentPanel) equipmentPanel.SetActive(false);
        if(statsPanel) statsPanel.SetActive(false);
    }

    public void BackToHubPanel()
    {
        if(characterHubPanel) characterHubPanel.SetActive(true);
        if(inventoryPanel) inventoryPanel.SetActive(false);
        if(equipmentPanel) equipmentPanel.SetActive(false);
        if(statsPanel) statsPanel.SetActive(false);
    }

    public void OpenStatsPanel()
    {
        if(statsPanel) statsPanel.SetActive(true);
        if(inventoryPanel) inventoryPanel.SetActive(false);
        if(equipmentPanel) equipmentPanel.SetActive(false);
    }
}