using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 6f;
    Rigidbody2D playerBody;
    float hInput;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        hInput = Input.GetAxis("Horizontal");

        playerBody.velocity = new Vector2(hInput * moveSpeed, playerBody.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce);
        }
         
    }
}
