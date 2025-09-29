// // using System.Collections.Generic;
// // using UnityEngine;

// // public class PlayerInventory : MonoBehaviour
// // {
// //     public static PlayerInventory instance;

// //     // Thay đổi từ List<ItemData> sang Dictionary<ItemData, int>
// //     public Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

// //     public delegate void OnInventoryChanged();
// //     public OnInventoryChanged onInventoryChangedCallback;

// //     void Awake()
// //     {
// //         instance = this;
// //     }

// //     public void AddItem(ItemData item)
// //     {
// //         // Nếu vật phẩm đã có trong túi đồ
// //         if (items.ContainsKey(item))
// //         {
// //             items[item]++; // Tăng số lượng lên 1
// //         }
// //         // Nếu chưa có
// //         else
// //         {
// //             items.Add(item, 1); // Thêm vào với số lượng là 1
// //         }
// //         Debug.Log("Added " + item.itemName + ". Total: " + items[item]);

// //         // Gửi tín hiệu rằng túi đồ đã thay đổi
// //         if (onInventoryChangedCallback != null)
// //         {
// //             onInventoryChangedCallback.Invoke();
// //         }
// //     }

// //     // (Bạn có thể thêm hàm RemoveItem tương tự nếu cần)
// // }

// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerInventory : MonoBehaviour
// {
//     public static PlayerInventory instance;
//     public Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();
//     public delegate void OnInventoryChanged();
//     public OnInventoryChanged onInventoryChangedCallback;

//     private PlayerStats playerStats;

//     void Awake()
//     {
//         instance = this;
//         // Lấy PlayerStats ngay khi được tạo ra để đảm bảo nó luôn có sẵn
//         playerStats = GetComponent<PlayerStats>();
//     }

//     // public void UseItem(ItemData item)
//     // {
//     //     if (items.ContainsKey(item))
//     //     {
//     //         if (item.itemType == ItemType.Consumable)
//     //         {
//     //             // Thêm một kiểm tra an toàn
//     //             if (playerStats != null)
//     //             {
//     //                 playerStats.Heal(item.healthRestore);
//     //                 playerStats.RestoreMana(item.manaRestore);
//     //                 RemoveItem(item);
//     //             }
//     //             else
//     //             {
//     //                 Debug.LogError("PlayerStats not found on Player!");
//     //             }
//     //         }
//     //     }
//     // }

//     public void UseItem(ItemData item)
// {
//     Debug.Log("<color=cyan>--- UseItem called for: " + item.itemName + " ---</color>");

//     if (playerStats == null)
//     {
//         Debug.LogError("PlayerStats reference is NULL in PlayerInventory! Cannot heal.");
//         return;
//     }

//     if (items.ContainsKey(item))
//     {
//         if (item.itemType == ItemType.Consumable)
//         {
//             Debug.Log("Item is a consumable. Health to restore: " + item.healthRestore);

//             playerStats.Heal(item.healthRestore);
//             playerStats.RestoreMana(item.manaRestore);

//             RemoveItem(item);
//             Debug.Log("<color=cyan>--- UseItem finished successfully ---</color>");
//         }
//         else
//         {
//             Debug.Log("Item " + item.itemName + " is not a consumable.");
//         }
//     }
//     else
//     {
//         Debug.LogWarning("Tried to use item " + item.itemName + " but it's not in the dictionary.");
//     }
// }
//     // Hàm sử dụng vật phẩm, được gọi từ InventorySlotUI
//     // public void UseItem(ItemData item)
//     // {
//     //     // Kiểm tra xem vật phẩm có trong túi không
//     //     if (items.ContainsKey(item))
//     //     {
//     //         if (item.itemType == ItemType.Consumable)
//     //         {
//     //             Debug.Log("Using " + item.itemName + ". Health Restore: " + item.healthRestore);

//     //             // Gọi hàm hồi máu/năng lượng từ PlayerStats
//     //             playerStats.Heal(item.healthRestore);
//     //             playerStats.RestoreMana(item.manaRestore);

//     //             // Tiêu hao vật phẩm
//     //             RemoveItem(item);
//     //         }
//     //     }
//     // }

//     // Các hàm AddItem và RemoveItem giữ nguyên
//     public void AddItem(ItemData item)
//     {
//         if (items.ContainsKey(item)) { items[item]++; }
//         else { items.Add(item, 1); }
//         if (onInventoryChangedCallback != null) { onInventoryChangedCallback.Invoke(); }
//     }

//     public void RemoveItem(ItemData item)
//     {
//         if (items.ContainsKey(item))
//         {
//             items[item]--;
//             if (items[item] <= 0) { items.Remove(item); }
//             if (onInventoryChangedCallback != null) { onInventoryChangedCallback.Invoke(); }
//         }
//     }
//     // Hàm trừ vật phẩm
//     // public void RemoveItem(ItemData item)
//     // {
//     //     if (items.ContainsKey(item))
//     //     {
//     //         items[item]--; // Giảm số lượng đi 1
//     //         if (items[item] <= 0)
//     //         {
//     //             items.Remove(item); // Nếu hết thì xóa khỏi túi đồ
//     //         }

//     //         // Gửi tín hiệu túi đồ đã thay đổi để UI cập nhật lại
//     //         if (onInventoryChangedCallback != null)
//     //         {
//     //             onInventoryChangedCallback.Invoke();
//     //         }
//     //     }
//     // }
// }

using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();
    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    private PlayerStats playerStats;

    void Awake()
    {
        instance = this;
        playerStats = GetComponent<PlayerStats>();
    }

    public void UseItem(ItemData item)
    {
        if (items.ContainsKey(item))
        {
            if (item.itemType == ItemType.Consumable)
            {
                if (playerStats != null)
                {
                    Debug.Log("Using " + item.itemName + ". Health Restore: " + item.healthRestore);
                    playerStats.Heal(item.healthRestore);
                    playerStats.RestoreMana(item.manaRestore);
                    RemoveItem(item);
                }
                else
                {
                    Debug.LogError("PlayerStats not found on Player!");
                }
            }
        }
    }
    
    public void AddItem(ItemData item)
    {
        if (items.ContainsKey(item)) { items[item]++; }
        else { items.Add(item, 1); }
        if (onInventoryChangedCallback != null) { onInventoryChangedCallback.Invoke(); }
    }

    public void RemoveItem(ItemData item)
    {
        if (items.ContainsKey(item))
        {
            items[item]--;
            if (items[item] <= 0) { items.Remove(item); }
            if (onInventoryChangedCallback != null) { onInventoryChangedCallback.Invoke(); }
        }
    }
}