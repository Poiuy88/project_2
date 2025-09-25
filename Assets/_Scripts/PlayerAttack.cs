using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 15;
    public GameObject attackHitbox; // Tham chiếu tới hitbox

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Kích hoạt animation tấn công
        anim.SetTrigger("attack");
    }

    // === CÁC HÀM NÀY SẼ ĐƯỢC GỌI BỞI ANIMATION EVENT ===
    void EnableHitbox()
    {
        attackHitbox.SetActive(true);
    }

    void DisableHitbox()
    {
        attackHitbox.SetActive(false);
    }
}