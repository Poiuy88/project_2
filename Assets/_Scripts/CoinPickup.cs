using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;

    // Đảm bảo hàm này là OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra Tag của đối tượng va chạm
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.AddCoins(coinValue);
                Debug.Log($"Picked up {coinValue} coins. Total: {playerStats.coins}"); // Thêm Log để xác nhận
            }
            else
            {
                 Debug.LogError("Could not find PlayerStats on the player object!");
            }
            Destroy(gameObject);
        }
    }
}