// using UnityEngine;

// [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
// public class ItemData : ScriptableObject
// {
//     public string itemName = "New Item";
//     public Sprite icon = null;
//     public string description = "Item Description";
// }
using UnityEngine;

// Enum để định nghĩa các loại vật phẩm
public enum ItemType { Consumable, Equipment, Material }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon = null;
    public string description = "Item Description";

    [Header("Item Properties")]
    public ItemType itemType; // Loại vật phẩm

    [Header("Consumable Stats")]
    public int healthRestore; // Lượng máu hồi
    public int manaRestore; // Lượng năng lượng hồi
}