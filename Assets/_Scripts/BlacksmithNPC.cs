using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public List<ItemData> itemsForSale; // Danh sách các vật phẩm để bán
    public Transform shopSlotHolder;
    public GameObject shopSlotPrefab; // Prefab cho một ô bán hàng

    private bool playerInRange = false;
    private PlayerStats playerStats;
    private PlayerInventory playerInventory;
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
        UpdateShopUI(); // Cập nhật các mặt hàng
    }

    public void OpenUpgradePanel()
    {
        // Logic cho nâng cấp sẽ được thêm sau
        upgradePanel.SetActive(true);
        mainPanel.SetActive(false);
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
    void UpdateShopUI()
    {
        // Xóa các slot cũ đi
        foreach (Transform child in shopSlotHolder)
        {
            Destroy(child.gameObject);
        }

        // Tạo các slot mới từ danh sách itemsForSale
        foreach (ItemData item in itemsForSale)
        {
            GameObject slotGO = Instantiate(shopSlotPrefab, shopSlotHolder);
            // Lấy script và truyền dữ liệu vật phẩm, cùng với một tham chiếu đến chính thợ rèn này
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
        else
        {
            ShowFeedback("Không đủ tiền!", 2f, Color.red);
        }
    }
    void ShowFeedback(string message, float duration, Color color)
    {
        if (feedbackText != null)
        {
            StopCoroutine("FadeOutFeedback"); // Dừng coroutine cũ nếu có
            feedbackText.text = message;
            feedbackText.color = color;
            feedbackText.gameObject.SetActive(true);
            StartCoroutine(FadeOutFeedback(duration));
        }
    }
    IEnumerator FadeOutFeedback(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }
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