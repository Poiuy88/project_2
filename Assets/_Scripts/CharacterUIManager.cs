// using UnityEngine;

// public class CharacterUIManager : MonoBehaviour
// {
//     public GameObject characterHubPanel;
//     public GameObject inventoryPanel;
//     public GameObject equipmentPanel;

//     void Start()
//     {
//         CloseAllPanels();
//     }

//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.T))
//         {
//             // Bật/tắt panel trung tâm
//             characterHubPanel.SetActive(!characterHubPanel.activeSelf);

//             // Nếu tắt panel trung tâm, tắt luôn các panel con
//             if (!characterHubPanel.activeSelf)
//             {
//                 CloseAllPanels();
//             }
//         }
//     }

//     public void OpenInventory()
//     {
//         inventoryPanel.SetActive(true);
//         equipmentPanel.SetActive(false);
//     }

//     public void OpenEquipment()
//     {
//         inventoryPanel.SetActive(false);
//         equipmentPanel.SetActive(true);
//     }

//     public void CloseAllPanels()
//     {
//         characterHubPanel.SetActive(false);
//         inventoryPanel.SetActive(false);
//         equipmentPanel.SetActive(false);
//     }
// }
using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    public GameObject characterHubPanel;
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;

    void Start()
    {
        CloseAllPanels();
        // Đảm bảo các panel con cũng tắt từ đầu
        if(inventoryPanel != null) inventoryPanel.SetActive(false);
        if(equipmentPanel != null) equipmentPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Phím 'T' được nhấn.");
            characterHubPanel.SetActive(!characterHubPanel.activeSelf);

            if (!characterHubPanel.activeSelf)
            {
                Debug.Log("CharacterHubPanel tắt, đóng tất cả các panel con.");
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