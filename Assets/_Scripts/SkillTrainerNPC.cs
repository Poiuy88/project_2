using UnityEngine;
using TMPro;

public class SkillTrainerNPC : MonoBehaviour
{
    public GameObject skillLearnPanel;
    public GameObject interactionPrompt;
    public TextMeshProUGUI feedbackText; // Một Text để hiển thị thông báo

    private bool playerInRange = false;
    private PlayerStats playerStats;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            skillLearnPanel.SetActive(true);
            UpdateFeedbackText(""); // Xóa thông báo cũ
        }
    }

    public void LearnFireballSkill()
    {
        int requiredLevel = 5;
        int cost = 50;

        if (GameManager.hasLearnedFireball)
        {
            UpdateFeedbackText("Cháu đã học chiêu này rồi!");
            return;
        }

        if (playerStats.playerLevel >= requiredLevel)
        {
            if (playerStats.SpendCoins(cost))
            {
                GameManager.hasLearnedFireball = true;
                UpdateFeedbackText("Chúc mừng! Cháu đã học được Quả Cầu Lửa!");
            }
            else
            {
                UpdateFeedbackText("Không đủ xu!");
            }
        }
        else
        {
            UpdateFeedbackText("Cần đạt Level " + requiredLevel + "!");
        }
    }

    public void ClosePanel() { skillLearnPanel.SetActive(false); }
    void UpdateFeedbackText(string message) { if (feedbackText != null) feedbackText.text = message; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionPrompt.SetActive(true);
            playerStats = other.GetComponent<PlayerStats>();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionPrompt.SetActive(false);
            skillLearnPanel.SetActive(false);
        }
    }
}