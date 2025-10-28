using UnityEngine;

public class Book : ObjectParent
{
    new private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        thisCollision = GetComponent<BoxCollider2D>();
        thisCollision.isTrigger = true;
    }
}
