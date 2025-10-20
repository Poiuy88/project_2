using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    // Các biến cho hệ thống dịch chuyển
    public static string nextSpawnPointName;
    public static string previousSceneName;
    public static Vector2 playerPositionOnExit;
    public static bool returningToPreviousScene = false;

    // Biến cho hệ thống kỹ năng
    public static bool hasLearnedFireball = false;

    private void Awake()
    {
        // Dòng log này để xác nhận GameManager đang hoạt động
        Debug.Log("<color=purple>GAME MANAGER AWAKE!</color>");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}