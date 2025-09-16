//PLAYER.CS

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    private float speed = 3f;
    private float jumpForce = 20f;
    private float moveHorizontal;
    private bool isJumping;

    void Update()
    {
        //get move input
        moveHorizontal = Input.GetAxisRaw("Horizontal");

        //jump
        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        //move
        if(moveHorizontal !=0)
        {
            rb.AddForce(new Vector2(moveHorizontal * speed, 0), ForceMode2D.Impulse);
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
