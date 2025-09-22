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
    private float moveHorizontal;
    private float moveVertical;
    private bool isJumping;
    private float jumpTimeCounter;
    private float jumpTime = 0.35f;

    void Update()
    {
        //get move input
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        //move
        if(moveHorizontal !=0)
        {
            rb.AddForce(new Vector2(moveHorizontal * speed, 0), ForceMode2D.Impulse);
        }
        //jump
        if(!isJumping && moveVertical > 0)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        //keep jumping 
        if (Input.GetKey(KeyCode.UpArrow) && isJumping)
        {
            if (jumpTimeCounter > 0) {
                rb.AddForce(new Vector2(0, 0.01f), ForceMode2D.Impulse);
                jumpTimeCounter -= Time.deltaTime;
            } else
            {
                isJumping = false;
            }
        }
        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
        }
    }

    //check if can jump
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
        }
    }
}
