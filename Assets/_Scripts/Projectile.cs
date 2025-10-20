using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 100f;
    public int damage = 30;
    public float homingRadius = 150f; // Bán kính tìm kiếm kẻ địch
    public float rotationSpeed = 20f; // Tốc độ xoay để đuổi theo mục tiêu

    private Transform target; // Mục tiêu (kẻ địch)
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FindNearestEnemy();

        // Nếu không tìm thấy kẻ địch, bay thẳng về phía trước
        if (target == null)
        {
            rb.linearVelocity = transform.right * speed;
        }

        // Tự hủy sau 5 giây để tránh bay vô hạn
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // Nếu có mục tiêu, liên tục đuổi theo
        if (target != null)
        {
            // Xoay hướng viên đạn về phía mục tiêu
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.right).z;
            rb.angularVelocity = -rotateAmount * rotationSpeed;

            // Di chuyển về phía trước theo hướng đã xoay
            rb.linearVelocity = transform.right * speed;
        }
    }

    void FindNearestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        // Tìm tất cả các đối tượng có tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            // Nếu kẻ địch này ở gần hơn và trong bán kính tìm kiếm
            if (distance < closestDistance && distance <= homingRadius)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.transform; // Gán mục tiêu
        }
    }

    // Hàm OnTriggerEnter2D giữ nguyên
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null) { enemyHealth.TakeDamage(damage); }
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}