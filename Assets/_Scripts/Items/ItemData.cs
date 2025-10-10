
using UnityEngine;

public enum ItemType { Consumable, Equipment, Material }
public enum EquipmentSlot { Weapon, Armor } // Các ô trang bị, có thể thêm Helmet, Boots...

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int price; // Giá bán của vật phẩm
    public string itemName = "New Item";
    public Sprite icon = null;
    public string description = "Item Description";

    [Header("Item Properties")]
    public ItemType itemType;

    [Header("Equipment Stats")]
    public EquipmentSlot equipSlot; // Vị trí mặc trang bị này
    public int attackBonus; // Công cộng thêm
    public int defenseBonus; // Thủ cộng thêm

    [Header("Consumable Stats")]
    public int healthRestore;
    public int manaRestore;
}