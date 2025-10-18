using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    public EquipmentSlot equipSlot; // Gán trong Inspector: Weapon, Armor...
    public Image icon;
    public Button button;
    private ItemData currentItem;

    public void DisplayItem(ItemData item)
    {
        currentItem = item;
        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }
        else
        {
            icon.enabled = false;
        }
    }
    void Start()
    {
        button.onClick.AddListener(() => {
            if (currentItem != null)
            {
                // Khi được bấm, mở panel hành động cho vật phẩm này
                ItemActionPanel.instance.OpenPanelForEquipmentItem(currentItem);
            }
        });
    }
}