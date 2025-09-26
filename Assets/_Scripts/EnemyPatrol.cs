using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f; // Tốc độ di chuyển
    public float patrolDistance = 3f; // Khoảng cách đi tuần tra mỗi bên

    private Vector3 startPosition;
    private bool movingRight = true;
    private Vector3 leftPatrolPoint, rightPatrolPoint;
    private Transform spriteTransform;

    void Start()
    {
        startPosition = transform.position;
        leftPatrolPoint = startPosition - new Vector3(patrolDistance, 0, 0);
        rightPatrolPoint = startPosition + new Vector3(patrolDistance, 0, 0);
        spriteTransform = transform; // Giả sử sprite và script cùng 1 object
    }

    void Update()
    {
        if (movingRight)
        {
            // Di chuyển sang phải
            transform.position = Vector3.MoveTowards(transform.position, rightPatrolPoint, speed * Time.deltaTime);
            if (transform.position == rightPatrolPoint)
            {
                // Khi đến điểm đích, đổi hướng
                movingRight = false;
                Flip();
            }
        }
        else
        {
            // Di chuyển sang trái
            transform.position = Vector3.MoveTowards(transform.position, leftPatrolPoint, speed * Time.deltaTime);
            if (transform.position == leftPatrolPoint)
            {
                // Khi đến điểm đích, đổi hướng
                movingRight = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        // Lật hình ảnh của quái vật
        Vector3 scaler = spriteTransform.localScale;
        scaler.x *= -1;
        spriteTransform.localScale = scaler;
    }
}