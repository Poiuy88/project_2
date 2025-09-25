using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    private Queue<string> sentences; // Hàng đợi để lưu trữ các câu thoại

    // Singleton pattern: Đảm bảo chỉ có một DialogueManager trong game
    public static DialogueManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    void Update()
    {
        // Nếu panel hội thoại đang bật và người chơi nhấn chuột trái
        if (dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Debug.Log("Kết thúc hội thoại.");
    }
}