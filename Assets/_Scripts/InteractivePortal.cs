using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractivePortal : MonoBehaviour
{
    [Header("Destination Settings")]
    public string sceneToLoad;
    public string spawnPointName;

    [Header("Portal Type")]
    public bool isExitPortal = false;

    public GameObject interactionPrompt;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isExitPortal)
            {
                Debug.Log("<color=red>EXIT PORTAL ACTIVATED. Preparing to return to: " + GameManager.previousSceneName + "</color>");
                GameManager.returningToPreviousScene = true;
                SceneManager.LoadScene(GameManager.previousSceneName);
            }
            else
            {
                GameManager.previousSceneName = SceneManager.GetActiveScene().name;
                GameManager.playerPositionOnExit = transform.position;
                GameManager.nextSpawnPointName = spawnPointName;
                Debug.Log("<color=cyan>ENTRANCE PORTAL ACTIVATED. Storing return info: Scene=" + GameManager.previousSceneName + ", Position=" + GameManager.playerPositionOnExit + ". Going to: " + sceneToLoad + "</color>");
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
    
    // Các hàm Start, OnTriggerEnter2D, OnTriggerExit2D giữ nguyên
    void Start() { if (interactionPrompt != null) interactionPrompt.SetActive(false); }
    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) { playerInRange = true; if (interactionPrompt != null) interactionPrompt.SetActive(true); } }
    private void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) { playerInRange = false; if (interactionPrompt != null) interactionPrompt.SetActive(false); } }
}