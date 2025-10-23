using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUIController : MonoBehaviour
{
    public GameObject healthBarPrefab;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private Image healthBarFill;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI levelText;
    private EnemyHealth enemyHealth;
    private Transform healthBarInstance;
    private Canvas healthBarCanvas;
    private Vector3 initialHealthBarScale;
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth == null) return;

        GameObject hbInstance = Instantiate(healthBarPrefab, transform.position + offset, Quaternion.identity, transform);
        healthBarInstance = hbInstance.transform;
        healthBarCanvas = hbInstance.GetComponent<Canvas>();
        initialHealthBarScale = healthBarInstance.localScale;

        // Tìm thanh fill và health text (như cũ)
        Transform fillTransform = healthBarInstance.Find("HealthBarBackground/HealthBarFill");
        if (fillTransform != null) healthBarFill = fillTransform.GetComponent<Image>();
        Transform healthTextTransform = healthBarInstance.Find("HealthBarBackground/HealthText");
        if (healthTextTransform != null) healthText = healthTextTransform.GetComponent<TextMeshProUGUI>();

        // --- TÌM VÀ CẬP NHẬT LEVEL TEXT ---
        Transform levelTextTransform = healthBarInstance.Find("LevelText"); // << TÌM LEVEL TEXT
        if (levelTextTransform != null)
        {
            levelText = levelTextTransform.GetComponent<TextMeshProUGUI>();
            // Cập nhật nội dung Text ngay khi tìm thấy
            levelText.text = "Lv. " + enemyHealth.level; // << CẬP NHẬT LEVEL
        }
        else
        {
            Debug.LogWarning("Could not find 'LevelText' inside the health bar prefab!", healthBarInstance.gameObject);
        }
        // --- KẾT THÚC TÌM VÀ CẬP NHẬT ---

        if (healthBarCanvas != null) healthBarCanvas.sortingOrder = 10;
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
        // Logic chống lật (giữ nguyên)
        if (healthBarInstance != null)
        {
            if (transform.localScale.x < 0) healthBarInstance.localScale = new Vector3(-initialHealthBarScale.x, initialHealthBarScale.y, initialHealthBarScale.z);
            else healthBarInstance.localScale = initialHealthBarScale;
        }
        // Logic xoay camera (giữ nguyên)
        // if(healthBarInstance != null && Camera.main != null) { /*...*/ }
    }

    void UpdateHealthBar()
    {
        // Thêm các kiểm tra null để đảm bảo an toàn
        if (enemyHealth == null || healthBarInstance == null) return;

        // --- LOGIC HIỂN THỊ MỚI ---
        // Chỉ hiển thị thanh máu khi:
        // 1. Máu không đầy (currentHealth < maxHealth)
        // 2. VÀ Máu vẫn còn lớn hơn 0 (currentHealth > 0)
        bool shouldShow = enemyHealth.currentHealth < enemyHealth.maxHealth && enemyHealth.currentHealth > 0;

        // Bật hoặc tắt toàn bộ đối tượng thanh máu dựa trên điều kiện trên
        healthBarInstance.gameObject.SetActive(shouldShow);
        // --- KẾT THÚC LOGIC HIỂN THỊ MỚI ---

        // Chỉ cập nhật fillAmount và Text nếu thanh máu đang được hiển thị
        if (shouldShow)
        {
            // Cập nhật thanh fill (nếu tìm thấy)
            if (healthBarFill != null)
            {
                healthBarFill.fillAmount = (float)enemyHealth.currentHealth / enemyHealth.maxHealth;
            }

            // Cập nhật Text hiển thị máu (nếu tìm thấy)
            if (healthText != null)
            {
                healthText.text = enemyHealth.currentHealth + " / " + enemyHealth.maxHealth;
            }
        }
    }
    public void HideHealthBarInstantly()
    {
        if (healthBarInstance != null)
        {
            healthBarInstance.gameObject.SetActive(false);
        }
    }
}