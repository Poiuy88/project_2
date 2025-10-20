
using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    public static CharacterUIManager instance;
    void Awake() { instance = this; }
    public GameObject characterHubPanel;
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public ItemActionPanel itemActionPanel;
    public GameObject statsPanel;


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
        inventoryPanel.SetActive(true);
        equipmentPanel.SetActive(false);
        statsPanel.SetActive(false);
        characterHubPanel.SetActive(false);
    }

    public void OpenEquipment()
    {
        inventoryPanel.SetActive(false);
        equipmentPanel.SetActive(true);
        statsPanel.SetActive(false);
        characterHubPanel.SetActive(false);
    }

    public void CloseAllPanels()
    {
        characterHubPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        equipmentPanel.SetActive(false);
        statsPanel.SetActive(false);
    }
    public void BackToHubPanel()
{
    characterHubPanel.SetActive(true);
    inventoryPanel.SetActive(false);
    equipmentPanel.SetActive(false);
    statsPanel.SetActive(false);
}
    public void OpenStatsPanel()
    {
        statsPanel.SetActive(true);
        inventoryPanel.SetActive(false);
        equipmentPanel.SetActive(false);
    }
}