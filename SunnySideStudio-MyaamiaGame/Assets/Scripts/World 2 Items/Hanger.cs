using UnityEngine;

public class Hanger : ObjectParent
{
    //float swingTorque = 30f;
    //float maxRotation = 60f;
    float jumpForce = 12f;
    float swingBoostMultiplier = 0.1f;
    float maxSwingSpeed = 300f;

    Rigidbody2D playerRb;
    HingeJoint2D joint;
    //bool isPlayerAttached;

    bool canBePlaced = false;
    Transform placementZoneTransform = null;

    Vector2 fixedPos;

    new void Update()
    {
        if (thisCollision.isTrigger)
        {
            // handle arrow movement for placement
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");
            rb.AddForce(new Vector2(moveHorizontal * speed, 0), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(0, moveVertical * speed), ForceMode2D.Impulse);
            capVelocity();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnPlaced(); // custom placement logic
            }
            return;
        }

        //// after placement - swinging logic
        //if (isPlayerAttached && playerRb)
        //{
        //    float horizontal = Input.GetAxisRaw("Horizontal");
        //    float vertical = Input.GetAxisRaw("Vertical");

        //    // add torque
        //    rb.AddTorque(-horizontal * swingTorque * Time.deltaTime, ForceMode2D.Force);

        //    // clamp rotation
        //    rb.rotation = Mathf.Clamp(rb.rotation, -maxRotation, maxRotation);

        //    if (vertical != 0)
        //        DetachPlayer();
        //}
        //else
        //{
        //    // settle hanger back to rest
        //    rb.angularVelocity *= 0.95f;
        //    rb.rotation = Mathf.Lerp(rb.rotation, 0f, Time.deltaTime * 1.5f);
        //}
    }

    // confirmed placement
    public void OnPlaced()
    {
        if (!canBePlaced || placementZoneTransform == null)
        {
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("failure_summon"), Camera.main.transform.position);
            spriteRen.color = new Color(1, 0, 0, 0.5f);
            StartCoroutine(ColorFlash());
            return;
        }
        else
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rb.angularDamping = 1.5f;
            thisCollision.isTrigger = false;

            fixedPos = rb.position;

            spriteRen.color = new Color(1, 1, 1, 1);
            placementZoneTransform = null;
            canBePlaced = false;
            GameManager.isPlacing = false;
            GameManager.canType = true;
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("HangerPlacement"))
        {
            canBePlaced = true;
            placementZoneTransform = c.transform;
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("HangerPlacement"))
        {
            canBePlaced = false;
            placementZoneTransform = null;
        }
    }

    //void OnCollisionEnter2D(Collision2D c)
    //{
    //    if (thisCollision.isTrigger) return;

    //    if (c.collider.CompareTag("Player") && !isPlayerAttached)
    //    {
    //        playerRb = c.collider.GetComponent<Rigidbody2D>();
    //        if (playerRb) AttachPlayer();
    //    }
    //}

    void AttachPlayer()
    {
        joint = playerRb.gameObject.AddComponent<HingeJoint2D>();
        joint.connectedBody = rb;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = new Vector2(0, -7f);

        playerRb.GetComponent<Player>()?.SetHanging(true);
        //isPlayerAttached = true;
    }

    void DetachPlayer()
    {
        if (!playerRb) return;

        float swingSpeed = Mathf.Clamp(Mathf.Abs(rb.angularVelocity), 0f, maxSwingSpeed);
        if (joint) Destroy(joint);

        float angleRad = rb.rotation * Mathf.Deg2Rad;
        Vector2 tangent = new(Mathf.Cos(angleRad + Mathf.PI / 2f), Mathf.Sin(angleRad + Mathf.PI / 2f));
        float jumpPower = jumpForce + swingSpeed * swingBoostMultiplier;

        playerRb.AddForce(tangent * jumpPower, ForceMode2D.Impulse);
        playerRb.GetComponent<Player>()?.SetHanging(false);

        //isPlayerAttached = false;
        playerRb = null;
        joint = null;
    }

    void FixedUpdate()
    {
        if (!thisCollision.isTrigger)
            rb.position = fixedPos;
    }
}
