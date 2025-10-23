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