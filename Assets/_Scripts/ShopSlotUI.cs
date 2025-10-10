using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public Button buyButton;

    private ItemData currentItem;
    private BlacksmithNPC blacksmith; // Tham chiếu đến script của thợ rèn

    // Hàm này được gọi bởi BlacksmithNPC để điền thông tin vật phẩm
    public void DisplayItem(ItemData item, BlacksmithNPC blacksmithRef)
    {
        currentItem = item;
        blacksmith = blacksmithRef;

        itemIcon.sprite = item.icon;
        itemNameText.text = item.itemName;
        itemPriceText.text = item.price.ToString() + " xu";

        // Gán sự kiện cho nút Mua
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    // Khi nút Mua được bấm
    void OnBuyButtonClicked()
    {
        // Gọi đến hàm BuyItem của thợ rèn và truyền vào vật phẩm này
        blacksmith.BuyItem(currentItem);
    }
}