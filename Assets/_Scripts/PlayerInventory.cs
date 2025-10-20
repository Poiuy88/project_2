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