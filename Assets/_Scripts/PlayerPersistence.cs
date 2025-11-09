// using UnityEngine;

// public class PlayerPersistence : MonoBehaviour
// {
//     // Sử dụng Singleton pattern đơn giản để đảm bảo chỉ có một người chơi duy nhất
//     public static PlayerPersistence instance;

//     private void Awake()
//     {
//         // Nếu chưa có "instance" nào tồn tại
//         if (instance == null)
//         {
//             // Thì gán "instance" là chính đối tượng này
//             instance = this;
//             // Và ra lệnh cho Unity không hủy đối tượng này khi load scene mới
//             DontDestroyOnLoad(gameObject);
//         }
//         // Ngược lại, nếu "instance" đã tồn tại (tức là đã có 1 Player từ scene trước đi qua)
//         else
//         {
//             // Thì hủy đối tượng này đi để tránh bị trùng lặp
//             Destroy(gameObject);
//         }
//     }
// }
using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    public static PlayerPersistence instance;

    // Tạo một tham chiếu công khai để các script khác truy cập
    [HideInInspector] public PlayerStats playerStats;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Lấy component PlayerStats của chính Player "xịn" này
            playerStats = GetComponent<PlayerStats>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}