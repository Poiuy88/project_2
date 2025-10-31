using UnityEngine;
using TMPro; // Rất quan trọng, thêm thư viện này để dùng TextMeshPro

public class PlayerNameDisplay : MonoBehaviour
{
    // Kéo thả TextMeshPro vào đây
    public TextMeshProUGUI nameText; 
    
    // Kéo thả Canvas của Bảng Tên vào đây
    public Transform nameCanvasTransform; 

    private Vector3 initialCanvasScale; // Lưu scale gốc

    void Start()
    {
        // 1. Lưu lại scale ban đầu của Canvas
        if (nameCanvasTransform != null)
        {
            initialCanvasScale = nameCanvasTransform.localScale;
        }

        // 2. Lấy tên mà người chơi đã nhập ở LoginScene
        string playerName = PlayerPrefs.GetString("PlayerName");

        // 3. Đặt tên mặc định nếu không tìm thấy
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player"; // Tên mặc định
        }

        // 4. Gán tên đó cho Text
        if (nameText != null)
        {
            nameText.text = playerName;
        }
        else
        {
            Debug.LogError("Chưa gán 'Name Text' cho PlayerNameDisplay!");
        }
    }

    // Dùng LateUpdate để chạy sau khi PlayerController.Flip() chạy
    void LateUpdate()
    {
        // 5. Xử lý "Lật Tên" (Anti-Flip)
        if (nameCanvasTransform == null) return;

        // Kiểm tra xem Player (cha) có bị lật hay không
        if (transform.localScale.x < 0)
        {
            // Nếu Player lật (nhìn sang trái), lật ngược Canvas lại
            nameCanvasTransform.localScale = new Vector3(-initialCanvasScale.x, initialCanvasScale.y, initialCanvasScale.z);
        }
        else
        {
            // Nếu Player bình thường (nhìn sang phải), dùng scale gốc
            nameCanvasTransform.localScale = initialCanvasScale;
        }
    }
}