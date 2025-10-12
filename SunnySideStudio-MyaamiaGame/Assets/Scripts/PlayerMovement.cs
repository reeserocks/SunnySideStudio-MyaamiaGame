//PLAYERMOVEMENT.CS

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    private float speed = 3f;
    private float jumpForce = 20f;
    private float holdForce = 0.8f;
    private float maxHoldTime = 0.35f;

    private float moveHorizontal;
    private float moveVertical;
    private float holdTimeCounter;

    private bool isGrounded;
    private bool isJumping;
    private bool jumpHeld;

    void Update()
    {
        //get move input
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        //jump
        if (moveVertical > 0 && isGrounded)
        {
            isJumping = true;
            jumpHeld = true;
            holdTimeCounter = maxHoldTime;
        }
        //keep jumping
        if (moveVertical <=0)
        {
            jumpHeld = false;
        }
    }

    void FixedUpdate()
    {
        //move
        if(moveHorizontal !=0)
        {
            rb.AddForce(new Vector2(moveHorizontal * speed, 0), ForceMode2D.Impulse);
        }

        //jump
        if(isJumping)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            isJumping = false;
        }
        //keep jumping 
        if (jumpHeld && holdTimeCounter > 0)
        {
            rb.AddForce(Vector2.up * holdForce, ForceMode2D.Impulse);
            holdTimeCounter -= Time.fixedDeltaTime;
        }
    }

    //check if is grounded
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpHeld = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
