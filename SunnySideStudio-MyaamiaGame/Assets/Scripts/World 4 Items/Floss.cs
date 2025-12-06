using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Floss : MonoBehaviour
{
    private LineRenderer line;
    private List<Transform> points = new List<Transform>();
    private GameObject startPoint;
    private GameObject endPoint;
    private SpriteRenderer startSprite;
    private SpriteRenderer endSprite;
    private Rigidbody2D startRb;
    private Rigidbody2D endRb;
    private Collider2D startCollision;
    private Collider2D endCollision;
    private bool isColliding = false;
    private int collisionCount = 0;
    private int placementPhase = 0;
    private GameObject tempObject;

    protected float maxVelocity = 4.0f;
    protected float speed = .1f;
    protected float moveHorizontal;
    protected float moveVertical;

    private bool bothMove;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        startPoint = gameObject.transform.Find("StartPoint").gameObject;
        endPoint = gameObject.transform.Find("EndPoint").gameObject;
        points.Add(startPoint.transform);
        points.Add(endPoint.transform);
        startSprite = startPoint.GetComponent<SpriteRenderer>();
        endSprite = endPoint.GetComponent<SpriteRenderer>();
        startSprite.color = new Color(startSprite.color.r, startSprite.color.g, startSprite.color.b, 0.5f);
        endSprite.color = new Color(endSprite.color.r, endSprite.color.g, endSprite.color.b, 0.5f);
        startRb = startPoint.GetComponent<Rigidbody2D>();
        endRb = endPoint.GetComponent<Rigidbody2D>();
        startCollision = startPoint.GetComponent<Collider2D>();
        endCollision = endPoint.GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (placementPhase == 0)
        {
            if(MoveObject(startRb, startSprite, startCollision))
            {
                GameObject oldPoint = startPoint;
                startPoint = tempObject;
                points[0] = startPoint.transform;
                startRb = startPoint.GetComponent<Rigidbody2D>();
                Destroy(oldPoint.gameObject);
                tempObject = null;
            }
        }
        else if (placementPhase == 1) { 
            if(MoveObject(endRb, endSprite, endCollision))
            {
                GameObject oldPoint = endPoint;
                endPoint = tempObject;
                points[1] = endPoint.transform;
                endRb = endPoint.GetComponent<Rigidbody2D>();
                Destroy(oldPoint.gameObject);
                tempObject = null;
                if (startPoint.name.Contains("Platform") || endPoint.name.Contains("Platform"))
                {
                    bothMove = false;
                }
                else
                    bothMove = true;
            }
        }
        else if (bothMove)
        {

            //Manage connected object velocities
        }

        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i].position);
        }
    }

    private bool MoveObject(Rigidbody2D rb, SpriteRenderer spriteRen, Collider2D collision)
    {
        if (!(Input.GetKeyDown(KeyCode.Return)))
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");
            rb.AddForce(new Vector2(moveHorizontal * speed, 0), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(0, moveVertical * speed), ForceMode2D.Impulse);
            capVelocity(rb);
        }
        else
        {
            if (isColliding)
            {
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (placementPhase == 1) {
                    GameManager.isPlacing = false;
                    GameManager.canType = true;
                }
                //play successsful spawn sound
                placementPhase++;
                spriteRen.color = new Color(1, 1, 1, 1f);
                isColliding = false;
                collisionCount = 1;
                return true;
            }
            else
            {
                AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("failure_summon"), Camera.main.transform.position);
                spriteRen.color = new Color(1, 0, 0, .5f);
                StartCoroutine(ColorFlash(spriteRen));
            }
        }
        return false;
    }

    public void childTriggerEnter(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isColliding = true;
            collisionCount++;
            tempObject = collision.gameObject;
        }
    }
    
    public void childTriggerExit(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            collisionCount--;
            if (collisionCount == 0)
            {
                isColliding = false;
                tempObject = null;
            }
        }
    }

    protected void capVelocity(Rigidbody2D rb)
    {
        float cappedXVelocity = Mathf.Min(Mathf.Abs(rb.linearVelocityX), maxVelocity) * Mathf.Sign(rb.linearVelocityX);
        float cappedYVelocity = Mathf.Min(Mathf.Abs(rb.linearVelocityY), maxVelocity) * Mathf.Sign(rb.linearVelocityY);
        if (moveHorizontal == 0) { cappedXVelocity = 0; }
        if (moveVertical == 0) { cappedYVelocity = 0; }

        rb.linearVelocity = new Vector2(cappedXVelocity, cappedYVelocity);
    }

    protected IEnumerator ColorFlash(SpriteRenderer spriteRen)
    {
        yield return new WaitForSecondsRealtime(.5f);
        spriteRen.color = new Color(1, 1, 1, 0.5f);
    }
}
