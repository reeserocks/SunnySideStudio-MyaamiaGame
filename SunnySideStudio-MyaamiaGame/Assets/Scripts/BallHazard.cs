using UnityEngine;

public class BallHazard : MonoBehaviour
{
    private Vector3 initialStart;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialStart = this.transform.position; 
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -10)
        {
            this.transform.position = initialStart;
            rb.linearVelocityY = 0;
            rb.linearVelocityY = 0;
        }
        else
        {
            rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -8,8);
            rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -8,8);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.usedByEffector == true && collision.gameObject.GetComponent<AreaEffector2D>() != null && collision.gameObject.GetComponent<AreaEffector2D>().forceMagnitude != 0) 
        {
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.usedByEffector == true && collision.gameObject.GetComponent<AreaEffector2D>() != null && collision.gameObject.GetComponent<AreaEffector2D>().forceMagnitude != 0)
        {
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.usedByEffector == true && collision.gameObject.GetComponent<AreaEffector2D>() != null && collision.gameObject.GetComponent<AreaEffector2D>().forceMagnitude != 0)
        {
            rb.gravityScale = 3;
        }
    }
}
