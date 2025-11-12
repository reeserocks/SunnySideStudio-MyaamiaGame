using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Spoon : ObjectParent
{
    Animator animator;

    public float catapultTimer = 1f;
    public float motorSpeed;
    public float motorForce;

    private HingeJoint2D hinge;
    private JointMotor2D motor;

    private float resetCatapultTimer = 0.1f;
    private float timer;
    private bool timerStart = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        hinge = GetComponent<HingeJoint2D>();
        hinge.enabled = false;
        motor = hinge.motor;
    }

    void LateUpdate()
    {
        if (timerStart)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                hinge.useMotor = false;
                motor.motorSpeed = motorSpeed;
                motor.maxMotorTorque = motorForce;
                hinge.motor = motor;
                hinge.useLimits = false;
                hinge.useMotor = true;

                StartCoroutine(ResetCatapult());
            }
        }
    }

    new void Update()
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

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            timerStart = true;
            //animator.SetBool("isLaunching?", true);
        }
        if (c.CompareTag("Ground"))
        {
            isColliding = true;
            collisionCount++;
        }
    }

    IEnumerator ResetCatapult() 
    {
        timerStart = false;
        yield return new WaitForSeconds(resetCatapultTimer);

        motor.motorSpeed = 100f;
        motor.maxMotorTorque = 20f;
        hinge.motor = motor;
        hinge.useLimits = true;
        hinge.motor = motor;
        timer = catapultTimer;
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            timerStart = false;
            //animator.SetBool("isLaunching?", false);
        }
        if (c.CompareTag("Ground"))
        {
            collisionCount--;
            if (collisionCount == 0)
            {
                isColliding = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && rb.gravityScale == 1)
        {
            hinge.enabled = true;
        }
    }
}
