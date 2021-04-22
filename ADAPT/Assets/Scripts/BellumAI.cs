using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellumAI : MonoBehaviour
{
    public enum State { Patrol, Attack, Dead }
    public State currentState;

    Rigidbody2D rb2D;
    Animator anim;
    public GameObject graphics;

    float timer;
    [SerializeField] private float health = 5, speed = 250f, patrolRange = 2f;
    public bool canCharge = false, chargeTriggered = false, jumpTriggered = false, patrolling = false;
    public bool playerDetected = false, activeDamage = true;
    private bool dNASent = false;
    public Transform target;
    private Vector3 patrolTarget;

    void Start()
    {
        State CurrentState = State.Patrol;
        StartCoroutine(Patrol());

        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        FlipSprite();
        switch (currentState)
        {
            case State.Patrol:
                anim.SetBool("isAttacking", false);
                if (!patrolling)
                {
                    StartCoroutine(Patrol());
                }
                if (playerDetected)
                {
                    currentState = State.Attack;


                }

                //Move back and forth
                break;
            case State.Attack:
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", true);
                if (!playerDetected && health > 0)
                {
                    currentState = State.Patrol;
                    chargeTriggered = false;
                }
                timer += Time.deltaTime;
                if (!chargeTriggered)
                {
                    StartCoroutine(DelayCharge());
                }


                //Play attack animation, spawn hitbox
                break;
            case State.Dead:
                patrolling = false;
                //play animation
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isDead", true);

                if (!dNASent)
                {
                    target.gameObject.GetComponent<SivonController>().ConsumeDNA(SivonController.DNATypes.Bellum);
                    dNASent = true;
                }
                StopAllCoroutines();
                StartCoroutine(KillUnit());
                break;
        }


    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Patrol:
                if (rb2D.velocity.x != 0f)
                {
                    anim.SetBool("isWalking", true);
                }
                else
                {
                    anim.SetBool("isWalking", false);
                }
                break;

            case State.Attack:
                if (canCharge)
                {
                    StartCoroutine(EndChargeDelay());
                    Vector2 force = GetDirectionToTarget(target.position) * (speed * 2) * Time.deltaTime;
                    if (rb2D.velocity.x <= 0.05f || rb2D.velocity.x >= -0.05f && !jumpTriggered)
                    {
                        StartCoroutine(AddJumpForce());

                    }
                    rb2D.AddForce(force);
                }
                break;

            case State.Dead:
                rb2D.velocity = Vector2.zero;
                break;
        }
    }

    public void HitTarget(float damage)
    {
        rb2D.AddForce((GetDirectionToTarget(target.position).normalized * (damage * 10)), ForceMode2D.Impulse); health -= damage;

        if (health <= 0)
        {
            currentState = State.Dead;
        }
    }
    private Vector2 Direction(Vector3 tempTarget)
    {

        if (tempTarget.x - this.gameObject.transform.position.x < 0)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.right;
        }
    }
    private bool CheckAnimation()
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f < 1.0f;
    }

    private void FlipSprite()
    {
        if (rb2D.velocity.x > 0f)
        {

            graphics.transform.localScale = new Vector3(-.5f, .5f, 1);
        }
        else if (rb2D.velocity.x < 0f)
        {

            graphics.transform.localScale = new Vector3(.5f, .5f, 1);

        }
        else
        {
            return;
        }
    }
    private Vector2 GetDirectionToTarget(Vector3 tempTarget)
    {

        if (tempTarget.x - rb2D.position.x < 0)
        {
            return Vector2.left;
        }
        else if (tempTarget.x - rb2D.position.x > 0)
        {
            return Vector2.right;
        }
        else
        {
            return Vector2.zero;
        }

    }

    public void RecieveVisual(bool result)
    {
        playerDetected = result;
    }

    IEnumerator AddJumpForce()
    {
        jumpTriggered = true;
        rb2D.AddForce(new Vector2(rb2D.velocity.x, 500f));
        yield return new WaitForSeconds(2f);
        jumpTriggered = false;
    }

    IEnumerator EndChargeDelay()
    {
        canCharge = false;
        yield return new WaitForSeconds(2f);
        StartCoroutine(DelayCharge());
    }

    IEnumerator DelayCharge()
    {
        chargeTriggered = true;
        yield return new WaitForSeconds(4f);
        canCharge = true;

    }

    IEnumerator Patrol()
    {

        patrolling = true;
        yield return new WaitForSeconds(5f);
        if (currentState == State.Patrol)
        {
            Debug.Log("Patrolled once");
            patrolTarget = new Vector3(Random.Range(this.transform.position.x - patrolRange, this.transform.position.x + patrolRange), this.transform.position.y, this.transform.position.z);
            Vector2 force = GetDirectionToTarget(patrolTarget) * speed * Time.deltaTime;
            rb2D.AddForce(force, ForceMode2D.Impulse);
            patrolling = false;
        }
    }

    IEnumerator KillUnit()
    {
        yield return new WaitUntil(() => CheckAnimation() == true);
        Destroy(this.gameObject);

    }
}
