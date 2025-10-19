// HANGER.CS

using UnityEngine;

public class Hanger : ObjectParent
{
    float swingTorque = 50f;
    float maxRotation = 60f;
    float jumpForce = 12f;
    float swingBoostMultiplier = 0.1f;
    float maxSwingSpeed = 300f;

    bool isPlayerAttached;
    Rigidbody2D playerRb;
    HingeJoint2D joint;

    void Start()
    {
        collision.isTrigger = true;
    }

    new void Update()
    {
        if (isPlayerAttached && playerRb)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // swing hanger
            rb.AddTorque(-horizontal * swingTorque * Time.deltaTime, ForceMode2D.Force);

            // clamp rotation
            rb.rotation = Mathf.Clamp(rb.rotation, -maxRotation, maxRotation);

            // rotate player with hanger
            // playerRb.MoveRotation(rb.rotation);

            // jump off
            if (vertical != 0)
            {
                DetachPlayer();
            }
        }
        else
        {
            // settle back naturally
            rb.angularVelocity *= 0.95f;
            rb.rotation = Mathf.Lerp(rb.rotation, 0f, Time.deltaTime * 2f);
        }
    }

    new void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player") && !isPlayerAttached)
        {
            playerRb = c.GetComponent<Rigidbody2D>();
            if (playerRb)
            {
                AttachPlayer();
            }
        }
    }

    void AttachPlayer()
    {
        joint = playerRb.gameObject.AddComponent<HingeJoint2D>();
        joint.connectedBody = rb;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = Vector2.zero;

        isPlayerAttached = true;
    }

    void DetachPlayer()
    {
        if (!playerRb)
        {
            return;
        }

        float swingSpeed = rb.angularVelocity;

        // cap swing speed for jump
        float cappedSwing = Mathf.Clamp(Mathf.Abs(swingSpeed), 0f, maxSwingSpeed);

        if (joint)
        {
            Destroy(joint);
        }

        // tangent direction relative to hanger
        float angleRad = rb.rotation * Mathf.Deg2Rad;
        Vector2 tangentDir = new Vector2(Mathf.Cos(angleRad + Mathf.PI / 2f), Mathf.Sin(angleRad + Mathf.PI / 2f)).normalized;

        // calculate jump force
        float boostedJump = jumpForce + cappedSwing * swingBoostMultiplier;

        // apply jump
        playerRb.AddForce(tangentDir * boostedJump, ForceMode2D.Impulse);

        // reset state
        isPlayerAttached = false;
        playerRb = null;
        joint = null;
    }
}
