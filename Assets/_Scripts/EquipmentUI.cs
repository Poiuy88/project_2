using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipmentUI : MonoBehaviour
{
    public Transform slotHolder;
    private EquipmentSlotUI[] slots;

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChanged -= UpdateAllSlots;
            EquipmentManager.instance.onEquipmentChanged += UpdateAllSlots;
        }
        FindSlotsInCurrentScene();
    }

    void FindSlotsInCurrentScene()
    {
        if (SceneManager.GetActiveScene().name == "LoginScene") return;
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            slotHolder = canvas.transform.Find("EquipmentPanel/EquipmentSlotHolder");
            if (slotHolder != null)
            {
                slots = slotHolder.GetComponentsInChildren<EquipmentSlotUI>();
                UpdateAllSlots(null, null);
            }
        }
    }

    void UpdateAllSlots(ItemData newItem, ItemData oldItem)
    {
        if (slots == null) return;
        foreach (EquipmentSlotUI slot in slots)
        {
            int slotIndex = (int)slot.equipSlot;
            slot.DisplayItem(EquipmentManager.instance.currentEquipment[slotIndex]);
        }
    }
}