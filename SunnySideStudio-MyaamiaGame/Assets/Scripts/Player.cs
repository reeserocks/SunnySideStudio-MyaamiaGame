//PLAYER.CS

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

    private float jumpForce = 23f;
    private float holdForce = 0.8f;
    private float maxHoldTime = 0.35f;

    private float moveHorizontal;
    private float moveVertical;
    private float holdTimeCounter;

    public bool isGrounded;
    public bool isJumping;
    private bool jumpHeld;
    public bool canMove;

    public bool pushedByFan;
    private int fanDirection;


    private void Start()
    {
        GameManager.Instance.Player = this;
        canMove = true;
    }


    void Update()
    {
        if (canMove)
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
                rb.sharedMaterial.friction = 0.0f;
            }

            // keep jumping
            if (moveVertical < 0)
            {
                jumpHeld = false;
            }

            if (moveVertical == 0 && moveHorizontal == 0)
            {
                rb.linearVelocityX = 0.0f;
            }
        }
        
        if (transform.position.y < -5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void FixedUpdate()
    {
        if (animator.GetBool("hasBook"))
        {
            return;
        }
        
        // don't override win anim
        if (animator.GetBool("isWin"))
        {
            moveHorizontal = 0;
            return;
        }

        // move
        if (moveHorizontal != 0 && canMove)
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
            spriteRenderer.transform.localScale = new Vector3(-.18f, .18f, .18f);
        }
        else if (moveHorizontal > 0)
        {
            spriteRenderer.transform.localScale = new Vector3(.18f, .18f, .18f);
        }

        // jump
        if (isJumping && canMove)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            if (pushedByFan)
            {
                rb.linearVelocityX = 25 * fanDirection;
            }
            else
            {
                rb.linearVelocityX = Math.Min(rb.linearVelocityX, 4.8f);
            }
            isGrounded = false;
            isJumping = false;
        }
        // keep jumping 
        if (jumpHeld && holdTimeCounter > 0 && canMove)
        {
            rb.AddForce(Vector2.up * holdForce, ForceMode2D.Impulse);
            if (pushedByFan)
            {
                rb.linearVelocityX = 25 * fanDirection;
            }
            else
            {
                rb.linearVelocityX = Math.Min(rb.linearVelocityX, 3f);
            }
            holdTimeCounter -= Time.fixedDeltaTime;
        }
        // no y velocity when grounded
        if (isGrounded)
        {
            rb.linearVelocityY = 0;
            rb.sharedMaterial.friction = 0.4f;
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
        if (collision.CompareTag("Ground") && canMove)
        {
            isGrounded = true;
            jumpHeld = false;
            
        }
        else if (collision.gameObject.CompareTag("Fan") && canMove)
        {
            if (collision.gameObject.transform.rotation.z == -90.0f)
            {
                fanDirection = 0;
            }
            else if (collision.gameObject.transform.localScale.x > 0.0f)
            {
                fanDirection = -1;
            }
            else
                fanDirection = 1;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && canMove)
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

    //book 
    public void SetBook(bool book)
    {
        animator.SetBool("hasBook", book);
    }

    // SAVE AND LOAD
    public void Save(ref PlayerSaveData data)
    {
        data.Position = transform.position;
        data.levelUnlocked = GameManager.playerSaveData.levelUnlocked;
        data.worldUnlocked = GameManager.playerSaveData.worldUnlocked;
    }

    public void Load(PlayerSaveData data)
    {
        transform.position = data.Position;
        GameManager.playerSaveData.levelUnlocked = data.levelUnlocked;
        GameManager.playerSaveData.worldUnlocked = data.worldUnlocked;
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 Position;
    public int levelUnlocked;
    public int worldUnlocked;
}
