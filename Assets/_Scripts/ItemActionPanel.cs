using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemActionPanel : MonoBehaviour
{
    public static ItemActionPanel instance;
    void Awake() { instance = this; }

    public GameObject panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI statsText;
    public Button equipButton;
    public Button unequipButton;

    private ItemData currentItem;

    void Start()
    {
        panel.SetActive(false);
        equipButton.onClick.AddListener(EquipItem);
        unequipButton.onClick.AddListener(UnequipItem);
    }

    public void OpenPanelForInventoryItem(ItemData item)
    {
        currentItem = item;
        panel.SetActive(true);
        nameText.text = item.itemName;
        statsText.text = "Tấn công: " + item.attackBonus + "\nPhòng thủ: " + item.defenseBonus;

        equipButton.gameObject.SetActive(true);
        unequipButton.gameObject.SetActive(false);
    }

    public void OpenPanelForEquipmentItem(ItemData item)
    {
        currentItem = item;
        panel.SetActive(true);
        nameText.text = item.itemName;
        statsText.text = "Tấn công: " + item.attackBonus + "\nPhòng thủ: " + item.defenseBonus;

        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(true);
    }

    void EquipItem()
    {
        if (currentItem.itemType == ItemType.Equipment)
        {
            EquipmentManager.instance.Equip(currentItem);
        }
        panel.SetActive(false);
    }

    void UnequipItem()
    {
        EquipmentManager.instance.Unequip((int)currentItem.equipSlot);
        panel.SetActive(false);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}