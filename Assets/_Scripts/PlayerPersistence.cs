using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    public static PlayerPersistence instance;

    [HideInInspector] public PlayerStats playerStats;

    // Thêm tham chiếu đến túi đồ
    [HideInInspector] public PlayerInventory playerInventory;
    // --- KẾT THÚC CODE MỚI ---

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            playerStats = GetComponent<PlayerStats>();

            // Lấy component túi đồ
            playerInventory = GetComponent<PlayerInventory>();
            // --- KẾT THÚC CODE MỚI ---
        }
        else
        {
            Destroy(gameObject);
        }
    }
}