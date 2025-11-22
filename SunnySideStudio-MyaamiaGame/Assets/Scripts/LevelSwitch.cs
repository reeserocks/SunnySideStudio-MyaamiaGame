using UnityEngine;

public class LevelSwitch : MonoBehaviour
{
    public GameObject attachedObject;
    public Vector3 endLocation;
    private bool isFan = false;
    private Vector3 startLoc;
    private bool activated = false;
    public bool goesBack;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (attachedObject.tag == "Fan")
        {
            isFan = true;
        }
        startLoc = attachedObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (isFan)
            {
                attachedObject.GetComponent<Collider2D>().enabled = false;
                attachedObject.GetComponent<Animator>().enabled = false;
            }
            else if (goesBack)
            {
                if (Vector2.Distance(endLocation, attachedObject.transform.position) < 0.02f)
                {
                    endLocation = startLoc;
                    goesBack = false;
                }
                attachedObject.transform.position = Vector2.MoveTowards(attachedObject.transform.position, endLocation, speed * Time.deltaTime);
            }
            else
            {
                attachedObject.transform.position = Vector2.MoveTowards(attachedObject.transform.position, endLocation, speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            activated = true;
        }
    }
}
