using UnityEngine;

public class Fan : MonoBehaviour
{
    private RaycastHit playerHit;
    private bool shouldCheck = false;
    public Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindFirstObjectByType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            shouldCheck = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (shouldCheck)
        {
            if (Physics2D.Linecast(this.transform.position, player.gameObject.transform.position, 1, -0.31f, 0.01f).transform != this.transform)
            {
                player.pushedByFan = false;
                this.GetComponent<AreaEffector2D>().enabled = false;
            }
            else
            {
                player.pushedByFan = true;
                this.GetComponent<AreaEffector2D>().enabled = true;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            player.pushedByFan = false;
            shouldCheck = false;
            this.GetComponent<AreaEffector2D>().enabled = true;
        }
    }

}
