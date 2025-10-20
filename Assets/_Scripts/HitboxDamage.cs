using UnityEngine;

public class HitboxDamage : MonoBehaviour
{

    private int damage;

    void Start()
{
    // Lấy sát thương tổng từ PlayerStats thay vì PlayerAttack
    damage = GetComponentInParent<PlayerStats>().GetTotalAttack();
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu va chạm với đối tượng có tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}