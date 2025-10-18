using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public Transform slotHolder;
    private EquipmentSlotUI[] slots;

    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += UpdateUI;
        slots = slotHolder.GetComponentsInChildren<EquipmentSlotUI>();
    }

    void UpdateUI(ItemData newItem, ItemData oldItem)
    {
        // Cập nhật lại toàn bộ UI
        for (int i = 0; i < slots.Length; i++)
        {
            // Tìm đúng ô và hiển thị
            if (i == (int)slots[i].equipSlot)
            {
                slots[i].DisplayItem(EquipmentManager.instance.currentEquipment[i]);
            }
        }
    }
}