using System;
using System.Collections;
using UnityEngine;

public class Pillow : ObjectParent
{
    [SerializeField] Transform transformComp;
    Boolean hasFlattened = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && !hasFlattened)
        {
            transformComp.localScale = new Vector3(transformComp.localScale.x, transformComp.localScale.y / 2, transformComp.localScale.z);
            hasFlattened = true;
            StartCoroutine(waitTime(collision));
        }
    }

    IEnumerator waitTime (Collision2D collision)
    {
        yield return new WaitForSecondsRealtime(3);
        yield return new WaitForFixedUpdate();
        Debug.Log("lanuching");
        Rigidbody2D otherBody = collision.gameObject.GetComponent<Rigidbody2D>();
        Debug.Log(otherBody.gameObject);
        if (otherBody != null)
        {
            otherBody.GetComponent<Player>().isJumping = true;
            otherBody.AddForce(new Vector2(0, 45.0f), ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("error in getting rigid body");
        }
    }
}
