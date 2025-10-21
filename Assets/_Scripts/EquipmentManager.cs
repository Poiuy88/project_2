using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    
    public ItemData[] currentEquipment;
    public delegate void OnEquipmentChanged(ItemData newItem, ItemData oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    private PlayerInventory inventory;
    private PlayerStats playerStats;
    void Awake()
    {
        instance = this;

        // Tự động tạo mảng có kích thước bằng số lượng slot trong enum
        // DÒNG NÀY ĐƯỢC CHUYỂN TỪ START() LÊN ĐÂY
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new ItemData[numSlots];
    }
    void Start()
    {
        // Hàm Start bây giờ chỉ còn nhiệm vụ lấy các tham chiếu khác
        inventory = PlayerInventory.instance;
        playerStats = GetComponent<PlayerStats>();
    }

    public void Equip(ItemData newItem)
    {
        if (playerStats.playerLevel < 5)
        {
            Debug.Log("Cần đạt level 5 để trang bị!");
            return;
        }

        int slotIndex = (int)newItem.equipSlot;
        ItemData oldItem = null;

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.AddItem(oldItem);
        }

        currentEquipment[slotIndex] = newItem;
        inventory.RemoveItem(newItem);

        // Gửi tín hiệu quan trọng cho UI
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        UpdateStats();
    }

    public void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            ItemData oldItem = currentEquipment[slotIndex];
            inventory.AddItem(oldItem);
            currentEquipment[slotIndex] = null;

            // Gửi tín hiệu quan trọng cho UI
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
            UpdateStats();
        }
    }
    void UpdateStats()
    {
        // Reset các bonus về 0 trước khi tính lại
        int totalAttackBonus = 0;
        int totalDefenseBonus = 0;
        int totalHealthBonus = 0;
        int totalManaBonus = 0;
        float totalSpeedBonus = 0f;

        // Duyệt qua tất cả các trang bị đang mặc
        foreach (ItemData item in currentEquipment)
        {
            if (item != null) // Nếu ô đó có đồ
            {
                // Cộng dồn các chỉ số từ món đồ đó
                totalAttackBonus += item.attackBonus;
                totalDefenseBonus += item.defenseBonus;
                totalHealthBonus += item.healthBonus;
                totalManaBonus += item.manaBonus;
                totalSpeedBonus += item.speedBonus;
            }
        }
        // Gửi tất cả các bonus đã tính được cho PlayerStats
        playerStats.UpdateEquipmentBonuses(totalHealthBonus, totalManaBonus, totalAttackBonus, totalDefenseBonus, totalSpeedBonus);
    }
}