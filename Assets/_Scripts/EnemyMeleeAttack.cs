using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public int attackDamage = 20; // Sát thương của Heo Rừng
    public float attackRange = 1.5f; // Tầm tấn công
    public float attackCooldown = 2f; // Thời gian chờ giữa các đòn đánh
    public string attackTriggerName = "attack"; // Tên Trigger trong Animator (nếu có)

    private Transform playerTarget;
    private float lastAttackTime;
    private Animator anim; // Component Animator (nếu có)
    private AggroAI aiScript; // Để biết khi nào đang đuổi theo

    void Start()
    {
        anim = GetComponent<Animator>();
        aiScript = GetComponent<AggroAI>(); // Lấy script AI
    }

    void Update()
    {
        // Chỉ tấn công nếu đang ở trạng thái đuổi theo và đã hết cooldown
        if (aiScript != null && aiScript.GetCurrentState() == AggroAI.AIState.Chasing && Time.time > lastAttackTime + attackCooldown)
        {
            // Tìm người chơi (có thể lấy từ AggroAI)
            playerTarget = aiScript.GetPlayerTarget();

            if (playerTarget != null)
            {
                // Kiểm tra khoảng cách
                float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);
                if (distanceToPlayer <= attackRange)
                {
                    // Nếu đủ gần, thực hiện tấn công
                    Attack();
                }
            }
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time; // Đặt lại thời gian hồi chiêu

        // Kích hoạt animation tấn công (nếu có)
        if (anim != null)
        {
            anim.SetTrigger(attackTriggerName);
        }

        // Gây sát thương (Delay nhỏ để khớp animation nếu cần)
        // Invoke("DealDamage", 0.2f); // Hoặc gọi trực tiếp nếu không cần delay
        DealDamage();
    }

    void DealDamage()
    {
        // Kiểm tra lại lần nữa phòng khi người chơi chạy mất
        if (playerTarget != null && Vector2.Distance(transform.position, playerTarget.position) <= attackRange)
        {
            PlayerStats playerStats = playerTarget.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(attackDamage);
                Debug.Log(gameObject.name + " attacked Player for " + attackDamage + " damage.");
            }
        }
    }
}