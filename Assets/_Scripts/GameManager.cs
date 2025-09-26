using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Biến tĩnh để lưu tên điểm spawn cho màn chơi tiếp theo
    public static string nextSpawnPointName;

    private void Awake()
    {
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