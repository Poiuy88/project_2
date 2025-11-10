using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Tạo một instance (thể hiện) tĩnh để đảm bảo chỉ có 1 MusicManager duy nhất
    public static MusicManager instance;

    private void Awake()
    {
        // Nếu chưa có instance nào
        if (instance == null)
        {
            // Gán instance là đối tượng này
            instance = this;
            // Ra lệnh cho Unity không hủy đối tượng này khi tải scene mới
            DontDestroyOnLoad(gameObject);
        }
        // Nếu đã có instance (ví dụ: bạn quay lại LoginScene)
        else
        {
            // Hủy đối tượng này đi để tránh bị trùng lặp (và chạy 2 bài nhạc)
            Destroy(gameObject);
        }
    }
}