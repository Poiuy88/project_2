using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Thêm thư viện SceneManagement

public class DialogueManager : MonoBehaviour
{
    // Các biến UI, tạm thời không cần public nữa vì chúng ta sẽ tự tìm
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI dialogueText;
    private GameObject dialoguePanel;

    private Queue<string> sentences;
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

    // Thêm các hàm để lắng nghe sự kiện load scene
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Hàm được gọi mỗi khi scene mới load xong
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIElements();
    }

    void FindUIElements()
    {
        // Chỉ tìm kiếm nếu chúng ta không ở trong scene Login
        if (SceneManager.GetActiveScene().name != "LoginScene")
        {
            Debug.Log("DialogueManager is searching for UI elements.");
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                // Tìm các đối tượng con bằng tên
                // Đảm bảo tên trong Hierarchy của bạn khớp với các tên này
                dialoguePanel = canvas.transform.Find("DialoguePanel").gameObject;
                nameText = dialoguePanel.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
                dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();

                // Ẩn panel đi khi bắt đầu
                if (dialoguePanel != null)
                {
                    dialoguePanel.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("DialogueManager could not find a Canvas in this scene.");
            }
        }
    }

    // Hàm Update để xử lý nhấn chuột qua thoại
    void Update()
    {
        // Thêm kiểm tra 'dialoguePanel != null' để đảm bảo an toàn
        if (dialoguePanel != null && dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }

    // Các hàm StartDialogue, DisplayNextSentence, EndDialogue giữ nguyên
    public void StartDialogue(Dialogue dialogue)
    {
        if (dialoguePanel == null) FindUIElements(); // Thử tìm lại UI nếu bị thiếu

        dialoguePanel.SetActive(true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
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
    }
}



// public class DialogueManager : MonoBehaviour
// {
//     public TextMeshProUGUI nameText;
//     public TextMeshProUGUI dialogueText;
//     public GameObject dialoguePanel;

//     private Queue<string> sentences; // Hàng đợi để lưu trữ các câu thoại

//     // Singleton pattern: Đảm bảo chỉ có một DialogueManager trong game
//     public static DialogueManager instance;

//     private void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }

//         sentences = new Queue<string>();
//     }

//     public void StartDialogue(Dialogue dialogue)
//     {
//         dialoguePanel.SetActive(true);
//         nameText.text = dialogue.name;
//         sentences.Clear();

//         foreach (string sentence in dialogue.sentences)
//         {
//             sentences.Enqueue(sentence);
//         }

//         DisplayNextSentence();
//     }
//     void Update()
//     {
//         // Nếu panel hội thoại đang bật và người chơi nhấn chuột trái
//         if (dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
//         {
//             DisplayNextSentence();
//         }
//     }
//     public void DisplayNextSentence()
//     {
//         if (sentences.Count == 0)
//         {
//             EndDialogue();
//             return;
//         }

//         string sentence = sentences.Dequeue();
//         dialogueText.text = sentence;
//     }

//     void EndDialogue()
//     {
//         dialoguePanel.SetActive(false);
//         Debug.Log("Kết thúc hội thoại.");
//     }
// }
