using System.Collections;
using UnityEngine;

public class ObjectParent : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = .1f;
    private float moveHorizontal;
    private float moveVertical;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!(Input.GetKeyDown(KeyCode.Return)))
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");
            rb.AddForce(new Vector2(moveHorizontal * speed, 0), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(0, moveVertical * speed), ForceMode2D.Impulse);
        }
        else {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.gravityScale = 1;
            this.enabled = false;
        }
    }
}
