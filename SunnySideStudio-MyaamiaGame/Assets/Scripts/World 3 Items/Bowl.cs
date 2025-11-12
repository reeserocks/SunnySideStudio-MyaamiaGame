using Unity.VisualScripting;
using UnityEngine;

public class Bowl : ObjectParent
{
    private AreaEffector2D effector;
    private Water water;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        effector = GetComponent<AreaEffector2D>();  
        effector.enabled = false;
    }

    // Update is called once per frame
    new void Update()
    {
        if (!(Input.GetKeyDown(KeyCode.Return)))
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                water.SwitchDirection(this.transform.localScale.x);
            }
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");
            rb.AddForce(new Vector2(moveHorizontal * speed, 0), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(0, moveVertical * speed), ForceMode2D.Impulse);
            capVelocity();
        }
        else
        {
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
                spriteRen.color = new Color(1, 0, 0, .5f);
                StartCoroutine(ColorFlash());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isColliding = true;
            collisionCount++;
        }
        else if (collision.gameObject.CompareTag("Fan"))
        {
            if (collision.gameObject.transform.localScale.x > 0.0f && this.transform.localScale.x < 0.0f)
            {
                effector.enabled = true;
            }
            else if (collision.gameObject.transform.localScale.x < 0.0f && this.transform.localScale.x > 0.0f)
            {
                effector.enabled = true;
            }
        }
        else if (collision.gameObject.CompareTag("SwitchableWater"))
        {
            water = collision.gameObject.ConvertTo<Water>();
            water.SwitchDirection(this.transform.localScale.x);
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
        else if (collision.gameObject.CompareTag("Fan"))
        {
            effector.enabled = false;
        }
        else if (collision.gameObject.CompareTag("SwitchableWater"))
        {
            water.SwitchDirection(this.transform.localScale.x);
            water = null;
        }
    }
}
