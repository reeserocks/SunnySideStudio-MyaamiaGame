using UnityEngine;

public class Ball : ObjectParent
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.position = this.gameObject.transform.position + new Vector3(0,2.37f,0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            this.GetComponent<Rigidbody2D>().linearVelocityX = collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityX;
        }
    }

}
