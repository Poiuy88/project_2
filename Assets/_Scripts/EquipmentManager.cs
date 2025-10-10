using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    void Awake() { instance = this; }

    public ItemData[] currentEquipment; // Mảng chứa các trang bị đang mặc
    private PlayerInventory inventory;
    private PlayerStats playerStats;

    void Start()
    {
        inventory = PlayerInventory.instance;
        playerStats = GetComponent<PlayerStats>();

        // Khởi tạo mảng trang bị dựa trên số lượng slot
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new ItemData[numSlots];
    }

    public void Equip(ItemData newItem)
    {
        if (playerStats.playerLevel < 5)
    {
        Debug.Log("Cần đạt level 5 để có thể trang bị vật phẩm!");
        return; // Dừng hàm lại, không cho phép mặc đồ
    }
        int slotIndex = (int)newItem.equipSlot;

        // Nếu đã có đồ ở ô đó, tháo ra trước
        if (currentEquipment[slotIndex] != null)
        {
            inventory.AddItem(currentEquipment[slotIndex]);
        }

        currentEquipment[slotIndex] = newItem;
        inventory.RemoveItem(newItem);
        UpdateStats();
    }

    void UpdateStats()
    {
        int totalAttackBonus = 0;
        int totalDefenseBonus = 0;

        foreach (ItemData item in currentEquipment)
        {
            if (item != null)
            {
                totalAttackBonus += item.attackBonus;
                totalDefenseBonus += item.defenseBonus;
            }
        }
        playerStats.UpdateEquipmentStats(totalAttackBonus, totalDefenseBonus);
    }
}