using UnityEngine;

public class Hanger : ObjectParent
{
    [SerializeField] float swingTorque = 50f;
    [SerializeField] float maxRotation = 60f;
    [SerializeField] float jumpForce = 12f;
    [SerializeField] float swingBoostMultiplier = 0.1f;
    [SerializeField] float maxSwingSpeed = 300f;

    Rigidbody2D playerRb;
    HingeJoint2D joint;
    bool isPlayerAttached;

    bool canBePlaced = false;
    Transform placementZoneTransform = null;

    new void Update()
    {
        if (thisCollision.isTrigger)
        {
            // Only handle arrow movement for placement
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

        // After placement - swinging logic
        if (isPlayerAttached && playerRb)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Add torque to swing hanger
            rb.AddTorque(-horizontal * swingTorque * Time.deltaTime, ForceMode2D.Force);

            // Clamp rotation for safety
            rb.rotation = Mathf.Clamp(rb.rotation, -maxRotation, maxRotation);

            if (vertical != 0)
                DetachPlayer();
        }
        else
        {
            // Smoothly settle hanger back to rest
            rb.angularVelocity *= 0.95f;
            rb.rotation = Mathf.Lerp(rb.rotation, 0f, Time.deltaTime * 1.5f);
        }
    }

    // Called when player confirms placement
    public void OnPlaced()
    {
        if (!canBePlaced || placementZoneTransform == null)
        {
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("failure_summon"), Camera.main.transform.position);
            spriteRen.color = new Color(1, 0, 0, 0.5f);
            StartCoroutine(ColorFlash());
            return;
        }

        // Lock position but allow rotation
        thisCollision.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        // rotation is unlocked for swinging

        spriteRen.color = new Color(1, 1, 1, 1);
        placementZoneTransform = null;
        canBePlaced = false;
        GameManager.isPlacing = false;
    }

    // Detect valid placement zones
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("HangerPlacement"))
        {
            Debug.Log("Hanger can be placed here.");
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

    void OnCollisionEnter2D(Collision2D c)
    {
        if (thisCollision.isTrigger) return;

        if (c.collider.CompareTag("Player") && !isPlayerAttached)
        {
            playerRb = c.collider.GetComponent<Rigidbody2D>();
            if (playerRb) AttachPlayer();
        }
    }

    void AttachPlayer()
    {
        joint = playerRb.gameObject.AddComponent<HingeJoint2D>();
        joint.connectedBody = rb;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = Vector2.zero;

        playerRb.GetComponent<Player>()?.SetHanging(true);
        isPlayerAttached = true;
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

        isPlayerAttached = false;
        playerRb = null;
        joint = null;
    }
}
