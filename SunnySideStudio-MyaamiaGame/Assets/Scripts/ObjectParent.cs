using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectParent : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Collider2D thisCollision;
    private float maxVelocity = 4.0f;
    protected float speed = .1f;
    protected float moveHorizontal;
    protected float moveVertical;
    protected bool isColliding = false;
    protected int collisionCount = 0;
    protected SpriteRenderer spriteRen;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        thisCollision = GetComponent<Collider2D>();
        thisCollision.isTrigger = true;
        if (this.TryGetComponent<SpriteRenderer>(out SpriteRenderer tempRen))
        {
            spriteRen = tempRen;
        }
        else
        {
            spriteRen = this.GetComponentInChildren<SpriteRenderer>();
        }
        spriteRen.color = new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, .5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isColliding = true;
            collisionCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            collisionCount--;
            if (collisionCount == 0)
            {
                isColliding = false;
            }
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
                GameManager.isPlacing = false;
                GameManager.canType = true;
                //play successsful spawn sound
                this.enabled = false;
                spriteRen.color = new Color(1, 1, 1, 1f);
            }
            else
            {
                AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("failure_summon"), Camera.main.transform.position);
                spriteRen.color = new Color(1, 0 , 0, .5f);
                StartCoroutine(ColorFlash());
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

    protected IEnumerator ColorFlash()
    {
        yield return new WaitForSecondsRealtime(.5f);
        spriteRen.color = new Color(1, 1, 1, 0.5f);
    }
}
