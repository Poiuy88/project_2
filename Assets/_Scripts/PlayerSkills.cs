using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [Header("Fireball Skill")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public int manaCost = 15;
    public float fireballCooldown = 5f; // Thời gian hồi chiêu
    [Header("Sound Effect")]
    public AudioClip castSound; // Âm thanh khi tung chiêu
    
    private AudioSource audioSource;

    // Biến private để theo dõi thời gian hồi chiêu
    private float currentCooldown = 0f;

    private PlayerStats playerStats;
    private PlayerController playerController;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Liên tục giảm thời gian hồi chiêu
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    // Hàm này bây giờ là public để nút có thể gọi
    public void CastFireball()
    {
        // Kiểm tra tất cả các điều kiện: đã học, hết hồi chiêu, đủ năng lượng
        if (GameManager.hasLearnedFireball && currentCooldown <= 0 && playerStats.currentMana >= manaCost)
        {
            playerStats.UseMana(manaCost);

            float angle = playerController.IsFacingRight() ? 0f : 180f;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Instantiate(fireballPrefab, firePoint.position, rotation);
            if (audioSource != null && castSound != null)
            {
                audioSource.PlayOneShot(castSound);
            }

            // Đặt lại thời gian hồi chiêu
            currentCooldown = fireballCooldown;
        }
    }

    // Hàm public để UI có thể lấy thông tin
    public float GetCurrentCooldown() { return currentCooldown; }
}