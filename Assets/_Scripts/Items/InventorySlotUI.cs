using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Button button; // Tham chiếu này rất quan trọng

    private ItemData currentItem;

    // Hàm này được gọi bởi InventoryUI để điền thông tin vào ô
    public void DisplayItem(ItemData item, int quantity)
    {
        currentItem = item;

        icon.sprite = item.icon;
        icon.enabled = true;

        if (quantity > 1)
        {
            quantityText.text = quantity.ToString();
            quantityText.gameObject.SetActive(true);
        }
        else
        {
            quantityText.gameObject.SetActive(false);
        }

        // Xóa các listener cũ để đảm bảo không bị gọi nhiều lần
        button.onClick.RemoveAllListeners();
        // Thêm một listener mới, khi nút được bấm, nó sẽ gọi hàm OnSlotClicked
        button.onClick.AddListener(OnSlotClicked);
    }

    // Hàm được gọi khi người chơi nhấp vào ô này
    // void OnSlotClicked()
    // {
    //     if (currentItem != null)
    //     {
    //         // Gọi đến hàm UseItem trong túi đồ của người chơi
    //         PlayerInventory.instance.UseItem(currentItem);
    //     }
    // }
    void OnSlotClicked()
    {
        if (currentItem != null)
        {
            // Mở panel hành động cho vật phẩm này
            ItemActionPanel.instance.OpenPanelForInventoryItem(currentItem);
        }
    }
}