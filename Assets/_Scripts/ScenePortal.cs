using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện để quản lý Scene

public class ScenePortal : MonoBehaviour
{
    // Tên của scene mà cổng này sẽ dẫn đến
    public string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu đối tượng va chạm có tag là "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the portal. Loading scene: " + sceneToLoad);
            // Load scene mới
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}