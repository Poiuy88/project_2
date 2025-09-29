// // using UnityEngine;
// // using UnityEngine.UI;
// // using TMPro;

// // public class InventorySlotUI : MonoBehaviour
// // {
// //     public Image icon;
// //     public TextMeshProUGUI quantityText;

// //     public void DisplayItem(ItemData item, int quantity)
// //     {
// //         // Hiển thị icon
// //         icon.sprite = item.icon;
// //         icon.enabled = true;

// //         // Hiển thị số lượng (chỉ khi lớn hơn 1)
// //         if (quantity > 1)
// //         {
// //             quantityText.text = quantity.ToString();
// //             quantityText.enabled = true;
// //         }
// //         else
// //         {
// //             quantityText.enabled = false;
// //         }
// //     }
// // }

// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class InventorySlotUI : MonoBehaviour
// {
//     public Image icon;
//     public TextMeshProUGUI quantityText;

//     public void DisplayItem(ItemData item, int quantity)
//     {
//         // Hiển thị icon
//         icon.sprite = item.icon;
//         icon.enabled = true;

//         // Hiển thị số lượng (chỉ khi lớn hơn 1)
//         if (quantity > 1)
//         {
//             quantityText.text = quantity.ToString();
//             // Dùng gameObject.SetActive để đảm bảo nó luôn hiện ra
//             quantityText.gameObject.SetActive(true); 
//         }
//         else
//         {
//             // Ẩn đối tượng Text đi nếu số lượng là 1
//             quantityText.gameObject.SetActive(false);
//         }
//     }
// }
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
    void OnSlotClicked()
    {
        if (currentItem != null)
        {
            // Gọi đến hàm UseItem trong túi đồ của người chơi
            PlayerInventory.instance.UseItem(currentItem);
        }
    }
}