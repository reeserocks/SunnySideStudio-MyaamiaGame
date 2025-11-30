using UnityEngine;

public class Book : ObjectParent
{
    new private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        thisCollision = GetComponent<BoxCollider2D>();
        thisCollision.isTrigger = true;
        Transform thisTransform = GetComponent<Transform>();
        thisTransform.localRotation = new Quaternion(0,0,90,0);
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
}
