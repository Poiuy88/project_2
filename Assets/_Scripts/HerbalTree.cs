using UnityEngine;
using TMPro; // Nhớ thêm dòng này để làm việc với TextMeshPro

public class HerbalTree : MonoBehaviour
{
    public GameObject interactionPrompt; // Biến để kéo thả UI Text vào
    public float cooldownTime = 10f; // Thời gian hồi, 10 giây để test (bạn có thể đổi thành 300f cho 5 phút)

    private bool playerInRange = false; // Kiểm tra xem người chơi có ở trong vùng không
    private float nextHarvestTime = 0f; // Mốc thời gian có thể thu hoạch lần tiếp theo
    private PlayerInventory playerInventory; // Để tham chiếu đến túi đồ của người chơi
    public ItemData herbalFruitData;

    void Update()
    {
        // Nếu người chơi ở trong vùng và nhấn phím E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Kiểm tra xem đã đến lúc thu hoạch chưa
            if (Time.time >= nextHarvestTime)
            {
                Harvest();
            }
            else
            {
                Debug.Log("Cây chưa sẵn sàng để thu hoạch!");
            }
        }

        // Cập nhật trạng thái của dòng chữ prompt
        if (playerInRange)
        {
            // Nếu người chơi ở trong vùng và cây đã sẵn sàng, hiện prompt
            interactionPrompt.SetActive(Time.time >= nextHarvestTime);
        }
    }

    // 
    private void Harvest()
    {
        Debug.Log("Thu hoạch thành công!");
        nextHarvestTime = Time.time + cooldownTime;

        if (playerInventory != null && herbalFruitData != null)
        {
            // Gọi hàm AddItem mới
            playerInventory.AddItem(herbalFruitData);
        }

        interactionPrompt.SetActive(false);
    }
    // Hàm này được tự động gọi khi có đối tượng đi vào vùng trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng đó có tag "Player" không
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Lấy component túi đồ từ người chơi
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }

    // Hàm này được tự động gọi khi đối tượng đi ra khỏi vùng trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Ẩn prompt khi người chơi đi xa
            interactionPrompt.SetActive(false);
            playerInventory = null; // Xóa tham chiếu khi người chơi đi xa
        }
    }
}