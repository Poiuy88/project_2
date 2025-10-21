// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// public class BlacksmithNPC : MonoBehaviour
// {
//     [Header("UI Panels")]
//     public GameObject mainPanel;
//     public GameObject shopPanel;
//     public GameObject upgradePanel;
//     public GameObject interactionPrompt;
//     public TextMeshProUGUI feedbackText;

//     [Header("Shop Settings")]
//     public List<ItemData> itemsForSale; // Danh sách các vật phẩm để bán
//     public Transform shopSlotHolder;
//     public GameObject shopSlotPrefab; // Prefab cho một ô bán hàng
//     public List<UpgradeRecipe> availableUpgrades;

//     private bool playerInRange = false;
//     private PlayerStats playerStats;
//     private PlayerInventory playerInventory;
//     private UpgradeRecipe currentRecipe;
//     void Start()
//     {
//         if(feedbackText != null) feedbackText.gameObject.SetActive(false);
//     }

//     void Update()
//     {
//         if (playerInRange && Input.GetKeyDown(KeyCode.E))
//         {
//             mainPanel.SetActive(true);
//         }
//     }

//     // --- Các hàm điều khiển UI ---
//     public void OpenShopPanel()
//     {
//         shopPanel.SetActive(true);
//         mainPanel.SetActive(false);
//         UpdateShopUI(); // Cập nhật các mặt hàng
//     }

//     public void OpenUpgradePanel()
//     {
//         // Logic cho nâng cấp sẽ được thêm sau
//         upgradePanel.SetActive(true);
//         mainPanel.SetActive(false);
//     }

//     public void CloseAllPanels()
//     {
//         mainPanel.SetActive(false);
//         shopPanel.SetActive(false);
//         upgradePanel.SetActive(false);
//     }

//     public void BackToMainPanel()
//     {
//         shopPanel.SetActive(false);
//         upgradePanel.SetActive(false);
//         mainPanel.SetActive(true);
//     }
//     void UpdateShopUI()
//     {
//         // Xóa các slot cũ đi
//         foreach (Transform child in shopSlotHolder)
//         {
//             Destroy(child.gameObject);
//         }

//         // Tạo các slot mới từ danh sách itemsForSale
//         foreach (ItemData item in itemsForSale)
//         {
//             GameObject slotGO = Instantiate(shopSlotPrefab, shopSlotHolder);
//             // Lấy script và truyền dữ liệu vật phẩm, cùng với một tham chiếu đến chính thợ rèn này
//             slotGO.GetComponent<ShopSlotUI>().DisplayItem(item, this);
//         }
//     }

//     public void BuyItem(ItemData item)
//     {
//         if (playerStats.SpendCoins(item.price))
//         {
//             playerInventory.AddItem(item);
//             ShowFeedback("Mua thành công " + item.itemName + "!", 2f, Color.green);
//         }
//         else
//         {
//             ShowFeedback("Không đủ tiền!", 2f, Color.red);
//         }
//     }
//     void ShowFeedback(string message, float duration, Color color)
//     {
//         if (feedbackText != null)
//         {
//             StopCoroutine("FadeOutFeedback"); // Dừng coroutine cũ nếu có
//             feedbackText.text = message;
//             feedbackText.color = color;
//             feedbackText.gameObject.SetActive(true);
//             StartCoroutine(FadeOutFeedback(duration));
//         }
//     }
//     IEnumerator FadeOutFeedback(float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         if (feedbackText != null)
//         {
//             feedbackText.gameObject.SetActive(false);
//         }
//     }

//     // --- Các hàm Trigger ---
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             playerInRange = true;
//             interactionPrompt.SetActive(true);
//             playerStats = other.GetComponent<PlayerStats>();
//             playerInventory = other.GetComponent<PlayerInventory>();
//         }
//     }

//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             playerInRange = false;
//             interactionPrompt.SetActive(false);
//             CloseAllPanels();
//         }
//     }
//     void UpdateUpgradeUI()
//     {
//         // 1. Tìm xem người chơi đang MẶC trang bị gì
//         //    (Để đơn giản, trước mắt ta tập trung nâng cấp VŨ KHÍ đang mặc)
//         ItemData equippedWeapon = EquipmentManager.instance.currentEquipment[(int)EquipmentSlot.Weapon];

//         // 2. Tìm xem có công thức nâng cấp cho vũ khí đó không
//         currentRecipe = FindRecipeFor(equippedWeapon);

//         // 3. Cập nhật UI dựa trên việc có tìm thấy công thức hay không
//         if (currentRecipe != null)
//         {
//             // Hiển thị thông tin vật phẩm hiện tại
//             currentItemIcon.sprite = currentRecipe.baseItem.icon;
//             currentItemIcon.enabled = true; // Đảm bảo hình ảnh hiển thị
//             currentItemName.text = currentRecipe.baseItem.itemName;

//             // Hiển thị thông tin vật phẩm kết quả
//             resultItemIcon.sprite = currentRecipe.resultItem.icon;
//             resultItemIcon.enabled = true;
//             // Hiển thị chỉ số rõ ràng (điều chỉnh định dạng nếu cần)
//             resultItemStats.text = $"Tấn công: {currentRecipe.baseItem.attackBonus} -> <color=green>{currentRecipe.resultItem.attackBonus}</color>\n" +
//                                 $"Phòng thủ: {currentRecipe.baseItem.defenseBonus} -> <color=green>{currentRecipe.resultItem.defenseBonus}</color>"; // Thêm chỉ số khác nếu cần

//             // Hiển thị yêu cầu
//             requirementsText.text = $"Yêu cầu: Level {currentRecipe.requiredLevel}, {currentRecipe.cost} xu";

//             // Cho phép bấm nút nâng cấp
//             upgradeButton.interactable = true;
//         }
//         else
//         {
//             // Nếu không tìm thấy công thức (hoặc chưa mặc vũ khí), hiển thị thông tin chờ
//             currentItemIcon.sprite = null; // Xóa icon
//             currentItemIcon.enabled = false; // Ẩn component Image
//             currentItemName.text = "Đặt vũ khí đang mặc vào"; // Hoặc thông báo tương tự

//             resultItemIcon.sprite = null;
//             resultItemIcon.enabled = false;
//             resultItemStats.text = "";
//             requirementsText.text = "Không tìm thấy công thức nâng cấp.";

//             // Không cho phép bấm nút nâng cấp
//             upgradeButton.interactable = false;
//         }
//     }
//     // Hàm tìm công thức phù hợp (Giữ nguyên hàm này)
//     UpgradeRecipe FindRecipeFor(ItemData item)
//     {
//         if (item == null) return null;
//         foreach (UpgradeRecipe recipe in availableUpgrades)
//         {
//             if (recipe.baseItem == item)
//             {
//                 return recipe;
//             }
//         }
//         return null;
//     }
//     // Hàm này được gọi bởi nút "Nâng Cấp"
//     public void PerformUpgrade()
//     {
//         // Kiểm tra lại xem có công thức hợp lệ không
//         if (currentRecipe == null)
//         {
//             ShowFeedback("Không có gì để nâng cấp!", 2f, Color.yellow);
//             return;
//         }

//         // 1. Kiểm tra Level người chơi
//         if (playerStats.playerLevel < currentRecipe.requiredLevel)
//         {
//             ShowFeedback($"Cần đạt Level {currentRecipe.requiredLevel}!", 2f, Color.red);
//             return;
//         }

//         // 2. Kiểm tra Chi phí (dùng SpendCoins đảm bảo an toàn)
//         if (playerStats.SpendCoins(currentRecipe.cost))
//         {
//             // 3. Thực hiện việc đổi vật phẩm
//             //    - Tháo trang bị cũ (hàm này cũng tạm thời thêm nó vào túi đồ)
//             EquipmentManager.instance.Unequip((int)currentRecipe.baseItem.equipSlot);
//             //    - Xóa vật phẩm cũ vừa được thêm lại vào túi đồ
//             playerInventory.RemoveSpecificItem(currentRecipe.baseItem); // Cần một hàm mới cho việc này
//             //    - Thêm vật phẩm mới đã nâng cấp vào túi đồ
//             playerInventory.AddItem(currentRecipe.resultItem);

//             ShowFeedback("Nâng cấp thành công!", 2f, Color.green);
//             UpdateUpgradeUI(); // Làm mới UI để hiển thị không còn nâng cấp (hoặc cái tiếp theo)
//         }
//         else
//         {
//             ShowFeedback("Không đủ tiền!", 2f, Color.red);
//         }
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // << THÊM THƯ VIỆN UI
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

    // --- CÁC BIẾN UI CHO NÂNG CẤP (BỊ THIẾU TRƯỚC ĐÂY) ---
    [Header("Upgrade UI Elements")]
    public Image currentItemIcon;
    public TextMeshProUGUI currentItemName;
    public Image resultItemIcon;
    public TextMeshProUGUI resultItemStats;
    public TextMeshProUGUI requirementsText;
    public Button upgradeButton;
    // --- KẾT THÚC PHẦN KHAI BÁO BỊ THIẾU ---

    [Header("Upgrade Settings")]
    public List<UpgradeRecipe> availableUpgrades;

    private bool playerInRange = false;
    private PlayerStats playerStats;
    private PlayerInventory playerInventory;
    private UpgradeRecipe currentRecipe;

    void Start()
    {
        if(feedbackText != null) feedbackText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            mainPanel.SetActive(true);
        }
    }

    // --- Các hàm điều khiển UI ---
    public void OpenShopPanel()
    {
        shopPanel.SetActive(true);
        mainPanel.SetActive(false);
        UpdateShopUI();
    }

    // Sửa lại hàm OpenUpgradePanel để nó cập nhật UI
    public void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);
        mainPanel.SetActive(false);
        UpdateUpgradeUI(); // Gọi hàm cập nhật
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

    // --- Logic Cửa hàng ---
    void UpdateShopUI()
    {
        foreach (Transform child in shopSlotHolder) { Destroy(child.gameObject); }
        foreach (ItemData item in itemsForSale)
        {
            GameObject slotGO = Instantiate(shopSlotPrefab, shopSlotHolder);
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

    // --- Logic Nâng cấp ---
    void UpdateUpgradeUI()
    {
        ItemData equippedWeapon = EquipmentManager.instance.currentEquipment[(int)EquipmentSlot.Weapon];
        currentRecipe = FindRecipeFor(equippedWeapon);

        if (currentRecipe != null)
        {
            currentItemIcon.sprite = currentRecipe.baseItem.icon;
            currentItemIcon.enabled = true;
            currentItemName.text = currentRecipe.baseItem.itemName;

            resultItemIcon.sprite = currentRecipe.resultItem.icon;
            resultItemIcon.enabled = true;
            resultItemStats.text = $"Tấn công: {currentRecipe.baseItem.attackBonus} -> <color=green>{currentRecipe.resultItem.attackBonus}</color>\n" +
                                   $"Phòng thủ: {currentRecipe.baseItem.defenseBonus} -> <color=green>{currentRecipe.resultItem.defenseBonus}</color>";

            requirementsText.text = $"Yêu cầu: Level {currentRecipe.requiredLevel}, {currentRecipe.cost} xu";
            upgradeButton.interactable = true;
        }
        else
        {
            currentItemIcon.sprite = null;
            currentItemIcon.enabled = false;
            currentItemName.text = "Đặt vũ khí đang mặc vào";
            resultItemIcon.sprite = null;
            resultItemIcon.enabled = false;
            resultItemStats.text = "";
            requirementsText.text = "Không tìm thấy công thức nâng cấp.";
            upgradeButton.interactable = false;
        }
    }

    UpgradeRecipe FindRecipeFor(ItemData item)
    {
        if (item == null) return null;
        foreach (UpgradeRecipe recipe in availableUpgrades)
        {
            if (recipe.baseItem == item) { return recipe; }
        }
        return null;
    }

    public void PerformUpgrade()
    {
        if (currentRecipe == null) { ShowFeedback("Không có gì để nâng cấp!", 2f, Color.yellow); return; }
        if (playerStats.playerLevel < currentRecipe.requiredLevel) { ShowFeedback($"Cần đạt Level {currentRecipe.requiredLevel}!", 2f, Color.red); return; }

        if (playerStats.SpendCoins(currentRecipe.cost))
        {
            EquipmentManager.instance.Unequip((int)currentRecipe.baseItem.equipSlot);
            playerInventory.RemoveSpecificItem(currentRecipe.baseItem);
            playerInventory.AddItem(currentRecipe.resultItem);
            ShowFeedback("Nâng cấp thành công!", 2f, Color.green);
            UpdateUpgradeUI();
        }
        else { ShowFeedback("Không đủ tiền!", 2f, Color.red); }
    }

    // --- Các hàm tiện ích ---
    void ShowFeedback(string message, float duration, Color color)
    {
        if (feedbackText != null)
        {
            StopCoroutine("FadeOutFeedback");
            feedbackText.text = message;
            feedbackText.color = color;
            feedbackText.gameObject.SetActive(true);
            StartCoroutine(FadeOutFeedback(duration));
        }
    }

    IEnumerator FadeOutFeedback(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (feedbackText != null) { feedbackText.gameObject.SetActive(false); }
    }

    // --- Các hàm Trigger ---
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