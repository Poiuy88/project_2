using UnityEngine;
using TMPro;
using UnityEngine.UI; // <-- THÊM THƯ VIỆN NÀY ĐỂ DÙNG "Button"

public class SkillTrainerNPC : MonoBehaviour
{
    public GameObject skillLearnPanel;
    public GameObject interactionPrompt;
    public TextMeshProUGUI feedbackText; 

    private bool playerInRange = false;
    private PlayerStats playerStats;

    // --- BẮT ĐẦU CODE MỚI ---
    private Button learnButton; // Biến lưu nút "Học"
    private Button closeButton; // Biến lưu nút "Đóng"
    private bool isPanelInitialized = false; // Biến cờ
    // --- KẾT THÚC CODE MỚI ---

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Tự động kết nối các nút khi mở bảng
            InitializePanelButtons(); // <-- GỌI HÀM MỚI

            skillLearnPanel.SetActive(true);
            UpdateFeedbackText(""); 
        }
    }

    // --- BẮT ĐẦU CODE MỚI ---
    // Hàm này sẽ tìm và gán sự kiện cho các nút
    void InitializePanelButtons()
    {
        // Chỉ chạy 1 lần duy nhất
        if (isPanelInitialized || skillLearnPanel == null)
        {
            return;
        }

        // 1. Tìm nút "Học chiêu" (Giả sử tên là "LearnButton")
        // Bạn PHẢI kiểm tra tên chính xác trong Hierarchy của Prefab
        Transform learnBtnTransform = skillLearnPanel.transform.Find("LearnButton");
        if (learnBtnTransform != null)
        {
            learnButton = learnBtnTransform.GetComponent<Button>();
            learnButton.onClick.RemoveAllListeners(); // Xóa sự kiện cũ (nếu có)
            learnButton.onClick.AddListener(LearnFireballSkill); // Gán hàm "Học"
        }
        else
        {
            Debug.LogError("SkillTrainerNPC: Không tìm thấy 'LearnButton' bên trong 'SkillLearnPanel'!");
        }

        // 2. Tìm nút "Đóng" (Giả sử tên là "CloseButton")
        Transform closeBtnTransform = skillLearnPanel.transform.Find("CloseButton");
        if (closeBtnTransform != null)
        {
            closeButton = closeBtnTransform.GetComponent<Button>();
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(ClosePanel); // Gán hàm "Đóng"
        }
        else
        {
            Debug.LogError("SkillTrainerNPC: Không tìm thấy 'CloseButton' bên trong 'SkillLearnPanel'!");
        }

        isPanelInitialized = true; // Đánh dấu là đã gán xong
    }
    // --- KẾT THÚC CODE MỚI ---

    // (Hàm LearnFireballSkill giữ nguyên)
    public void LearnFireballSkill()
    {
        int requiredLevel = 5;
        int cost = 50;

        if (GameManager.hasLearnedFireball)
        {
            UpdateFeedbackText("Cháu đã học chiêu này rồi!");
            return;
        }

        if (playerStats.playerLevel >= requiredLevel)
        {
            if (playerStats.SpendCoins(cost))
            {
                GameManager.hasLearnedFireball = true;
                UpdateFeedbackText("Chúc mừng! Cháu đã học được Quả Cầu Lửa!");
            }
            else
            {
                UpdateFeedbackText("Không đủ xu!");
            }
        }
        else
        {
            UpdateFeedbackText("Cần đạt Level " + requiredLevel + "!");
        }
    }

    // (Các hàm còn lại giữ nguyên)
    public void ClosePanel() { skillLearnPanel.SetActive(false); }
    void UpdateFeedbackText(string message) { if (feedbackText != null) feedbackText.text = message; }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionPrompt.SetActive(true);
            playerStats = other.GetComponent<PlayerStats>();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionPrompt.SetActive(false);
            skillLearnPanel.SetActive(false);
        }
    }
}