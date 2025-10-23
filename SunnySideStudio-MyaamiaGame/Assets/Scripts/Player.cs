//PLAYERMOVEMENT.CS

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;

    private float jumpForce = 20f;
    private float holdForce = 0.8f;
    private float maxHoldTime = 0.35f;

    private float moveHorizontal;
    private float moveVertical;
    private float holdTimeCounter;

    private bool isGrounded;
    private bool isJumping;
    private bool jumpHeld;

    private void Awake()
    {
        GameManager.Instance.Player = this;
    }


    void Update()
    {
        // get move input
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        // jump
        if (moveVertical > 0 && isGrounded)
        {
            isJumping = true;
            jumpHeld = true;
            holdTimeCounter = maxHoldTime;
        }
        // keep jumping
        if (moveVertical <= 0)
        {
            jumpHeld = false;
        }
        if (transform.position.y < -5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void FixedUpdate()
    {
        // don't override win anim
        if (animator.GetBool("isWin"))
        {
            moveHorizontal = 0;
            return;
        }

        // move
        if (moveHorizontal != 0)
        {
            float speed = isGrounded ? 8f : 6f;
            rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // movement animations
        if (moveHorizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveHorizontal > 0)
        {
            spriteRenderer.flipX = false;
        }

        // jump
        if (isJumping)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            rb.linearVelocityX = Math.Min(rb.linearVelocityX, 1);
            isGrounded = false;
            isJumping = false;
        }
        // keep jumping 
        if (jumpHeld && holdTimeCounter > 0)
        {
            rb.AddForce(Vector2.up * holdForce, ForceMode2D.Impulse);
            rb.linearVelocityX = Math.Min(rb.linearVelocityX, 1);
            holdTimeCounter -= Time.fixedDeltaTime;
        }

        // jump animations
        if (rb.linearVelocity.y == 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        if (rb.linearVelocity.y > 0)
        {
            animator.SetBool("isJumping", true);
        }
        if (rb.linearVelocity.y < 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
    }

    // check if is grounded
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

    //hang 
    public void SetHanging(bool hanging)
    {
        animator.SetBool("isHanging", hanging);
    }

    //win
    public void SetWin(bool win)
    {
        animator.SetBool("isWin", win);
    }

    // SAVE AND LOAD
    public void Save(ref PlayerSaveData data)
    {
        data.Position = transform.position;
        data.levelUnlocked = GameManager.playerSaveData.levelUnlocked;
    }

    public void Load(PlayerSaveData data)
    {
        transform.position = data.Position;
        GameManager.playerSaveData.levelUnlocked = data.levelUnlocked;
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 Position;
    public int levelUnlocked;
}
