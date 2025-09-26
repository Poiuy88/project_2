using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện cần thiết để quản lý Scene

public class InteractivePortal : MonoBehaviour
{
    // Biến để bạn điền tên Scene muốn chuyển đến trong Inspector
    public string sceneToLoad; 
    
    // Biến để bạn kéo thả đối tượng UI Text "Nhấn [E] để vào"
    public GameObject interactionPrompt; 
    public string spawnPointName;

    // Biến để kiểm tra xem người chơi có đang ở trong vùng tương tác không
    private bool playerInRange = false; 

    void Start()
    {
        // Đảm bảo dòng chữ thông báo được tắt đi khi bắt đầu
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        // Nếu người chơi đang ở trong vùng VÀ nhấn phím E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Lưu lại tên điểm spawn trước khi chuyển cảnh
            GameManager.nextSpawnPointName = spawnPointName;
            Debug.Log("<color=cyan>Portal: Setting next spawn point to: " + GameManager.nextSpawnPointName + "</color>");

            // Thì thực hiện lệnh chuyển sang scene đã định
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // Hàm này được Unity tự động gọi khi có đối tượng đi vào vùng trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng đó có phải là "Player" không
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Đánh dấu là người chơi đang ở trong vùng
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true); // Bật dòng chữ thông báo
            }
        }
    }

    // Hàm này được Unity tự động gọi khi đối tượng đi ra khỏi vùng trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng đó có phải là "Player" không
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Đánh dấu là người chơi đã rời đi
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false); // Tắt dòng chữ thông báo
            }
        }
    }
}