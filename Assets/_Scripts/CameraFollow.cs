using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Biến để kéo thả đối tượng Player vào
    public Transform target;

    // Biến để điều chỉnh độ mượt khi camera di chuyển
    public float smoothing = 5f;

    // Biến để giữ khoảng cách ban đầu giữa camera và nhân vật
    private Vector3 offset;

    void Start()
    {
        // === PHẦN CODE MỚI ĐỂ SỬA LỖI ===

        // 1. Kiểm tra xem camera có đang bị "lạc lõng" (chưa có target) không
        if (target == null)
        {
            Debug.Log("Camera target not assigned. Searching for Player...");
            // 2. Nếu chưa có, hãy tự động tìm đối tượng có tag "Player" trong màn chơi
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // 3. Nếu tìm thấy
            if (player != null)
            {
                // Gán target của camera là transform của đối tượng Player vừa tìm được
                target = player.transform;
                Debug.Log("Player found! Camera is now following the player.");
            }
            else
            {
                // Nếu không tìm thấy, báo lỗi để chúng ta biết
                Debug.LogError("Could not find GameObject with tag 'Player' for the camera to follow.");
                return; // Dừng lại để tránh gây thêm lỗi
            }
        }

        // === KẾT THÚC PHẦN CODE MỚI ===

        // Tính toán khoảng cách ban đầu (đoạn code này vẫn giữ nguyên)
        offset = transform.position - target.position;
    }

    // LateUpdate được gọi sau khi tất cả các hàm Update đã chạy xong
    void LateUpdate()
    {
        // Thêm một kiểm tra để đảm bảo target không bị mất giữa chừng
        if (target == null) return;
        
        // Vị trí mới mà camera muốn đến
        Vector3 targetCamPos = target.position + offset;

        // Di chuyển camera một cách mượt mà từ vị trí hiện tại đến vị trí mới
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}