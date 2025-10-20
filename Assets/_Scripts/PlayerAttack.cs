using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 15;
    public GameObject attackHitbox;

    private Animator anim;

    // Dùng Awake để khởi tạo, nó được gọi trước Start
    void Awake()
    {
        // LOG 1: Báo danh xem script này đang chạy trên đối tượng nào
        Debug.Log("<color=orange>PlayerAttack SCRIPT is AWAKE on object: " + gameObject.name + "</color>");

        anim = GetComponent<Animator>();

        // LOG 2: Báo cáo kết quả của việc tìm kiếm Animator
        if (anim != null)
        {
            Debug.Log("<color=green>SUCCESS: Animator component was FOUND on object: " + gameObject.name + "!</color>");
        }
        else
        {
            Debug.LogError("<color=red>FATAL ERROR: Animator component was NOT FOUND on object: " + gameObject.name + "!</color>");
        }
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // LOG 3: Báo rằng lệnh tấn công được ghi nhận
            Debug.Log("Attack input received on: " + gameObject.name);
            Attack();
        }
    }

    void Attack()
    {
        // LOG 4: Kiểm tra biến anim ngay trước khi sử dụng
        if (anim == null)
        {
            Debug.LogError("<color=red>CRITICAL: Trying to attack, but the 'anim' variable is NULL on object: " + gameObject.name + "</color>");
            return; // Dừng lại để không gây ra lỗi NullReferenceException
        }
        anim.SetTrigger("attack");
    }

    // Các hàm Enable/Disable Hitbox giữ nguyên
    void EnableHitbox() { if (attackHitbox != null) attackHitbox.SetActive(true); }
    void DisableHitbox() { if (attackHitbox != null) attackHitbox.SetActive(false); }
}