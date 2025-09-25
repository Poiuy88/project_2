using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện để quản lý Scene
using TMPro; // Thư viện để làm việc với TextMeshPro

public class LoginManager : MonoBehaviour
{
    // Tạo một biến công khai để kéo thả InputField vào từ Unity Editor
    public TMP_InputField nameInputField;

    // Hàm này sẽ được gọi khi nhấn nút "Đăng nhập"
    public void LoginGame()
    {
        // Lấy tên người chơi từ ô nhập liệu
        string playerName = nameInputField.text;

        // Kiểm tra xem người chơi đã nhập tên chưa
        if (string.IsNullOrEmpty(playerName))
        {
            // Nếu chưa nhập, in ra một thông báo lỗi và không làm gì cả
            Debug.Log("Tên nhân vật không được để trống!");
            return;
        }

        // Lưu tên người chơi để có thể sử dụng ở các scene khác
        // PlayerPrefs là một cách đơn giản để lưu dữ liệu nhỏ
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        // Chuyển sang scene VillageMap
        // Tên "VillageMap" phải chính xác như tên file scene của bạn
        SceneManager.LoadScene("VillageMap");
    }

    // Hàm này sẽ được gọi khi nhấn nút "Thoát"
    public void ExitGame()
    {
        // In ra console để kiểm tra trong Editor
        Debug.Log("Người chơi đã nhấn Thoát Game!");
        // Lệnh này chỉ hoạt động khi game đã được build thành file .exe
        Application.Quit();
    }
}