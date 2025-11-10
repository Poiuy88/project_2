using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    // --- BẮT ĐẦU SỬA ĐỔI ---
    // Thay vì tính toán offset, chúng ta sẽ TỰ ĐỊNH NGHĨA nó
    // Bạn có thể chỉnh giá trị này trong Inspector
    public Vector3 offset = new Vector3(0f, 1f, -10f);
    
    // Bỏ biến 'offsetCalculated'
    // --- KẾT THÚC SỬA ĐỔI ---


    void LateUpdate()
    {
        // 1. Nếu chưa có mục tiêu (target)
        if (target == null)
        {
            // Chỉ tìm Player nếu chúng ta KHÔNG ở LoginScene
            if (SceneManager.GetActiveScene().name != "LoginScene")
            {
                FindPlayer();
            }
            
            if (target == null)
            {
                return; 
            }
        }

        // 2. Di chuyển camera (LOGIC ĐƯỢC ĐƠN GIẢN HÓA)
        // Chúng ta không cần tính toán offset nữa, chỉ cần áp dụng nó
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    // Hàm riêng để tìm Player (giữ nguyên)
    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            target = player.transform;
            Debug.Log("CameraFollow: Đã tìm thấy Player!");
        }
    }
}