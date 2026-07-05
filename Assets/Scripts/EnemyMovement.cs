using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D EnemyBody;

    //[SerializeField] AudioClip enemyMoveClip;
    //AudioSource audioSource;

    void Start()
    {
        EnemyBody = GetComponent<Rigidbody2D>();
        //audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.clip = enemyMoveClip;
        //audioSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBody.velocity = new Vector2(moveSpeed, 0f);
        bool isMoving = Mathf.Abs(EnemyBody.velocity.x) > Mathf.Epsilon;
        //if (isMoving)
        //{
        //    if (!audioSource.isPlaying)
        //    {
        //        audioSource.Play();
        //    }
        //}
        //else
        //{
        //    audioSource.Stop();
        //}
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
