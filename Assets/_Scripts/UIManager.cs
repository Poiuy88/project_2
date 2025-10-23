using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InventoryUI), typeof(SkillUIUpdater))]
public class UIManager : MonoBehaviour
{
    private Slider healthBar;
    private Slider manaBar;
    private Slider expBar;
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI coinText;

    private PlayerStats playerStats;
    private InventoryUI inventoryUI;
    private SkillUIUpdater skillUIUpdater;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerStats();
        FindUIElements();
    }

    void FindPlayerStats()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null && SceneManager.GetActiveScene().name != "LoginScene")
        {
            Debug.LogError("UIManager: Could not find PlayerStats in the scene!");
        }
    }

    void FindUIElements()
    {
        if (SceneManager.GetActiveScene().name == "LoginScene") return;

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("UIManager: Could not find a Canvas in this scene! UI will not work.");
            return;
        }

        // --- SỬA LỖI Ở CÁC DÒNG GỌI HÀM DƯỚI ĐÂY ---
        // Phải truyền vào canvas.transform thay vì chỉ canvas
        healthBar = FindUIComponent<Slider>(canvas.transform, "HealthBar");
        manaBar = FindUIComponent<Slider>(canvas.transform, "ManaBar");
        expBar = FindUIComponent<Slider>(canvas.transform, "ExpBar");
        levelText = FindUIComponent<TextMeshProUGUI>(canvas.transform, "LevelText");

        Transform coinGroup = canvas.transform.Find("CoinDisplayGroup");
        if (coinGroup != null)
        {
            // Phải truyền vào coinGroup (là Transform)
            coinText = FindUIComponent<TextMeshProUGUI>(coinGroup, "CoinText");
        } else {
             Debug.LogError("UIManager: Could not find 'CoinDisplayGroup' in Canvas!");
        }
        // --- KẾT THÚC SỬA LỖI ---

        inventoryUI = GetComponent<InventoryUI>();
        skillUIUpdater = GetComponent<SkillUIUpdater>();

        UpdateBaseUI();
    }

    // Hàm tiện ích (giữ nguyên)
    T FindUIComponent<T>(Transform parent, string name) where T : Component
    {
        Transform element = parent.Find(name);
        if (element == null)
        {
            Debug.LogError($"UIManager: Could not find UI element named '{name}' under '{parent.name}'!");
            return null;
        }
        T component = element.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError($"UIManager: UI element '{name}' does not have the required component '{typeof(T).Name}'!");
        }
        return component;
    }

    void Update()
    {
        if (playerStats != null)
        {
            UpdateBaseUI();
        }
    }

    void UpdateBaseUI()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = playerStats.maxHealth;
            healthBar.value = playerStats.currentHealth;
        }
        if (manaBar != null)
        {
            manaBar.maxValue = playerStats.maxMana;
            manaBar.value = playerStats.currentMana;
        }
        if (expBar != null)
        {
            expBar.maxValue = playerStats.expToNextLevel;
            expBar.value = playerStats.currentExp;
        }
        if (levelText != null)
        {
            levelText.text = "Level: " + playerStats.playerLevel;
        }
        if (coinText != null)
        {
            coinText.text = playerStats.coins.ToString();
        }
    }
}