using UnityEngine;
using UnityEngine.SceneManagement;

public class FastTravelNPC : MonoBehaviour
{
    public GameObject fastTravelPanel; // Panel chọn điểm đến
    public GameObject interactionPrompt; // Dòng chữ "Nhấn E để nói chuyện"

    private bool playerInRange = false;
    private PlayerStats playerStats; // Để kiểm tra và trừ tiền của người chơi

    void Start()
    {
        fastTravelPanel.SetActive(false);
        interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Thay vì gọi DialogueManager, chúng ta sẽ mở bảng dịch chuyển
            fastTravelPanel.SetActive(true);
        }
    }

    // Hàm này sẽ được gọi bởi nút "Về Ngôi Làng"
    public void TravelToVillage()
    {
        // --- Định nghĩa thông tin chuyến đi ---
        int cost = 5; // Phí di chuyển
        string sceneName = "VillageMap";
        string spawnPointName = "SpawnPoint_FromMountain";

        // --- Kiểm tra và thực hiện ---
        if (playerStats.SpendCoins(cost)) // Dùng hàm SpendCoins mới
        {
            Debug.Log("Payment successful. Traveling to " + sceneName);
            GameManager.nextSpawnPointName = spawnPointName;
            SceneManager.LoadScene(sceneName);
            fastTravelPanel.SetActive(false); // Đóng bảng sau khi dịch chuyển
        }
        else
        {
            Debug.Log("Payment failed. Not enough coins.");
            // Sau này có thể hiện một thông báo lỗi trên UI
        }
    }

    // Hàm này được gọi bởi nút "Đóng"
    public void ClosePanel()
    {
        fastTravelPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionPrompt.SetActive(true);
            // Lấy component PlayerStats khi người chơi đến gần
            playerStats = other.GetComponent<PlayerStats>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionPrompt.SetActive(false);
            fastTravelPanel.SetActive(false); // Tự động đóng bảng khi người chơi đi xa
            playerStats = null;
        }
    }
}