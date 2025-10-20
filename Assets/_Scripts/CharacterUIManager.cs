
using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    public static CharacterUIManager instance;
    void Awake() { instance = this; }
    public GameObject characterHubPanel;
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public ItemActionPanel itemActionPanel;


    void Start()
    {
        CloseAllPanels();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            characterHubPanel.SetActive(!characterHubPanel.activeSelf);
            if (!characterHubPanel.activeSelf)
            {
                CloseAllPanels();
            }
        }
    }

    public void OpenInventory()
    {
        Debug.Log("Hàm OpenInventory() được gọi.");
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true);
            Debug.Log("InventoryPanel đã được bật.");
            characterHubPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("LỖI: Tham chiếu InventoryPanel trong CharacterUIManager bị NULL!");
        }
        if (equipmentPanel != null) equipmentPanel.SetActive(false);
    }

    public void OpenEquipment()
    {
        Debug.Log("Hàm OpenEquipment() được gọi.");
        if (equipmentPanel != null)
        {
            equipmentPanel.SetActive(true);
            Debug.Log("EquipmentPanel đã được bật.");
            characterHubPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("LỖI: Tham chiếu EquipmentPanel trong CharacterUIManager bị NULL!");
        }
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }

    public void CloseAllPanels()
    {
        if (characterHubPanel != null) characterHubPanel.SetActive(false);
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (equipmentPanel != null) equipmentPanel.SetActive(false);
    }
    public void BackToHubPanel()
    {
        Debug.Log("Quay lại Hub Panel.");
        inventoryPanel.SetActive(false);
        equipmentPanel.SetActive(false);
        characterHubPanel.SetActive(true);
    }
}