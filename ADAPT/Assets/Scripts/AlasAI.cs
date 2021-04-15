using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlasAI : MonoBehaviour
{
    public enum State { Patrol, Attack, Dead }
    public State currentState;

    Rigidbody2D rb2D;
    Animator anim;
    public GameObject graphics;

    private Vector3 patrolTarget, startLocation;
    [SerializeField] private float health = 5, speed = 250f, patrolRange = 2f;
    private bool attacking, patrolling;
    public bool playerDetected = false, activeDamage = true;
    public Transform target;
    // private Vector3 patrolTarget;

    void Start()
    {
        State CurrentState = State.Patrol;

        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        startLocation = rb2D.position;

    }

    void Update()
    {
        FlipSprite();
        switch (currentState)
        {
            case State.Patrol:
                anim.SetBool("isAttacking", false);
                if (playerDetected)
                {
                    currentState = State.Attack;
                }

                //Move back and forth
                break;
            case State.Attack:
                anim.SetBool("isAttacking", true);
                if (!playerDetected && health > 0)
                {
                    currentState = State.Patrol;
                }


                //Play attack animation, spawn hitbox
                break;
            case State.Dead:
                anim.SetBool("isAttacking", false);
                anim.SetBool("isDead", true);
                //play animation
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
                if (!patrolling) {
                    StartCoroutine(Patrol());
                }

                /*rb2D.AddForce(GetDirectionToTarget(patrolTarget) * speed * Time.deltaTime);

                if (rb2D.position == (Vector2)patrolTarget)
                {
                    rb2D.velocity = Vector2.zero;
                }*/
                break;

            case State.Attack:
                Vector2 force = GetDirectionToTarget(target.position) * speed * Time.deltaTime;
                rb2D.AddForce(force);
                break;

            case State.Dead:
                rb2D.velocity = Vector2.zero;
                break;
        }
    }

    public void HitTarget(float damage, Vector2 direction, float knockbackFactor)
    {
        rb2D.AddForce((direction.normalized * knockbackFactor), ForceMode2D.Impulse);
        health -= damage;

        if (health <= 0)
        {
            currentState = State.Dead;
        }
    }

    private bool CheckAnimation()
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f < 1.0f;
    }

    private void FlipSprite()
    {
        if (currentState != State.Attack)
        {
            if (rb2D.velocity.x > 0f)
            {
               
                graphics.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (rb2D.velocity.x < 0f)
            {
              
                graphics.transform.localScale = new Vector3(1f, 1f, 1f);

            }
            else
            {
                return;
            }
        }
        else
        {
            if (target.position.x - rb2D.position.x < 0)
            {
               
                graphics.transform.localScale = new Vector3(1f, 1f, 1f);

            }
            else if (target.position.x - rb2D.position.x > 0)
            {
               
                graphics.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                return;
            }
        }
    }
    private Vector2 GetDirectionToTarget(Vector3 tempTarget)
    {
        Vector2 temp = (Vector2)tempTarget - rb2D.position;
        return temp.normalized;

    }

    public void RecieveVisual(bool result)
    {
        playerDetected = result;
    }


    IEnumerator Patrol()
    {

        patrolling = true;
        yield return new WaitForSeconds(5f);
        if (currentState == State.Patrol)
        {
            Debug.Log("Patrolled once");
            patrolTarget = new Vector3(Random.Range(startLocation.x - patrolRange, startLocation.x + patrolRange), Random.Range(startLocation.y - patrolRange, startLocation.y + patrolRange), startLocation.z);
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
