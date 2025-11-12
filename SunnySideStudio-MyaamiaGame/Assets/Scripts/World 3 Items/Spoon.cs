using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Spoon : ObjectParent
{
    Rigidbody2D playerRb;
    Animator animator;

    public float catapultTimer = 1f;
    public float motorSpeed;
    public float motorForce;

    private HingeJoint2D hinge;
    private JointMotor2D motor;

    private float resetCatapultTimer = 0.1f;
    private float timer;
    private bool timerStart;

    void Start()
    {
        animator = GetComponent<Animator>();

        hinge = GetComponent<HingeJoint2D>();
        motor = hinge.motor;
    }

    void LateUpdate()
    {
        if (timerStart)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                hinge.useMotor = false;
                motor.motorSpeed = motorSpeed;
                motor.maxMotorTorque = motorForce;
                hinge.motor = motor;
                hinge.useLimits = false;
                hinge.useMotor = true;

                StartCoroutine(ResetCatapult());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            timerStart = true;
            //animator.SetBool("isLaunching?", true);
        }
    }

    IEnumerator ResetCatapult() 
    {
        timerStart = false;
        yield return new WaitForSeconds(resetCatapultTimer);

        motor.motorSpeed = 100f;
        motor.maxMotorTorque = 20f;
        hinge.motor = motor;
        hinge.useLimits = true;
        hinge.motor = motor;
        timer = catapultTimer;
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            timerStart = false;
            //animator.SetBool("isLaunching?", false);
        }
    }
}
