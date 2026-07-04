using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float doublejumpSpeed;
    [SerializeField] float climbSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] Vector2 kick = new Vector2(0f, 10f);

    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;

    Rigidbody2D playerBody;
    CapsuleCollider2D playerCollider;
    BoxCollider2D groundCollider;
    SpriteRenderer spriteRenderer;
    Animator playerAnim;

    float hInput;
    float gravityScaleAtStart;

    bool isGrounded;
    bool doublejump;
    bool isAlive = true;

    

    

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>(); 
        groundCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();
        gravityScaleAtStart = playerBody.gravityScale;
    }

    
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, 0.1f, groundLayer);
        
        if(!isAlive)
        {
            return;
        }

        Movement();
        Jump();
        //Jumpping();
        Flip();
        Climb();
        Attack();
        Die();
    }

    void Movement()
    {
        if(!isAlive) { return; }

        hInput = Input.GetAxis("Horizontal");
        playerBody.velocity = new Vector2(hInput * moveSpeed , playerBody.velocity.y);

        bool hasfaceing = Mathf.Abs(playerBody.velocity.x) > Mathf.Epsilon; // for Flip
        playerAnim.SetBool("isRunning", hasfaceing);

    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded) 
            {
                playerBody.velocity = new Vector2(playerBody.velocity.x, jumpSpeed );
                isGrounded= false;
                
            }
            else
            {
                if(doublejump)
                {
                    playerBody.velocity = new Vector2(playerBody.velocity.x, doublejumpSpeed );
                    doublejump = false;
                }
            }   
        }
        if(isGrounded)
        {
            doublejump = true;
        }
    }
    
    void Jumpping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (groundCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                playerBody.velocity = new Vector2(playerBody.velocity.x, jumpSpeed);
            }
        }
            
    }

    void Flip()
    {
        //if(playerBody.velocity.x < 0)
        //   spriteRenderer.flipX =true;

        //else if(playerBody.velocity.x > 0)
        //        spriteRenderer.flipX=false;

        bool hasfaceing = Mathf.Abs(playerBody.velocity.x) > Mathf.Epsilon; // for Flip
        if (hasfaceing)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerBody.velocity.x), 1f);
        }
        
        
    }

    void Climb()
    {
        float vInput = Input.GetAxis("Vertical");
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            Vector2 climbVelocity = new Vector2(playerBody.velocity.x, vInput * climbSpeed);
            playerBody.velocity = climbVelocity;
            playerBody.gravityScale = 0;

            bool hasClimb = Mathf.Abs(playerBody.velocity.y) > Mathf.Epsilon;
            playerAnim.SetBool("isClimbing", hasClimb);
        }
        else
        {
            playerBody.gravityScale = gravityScaleAtStart;
            playerAnim.SetBool("isClimbing", false);
        }
        
    }

    void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!isAlive) { return; }
            Instantiate(bullet, gun.position, transform.rotation);
        }
    }
    void Die()
    {
        if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            playerAnim.SetTrigger("Dying");
            playerBody.velocity = kick;
            FindAnyObjectByType<GameSession>().PlayerDeath();
        }
    }
    public void backGame()
    {
        SceneManager.LoadScene(0);
    }
}
