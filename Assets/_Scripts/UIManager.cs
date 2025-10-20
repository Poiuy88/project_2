using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Thêm thư viện SceneManagement
public class UIManager : MonoBehaviour
{
    // Các biến vẫn giữ nguyên
    public PlayerStats playerStats;
    public Slider healthBar;
    public Slider manaBar;
    public Slider expBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI coinText;

    // Thêm hàm OnEnable và OnDisable
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Hàm này sẽ được gọi mỗi khi một scene mới được load xong
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIElements();
    }

    void FindUIElements()
    {
        Debug.Log("UIManager is searching for UI elements in the new scene.");
        // Tìm Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Could not find a Canvas in the new scene!");
            return;
        }

        // Tìm các thành phần UI bằng tên của chúng bên trong Canvas
        // Đảm bảo tên các đối tượng UI của bạn trong Hierarchy là chính xác
        healthBar = canvas.transform.Find("HealthBar").GetComponent<Slider>();
        manaBar = canvas.transform.Find("ManaBar").GetComponent<Slider>();
        expBar = canvas.transform.Find("ExpBar").GetComponent<Slider>();
        levelText = canvas.transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        coinText = canvas.transform.Find("CoinText").GetComponent<TextMeshProUGUI>();

        // Tìm PlayerStats (có thể nó cũng cần được tìm lại)
        playerStats = FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        // Code cập nhật UI giữ nguyên
        if (playerStats != null && healthBar != null && manaBar != null)
        {
            healthBar.maxValue = playerStats.maxHealth;
            healthBar.value = playerStats.currentHealth;
            manaBar.maxValue = playerStats.maxMana;
            manaBar.value = playerStats.currentMana;
        }

        if (playerStats != null && expBar != null && levelText != null && coinText != null)
        {
            expBar.maxValue = playerStats.expToNextLevel;
            expBar.value = playerStats.currentExp;
            levelText.text = "Level: " + playerStats.playerLevel;
            coinText.text = "Coins: " + playerStats.coins.ToString();
        }
    }
}