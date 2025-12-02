using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Animator animator;
    private Vector3 startLocation;
    private float speed = 1;
    private BoxCollider2D[] colliders;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var others = FindFirstObjectByType<Bubble>();

        if (others != this)
        {
            Destroy(this.gameObject);
        }

        colliders = GetComponents<BoxCollider2D>();
        animator = GetComponent<Animator>();
        startLocation = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            speed = 3f;
            StartCoroutine(popWithPlayer());
            colliders[0].enabled = false;
        }
        else
        {
            StartCoroutine(pop());
            colliders[0].enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            speed = 1f;
            //StopCoroutine(popWithPlayer());
        }
    }

    IEnumerator popWithPlayer()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(pop());
    }

    IEnumerator pop()
    {
        colliders[1].enabled = false;
        animator.SetBool("pop?", true);
        speed = 0;
        yield return new WaitForSeconds(1f);
        Reset();
    }

    private void Reset()
    {
        this.transform.position = startLocation;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        colliders[0].enabled = true;
        colliders[1].enabled = true;
        speed = 1;
        animator.SetBool("pop?", false);
    }
}
