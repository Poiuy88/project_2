using UnityEngine;

public enum ItemType { Consumable, Equipment, Material }
public enum EquipmentSlot { Weapon, Armor, Helmet, Boots }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon = null;
    public string description = "Item Description";
    public int price;

    [Header("Item Properties")]
    public ItemType itemType;

    [Header("Equipment Properties")]
    public EquipmentSlot equipSlot;
    // Các chỉ số cộng thêm
    public int attackBonus;
    public int defenseBonus;
    public int healthBonus;
    public int manaBonus;
    public float speedBonus; // Tốc chạy dùng số lẻ nên là float

    [Header("Consumable Stats")]
    public int healthRestore;
    public int manaRestore;
}