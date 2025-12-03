using NUnit.Framework;
using UnityEngine;

public class Seed : ObjectParent
{
    private Collider2D[] allCollisions;

    new private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        allCollisions = GetComponents<Collider2D>();
        foreach (Collider2D collider in allCollisions)
            collider.enabled = false;
        allCollisions[0].enabled = true;
        thisCollision = allCollisions[0];
        allCollisions[1].enabled = true;

        if (this.TryGetComponent<SpriteRenderer>(out SpriteRenderer tempRen))
        {
            spriteRen = tempRen;
        }
        else
        {
            spriteRen = this.GetComponentInChildren<SpriteRenderer>();
        }
        spriteRen.color = new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, .5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && this.spriteRen.color.a == 1)
        {
            //@reese play animation here
            foreach (Collider2D collider in allCollisions)
                collider.enabled = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
