using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject interactionPrompt; // Dòng chữ "Nhấn E để nói chuyện"

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Bắt đầu hội thoại khi người chơi nhấn E
            DialogueManager.instance.StartDialogue(dialogue);
            // Ẩn prompt đi khi đang nói chuyện
            interactionPrompt.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionPrompt.SetActive(false);
        }
    }
}