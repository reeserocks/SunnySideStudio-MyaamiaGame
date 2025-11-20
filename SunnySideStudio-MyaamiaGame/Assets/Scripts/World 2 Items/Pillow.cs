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
        Rigidbody2D otherBody = collision.gameObject.GetComponent<Rigidbody2D>();
        otherBody.AddForce(new Vector2(0, 75.0f), ForceMode2D.Impulse);
    }
}
