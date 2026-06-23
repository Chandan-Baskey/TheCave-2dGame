using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D EnemyBody;
    void Start()
    {
        EnemyBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBody.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        Flip();
    }

    void Flip()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(EnemyBody.velocity.x)), 1f);
    }
    
}
