// using UnityEngine;
// using UnityEngine.EventSystems;
// public class PlayerAttack : MonoBehaviour
// {
//     public int attackDamage = 15;
//     public GameObject attackHitbox; // Tham chiếu tới hitbox

//     private Animator anim;

//     void Update()
//     {
//         // KIỂM TRA MỚI: Nếu chuột đang trỏ vào một đối tượng UI (nút bấm, panel...)
//         if (EventSystem.current.IsPointerOverGameObject())
//         {
//             // Thì không làm gì cả và thoát khỏi hàm Update ngay lập tức
//             return; 
//         }

//         // Nếu không trỏ vào UI, thì mới kiểm tra lệnh tấn công như bình thường
//         if (Input.GetMouseButtonDown(0))
//         {
//             Attack();
//         }
//     }


//     void Attack()
//     {
//         // Kích hoạt animation tấn công
//         anim.SetTrigger("attack");
//     }

//     // === CÁC HÀM NÀY SẼ ĐƯỢC GỌI BỞI ANIMATION EVENT ===
//     void EnableHitbox()
//     {
//         attackHitbox.SetActive(true);
//     }

//     void DisableHitbox()
//     {
//         attackHitbox.SetActive(false);
//     }
// }
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