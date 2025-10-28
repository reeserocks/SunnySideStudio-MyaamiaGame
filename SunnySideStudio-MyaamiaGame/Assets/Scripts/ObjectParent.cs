using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectParent : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected BoxCollider2D thisCollision;
    private float maxVelocity = 4.0f;
    protected float speed = .1f;
    protected float moveHorizontal;
    protected float moveVertical;
    protected bool isColliding = false;
    protected int collisionCount = 0;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        thisCollision = GetComponent<BoxCollider2D>();
        thisCollision.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
        collisionCount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionCount--;
        if (collisionCount == 0)
        {
            isColliding = false;
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!(Input.GetKeyDown(KeyCode.Return)))
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");
            rb.AddForce(new Vector2(moveHorizontal * speed, 0), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(0, moveVertical * speed), ForceMode2D.Impulse);
            capVelocity();
        }
        else {
            if (!isColliding)
            {
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = 1;
                thisCollision.isTrigger = false;
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
        if (moveHorizontal == 0) { cappedXVelocity = 0; }
        if (moveVertical == 0) {cappedYVelocity = 0; }

        rb.linearVelocity = new Vector2(cappedXVelocity, cappedYVelocity);
    }
}
