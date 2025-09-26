using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public static string nextSpawnPointName;
    public static string previousSceneName;
    public static Vector2 playerPositionOnExit;
    public static bool returningToPreviousScene = false;

    private void Awake()
    {
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