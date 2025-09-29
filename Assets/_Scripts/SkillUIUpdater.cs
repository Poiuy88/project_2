// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class SkillUIUpdater : MonoBehaviour
// {
//     public Image cooldownMask;
//     public TextMeshProUGUI cooldownText;
//     public Button skillButton;

//     private PlayerSkills playerSkills;

//     void Start()
//     {
//         // Tự động tìm PlayerSkills khi bắt đầu
//         playerSkills = FindObjectOfType<PlayerSkills>();
//         if (playerSkills != null)
//         {
//             // Gắn sự kiện OnClick cho nút để nó gọi hàm CastFireball
//             skillButton.onClick.AddListener(playerSkills.CastFireball);
//         }
//     }

//     void Update()
//     {
//         if (playerSkills == null) return;

//         // Chỉ hiển thị nút khi người chơi đã học chiêu
//         skillButton.gameObject.SetActive(GameManager.hasLearnedFireball);

//         float currentCooldown = playerSkills.GetCurrentCooldown();
//         float maxCooldown = playerSkills.fireballCooldown;

//         if (currentCooldown > 0)
//         {
//             cooldownMask.fillAmount = currentCooldown / maxCooldown;
//             cooldownText.text = Mathf.Ceil(currentCooldown).ToString();
//             cooldownText.enabled = true;
//         }
//         else
//         {
//             cooldownMask.fillAmount = 0;
//             cooldownText.enabled = false;
//         }
//     }
// }
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Thêm thư viện SceneManagement

public class SkillUIUpdater : MonoBehaviour
{
    // Các biến này sẽ được tự động tìm, không cần public nữa
    private Image cooldownMask;
    private TextMeshProUGUI cooldownText;
    private Button skillButton;

    private PlayerSkills playerSkills;

    // Đăng ký và hủy đăng ký sự kiện load scene
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Hàm được gọi mỗi khi một scene mới được tải xong
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cố gắng tìm lại tất cả các thành phần cần thiết trong scene mới
        FindAndSetupElements();
    }

    // Hàm tìm kiếm và thiết lập các thành phần UI
    void FindAndSetupElements()
    {
        // Không chạy logic này ở màn hình Login
        if (SceneManager.GetActiveScene().name == "LoginScene") return;

        // Luôn tìm lại PlayerSkills vì player có thể được khởi tạo sau
        playerSkills = FindObjectOfType<PlayerSkills>();
        
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            // Tìm nút bấm bằng tên
            Transform buttonTransform = canvas.transform.Find("FireballSkill_Button");
            if (buttonTransform != null)
            {
                skillButton = buttonTransform.GetComponent<Button>();
                cooldownMask = skillButton.transform.Find("CooldownMask").GetComponent<Image>();
                cooldownText = skillButton.transform.Find("CooldownText").GetComponent<TextMeshProUGUI>();

                // Kết nối lại sự kiện OnClick cho nút bấm
                if (playerSkills != null)
                {
                    skillButton.onClick.RemoveAllListeners(); // Xóa listener cũ
                    skillButton.onClick.AddListener(playerSkills.CastFireball);
                }
            }
            else
            {
                // Nếu không tìm thấy nút trong scene này, đặt nó là null
                skillButton = null;
            }
        }
    }

    void Update()
    {
        // Nếu không có nút kỹ năng hoặc người chơi trong scene này, không làm gì cả
        if (playerSkills == null || skillButton == null)
        {
            return;
        }

        // 1. Luôn cập nhật trạng thái Bật/Tắt của nút
        bool shouldBeActive = GameManager.hasLearnedFireball;
        if (skillButton.gameObject.activeSelf != shouldBeActive)
        {
            skillButton.gameObject.SetActive(shouldBeActive);
        }

        // Nếu nút đang bị tắt, không cần cập nhật hồi chiêu
        if (!shouldBeActive) return;

        // 2. Cập nhật hiển thị thời gian hồi chiêu
        float currentCooldown = playerSkills.GetCurrentCooldown();
        float maxCooldown = playerSkills.fireballCooldown;

        if (currentCooldown > 0)
        {
            cooldownMask.fillAmount = currentCooldown / maxCooldown;
            cooldownText.text = Mathf.Ceil(currentCooldown).ToString();
            cooldownText.enabled = true;
        }
        else
        {
            cooldownMask.fillAmount = 0;
            cooldownText.enabled = false;
        }
    }
}