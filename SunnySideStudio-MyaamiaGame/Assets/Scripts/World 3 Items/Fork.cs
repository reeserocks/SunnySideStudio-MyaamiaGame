using UnityEngine;

public class Fork : ObjectParent
{
    public bool isInWall = false;
    public bool tooFarIn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInWall)
        {
            tooFarIn = true;
        }
        else
        {
            isInWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (tooFarIn && isInWall)
        {
            isInWall = false;
        }
        else if (tooFarIn)
        {
            tooFarIn = false;
        }
        else
        {
            isInWall = false;
        }
    }

    private new void Update()
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
            if (!tooFarIn && isInWall)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = 1;
                collision.isTrigger = false;
                this.enabled = false;
            }
            else if (tooFarIn)
            {
                //play sound to indicate failure to spawn
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = 1;
                collision.isTrigger = false;
                this.enabled = false;
            }
        }
    }
}
