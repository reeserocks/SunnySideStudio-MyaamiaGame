using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectParent : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D collision;
    private float maxVelocity = 5.0f;
    private float speed = .1f;
    private float moveHorizontal;
    private float moveVertical;
    private bool isColliding = false;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collision = GetComponent<BoxCollider2D>();
        collision.isTrigger = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!(Input.GetKeyDown(KeyCode.Return)))
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");
            rb.AddForce(new Vector2(moveHorizontal * speed, 0), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(0, moveVertical * speed), ForceMode2D.Impulse);
            capVelocity();
        }
        else {
            if (!isColliding)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.gravityScale = 1;
                collision.isTrigger = false;
                this.enabled = false;
            }
            else
            {
                //play sound to indicate failure to spawn
            }
        }
    }

    protected void capVelocity ()
    {
        float cappedXVelocity = Mathf.Min(Mathf.Abs(rb.linearVelocityX), maxVelocity) * Mathf.Sign(rb.linearVelocityX);
        float cappedYVelocity = Mathf.Min(Mathf.Abs(rb.linearVelocityY), maxVelocity) * Mathf.Sign(rb.linearVelocityY);

        rb.linearVelocity = new Vector2(cappedXVelocity, cappedYVelocity);
    }
}
