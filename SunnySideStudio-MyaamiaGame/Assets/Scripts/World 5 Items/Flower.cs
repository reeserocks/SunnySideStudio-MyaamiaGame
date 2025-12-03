using System.Collections;
using UnityEngine;

public class Flower : ObjectParent
{
    private bool isSpawned = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSpawned && collision.CompareTag("Player")) {
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            collision.GetComponent<Player>().jumpForce = 11.5f;
            this.transform.parent = collision.transform;
            StartCoroutine(collision.GetComponent<Player>().floatingState(this));
        }
        if (collision.CompareTag("Ground"))
        {
            isColliding = true;
            collisionCount++;
        }
    }

    new protected void Update()
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
        else
        {
            if (!isColliding)
            {
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = 1;
                thisCollision.isTrigger = false;
                isSpawned = true;
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
}
