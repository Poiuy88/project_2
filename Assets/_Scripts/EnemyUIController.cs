using UnityEngine;
using UnityEngine.UI; // Cần cho Image
using TMPro;

public class EnemyUIController : MonoBehaviour
{
    public GameObject healthBarPrefab;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private Image healthBarFill;
    // private TextMeshProUGUI levelText;
    private EnemyHealth enemyHealth;
    private Transform healthBarInstance;
    private Canvas healthBarCanvas;
    void Start()
    {
        // Tìm component máu trước
        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component not found on " + gameObject.name);
            return; // Dừng lại nếu không có máu
        }

        // Tạo thanh máu
        GameObject hbInstance = Instantiate(healthBarPrefab, transform.position + offset, Quaternion.identity, transform);
        healthBarInstance = hbInstance.transform;
        healthBarCanvas = hbInstance.GetComponent<Canvas>(); // Lấy component Canvas

        // Tìm thanh fill một cách an toàn hơn
        Transform fillTransform = healthBarInstance.Find("HealthBarBackground/HealthBarFill");
        if (fillTransform != null)
        {
            healthBarFill = fillTransform.GetComponent<Image>();
        }
        else
        {
            Debug.LogError("Could not find 'HealthBarBackground/HealthBarFill' inside the health bar prefab!", healthBarInstance.gameObject);
        }

        // Tìm Text Level (nếu có)
        // Transform levelTextTransform = healthBarInstance.Find("LevelText");
        // if (levelTextTransform != null) levelText = levelTextTransform.GetComponent<TextMeshProUGUI>();

        // (Có thể thêm code để lấy Level của quái vật ở đây và cập nhật levelText)

        // Đặt Sorting Order cho Canvas để nó hiển thị đúng (quan trọng)
        if (healthBarCanvas != null)
        {
             healthBarCanvas.sortingOrder = 10; // Đặt một giá trị cao hơn các sprite nền
        }

        UpdateHealthBar(); // Cập nhật lần đầu
    }

    void Update()
    {
        UpdateHealthBar();
        // Xoay thanh máu theo camera (giữ nguyên)
        // if(healthBarInstance != null && Camera.main != null) { /*...*/ }
    }

    void UpdateHealthBar()
    {
        // Thêm các kiểm tra null để đảm bảo an toàn
        if (enemyHealth == null || healthBarFill == null || healthBarInstance == null) return;

        // Logic ẩn/hiện và cập nhật fillAmount giữ nguyên
        bool shouldShow = enemyHealth.currentHealth < enemyHealth.maxHealth && enemyHealth.currentHealth > 0;
        healthBarInstance.gameObject.SetActive(shouldShow);

        if (shouldShow)
        {
            float fillValue = (float)enemyHealth.currentHealth / enemyHealth.maxHealth;
            healthBarFill.fillAmount = fillValue;
            // Debug.Log($"Updating health bar for {gameObject.name}: Fill Amount = {fillValue}"); // Bỏ comment dòng này nếu cần gỡ lỗi
        }
    }   
}