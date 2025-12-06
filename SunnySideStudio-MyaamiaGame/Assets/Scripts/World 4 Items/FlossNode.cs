using UnityEngine;

public class FlossNode : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Floss floss = transform.parent.GetComponent<Floss>();
        floss.childTriggerEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Floss floss = transform.parent.GetComponent<Floss>();
        floss.childTriggerExit(collision);
    }
}
