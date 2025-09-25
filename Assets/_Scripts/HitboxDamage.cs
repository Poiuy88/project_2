using UnityEngine;

public class HitboxDamage : MonoBehaviour
{
    // Script này cần biết sát thương là bao nhiêu
    // Chúng ta sẽ lấy nó từ script PlayerAttack
    private int damage;

    void Start()
    {
        // Tìm và lấy giá trị sát thương từ script PlayerAttack trên đối tượng cha
        damage = GetComponentInParent<PlayerAttack>().attackDamage;
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