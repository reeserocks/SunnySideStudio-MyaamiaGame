using Unity.VisualScripting;
using UnityEngine;

public class Spoon : ObjectParent
{
    Rigidbody2D playerRb;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            animator.SetBool("isLaunching?", true);
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            animator.SetBool("isLaunching?", false);
        }
    }
}
