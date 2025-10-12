using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectParent : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D collision;
    private float maxVelocity = 5.0f;
    private float speed = .1f;
    private float moveHorizontal;
    private float moveVertical;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collision = GetComponent<BoxCollider2D>();
        collision.isTrigger = true;
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
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.gravityScale = 1;
            collision.isTrigger = false;
            this.enabled = false;
        }
    }

    protected void capVelocity ()
    {
        float cappedXVelocity = Mathf.Min(Mathf.Abs(rb.linearVelocityX), maxVelocity) * Mathf.Sign(rb.linearVelocityX);
        float cappedYVelocity = Mathf.Min(Mathf.Abs(rb.linearVelocityY), maxVelocity) * Mathf.Sign(rb.linearVelocityY);

        rb.linearVelocity = new Vector2(cappedXVelocity, cappedYVelocity);
    }
}
