using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <-- THÊM THƯ VIỆN NÀY
using TMPro;

public class BlacksmithNPC : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject shopPanel;
    public GameObject upgradePanel;
    public GameObject interactionPrompt;
    public TextMeshProUGUI feedbackText;

    [Header("Shop Settings")]
    public List<ItemData> itemsForSale;
    public Transform shopSlotHolder;
    public GameObject shopSlotPrefab;

    [Header("Upgrade UI Elements")]
    public Image currentItemIcon;
    public TextMeshProUGUI currentItemName;
    public Image resultItemIcon;
    public TextMeshProUGUI resultItemStats;
    public TextMeshProUGUI requirementsText;
    public Button upgradeButton; // <-- Ô NÀY VẪN CẦN KÉO VÀO

    [Header("Upgrade Settings")]
    public List<UpgradeRecipe> availableUpgrades;

    private bool playerInRange = false;
    private PlayerStats playerStats;
    private PlayerInventory playerInventory;
    private UpgradeRecipe currentRecipe;

    // --- BẮT ĐẦU CODE MỚI ---
    private bool arePanelsInitialized = false; // Biến cờ
    // --- KẾT THÚC CODE MỚI ---

    void Start()
    {
        if(feedbackText != null) feedbackText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Tự động kết nối các nút khi mở bảng
            InitializePanelButtons(); // <-- GỌI HÀM MỚI

            mainPanel.SetActive(true);
        }
    }

    // --- BẮT ĐẦU CODE MỚI ---
    // Hàm này sẽ tìm và gán sự kiện cho các nút
    void InitializePanelButtons()
    {
        if (arePanelsInitialized || mainPanel == null || shopPanel == null || upgradePanel == null)
        {
            return;
        }

        Debug.Log("Initializing Blacksmith NPC buttons...");

        // 1. Tìm các nút trong MainPanel
        Button openShopBtn = mainPanel.transform.Find("OpenShopButton")?.GetComponent<Button>();
        Button openUpgradeBtn = mainPanel.transform.Find("OpenUpgradeButton")?.GetComponent<Button>();
        
        // --- BẮT ĐẦU CODE MỚI ---
        // Tìm nút "Đóng" (Giả sử tên là "CloseButton")
        Button closeMainBtn = mainPanel.transform.Find("CloseButton")?.GetComponent<Button>();
        // --- KẾT THÚC CODE MỚI ---

        if (openShopBtn != null) { /* (Code cũ) */ openShopBtn.onClick.AddListener(OpenShopPanel); }
        if (openUpgradeBtn != null) { /* (Code cũ) */ openUpgradeBtn.onClick.AddListener(OpenUpgradePanel); }

        // --- BẮT ĐẦU CODE MỚI ---
        if (closeMainBtn != null)
        {
            closeMainBtn.onClick.RemoveAllListeners();
            closeMainBtn.onClick.AddListener(CloseAllPanels); // Gán hàm "Đóng"
        }
        else { Debug.LogError("BlacksmithNPC: Không tìm thấy 'CloseButton' trong 'MainPanel'!"); }
        // --- KẾT THÚC CODE MỚI ---


        // 2. Tìm các nút "Back" (Quay lại) (Code cũ giữ nguyên)
        Button backFromShop = shopPanel.transform.Find("BackButton")?.GetComponent<Button>();
        Button backFromUpgrade = upgradePanel.transform.Find("BackButton")?.GetComponent<Button>();
        
        if(backFromShop) { backFromShop.onClick.AddListener(BackToMainPanel); }
        if(backFromUpgrade) { backFromUpgrade.onClick.AddListener(BackToMainPanel); }

        // 3. Gán sự kiện cho Nút Nâng cấp (Code cũ giữ nguyên)
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(PerformUpgrade);
        } 

        arePanelsInitialized = true; 
    }

    // --- Các hàm điều khiển UI (Giữ nguyên) ---
    public void OpenShopPanel()
    {
        shopPanel.SetActive(true);
        mainPanel.SetActive(false);
        UpdateShopUI();
    }
    
    public void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);
        mainPanel.SetActive(false);
        UpdateUpgradeUI(); 
    }

    public void CloseAllPanels()
    {
        mainPanel.SetActive(false);
        shopPanel.SetActive(false);
        upgradePanel.SetActive(false);
    }

    public void BackToMainPanel()
    {
        shopPanel.SetActive(false);
        upgradePanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    // --- Logic Cửa hàng (Giữ nguyên) ---
    void UpdateShopUI()
    {
        foreach (Transform child in shopSlotHolder) { Destroy(child.gameObject); }
        foreach (ItemData item in itemsForSale)
        {
            GameObject slotGO = Instantiate(shopSlotPrefab, shopSlotHolder);
            // Logic gán sự kiện cho ShopSlotUI đã đúng và tự động, không cần sửa
            slotGO.GetComponent<ShopSlotUI>().DisplayItem(item, this);
        }
    }

    public void BuyItem(ItemData item)
    {
        if (playerStats.SpendCoins(item.price))
        {
            playerInventory.AddItem(item);
            ShowFeedback("Mua thành công " + item.itemName + "!", 2f, Color.green);
        }
        else { ShowFeedback("Không đủ tiền!", 2f, Color.red); }
    }

    // --- Logic Nâng cấp (Giữ nguyên) ---
    void UpdateUpgradeUI()
    {
        // (Giữ nguyên toàn bộ code)
        ItemData equippedWeapon = EquipmentManager.instance.currentEquipment[(int)EquipmentSlot.Weapon];
        currentRecipe = FindRecipeFor(equippedWeapon);

        if (currentRecipe != null)
        {
            // (code... )
            upgradeButton.interactable = true;
        }
        else
        {
            // (code... )
            upgradeButton.interactable = false;
        }
    }

    UpgradeRecipe FindRecipeFor(ItemData item) { /* (Giữ nguyên) */ return null; }

    public void PerformUpgrade()
    {
        // (Giữ nguyên toàn bộ code)
        if (currentRecipe == null) { ShowFeedback("Không có gì để nâng cấp!", 2f, Color.yellow); return; }
        // (code... )
    }

    // --- Các hàm tiện ích (Giữ nguyên) ---
    void ShowFeedback(string message, float duration, Color color) { /* (Giữ nguyên) */ }
    IEnumerator FadeOutFeedback(float delay) { /* (Giữ nguyên) */ yield return null; }

    // --- Các hàm Trigger (Giữ nguyên) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionPrompt.SetActive(true);
            playerStats = other.GetComponent<PlayerStats>();
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionPrompt.SetActive(false);
            CloseAllPanels();
        }
    }
}