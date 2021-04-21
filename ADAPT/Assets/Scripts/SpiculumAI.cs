using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiculumAI : MonoBehaviour
{
    public enum State { Patrol, Attack, Dead }
    public State currentState;

    Rigidbody2D rb2D;
    Animator anim;
    public GameObject graphics;


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
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", true);
                if (!playerDetected && health > 0)
                {
                    currentState = State.Patrol;
                }


                //Play attack animation, spawn hitbox
                break;
            case State.Dead:
                anim.SetBool("isWalking", false);
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
                if (rb2D.velocity.x != 0f)
                {
                    anim.SetBool("isWalking", true);
                }
                else
                {
                    anim.SetBool("isWalking", false);
                }
                /*rb2D.AddForce(GetDirectionToTarget(patrolTarget) * speed * Time.deltaTime);

                if (rb2D.position == (Vector2)patrolTarget)
                {
                    rb2D.velocity = Vector2.zero;
                }*/
                break;

            case State.Attack:
                if (!attacking)
                {
                    StartCoroutine(Attack());
                }
                break;

            case State.Dead:
                rb2D.velocity = Vector2.zero;
                break;
        }
    }

    public void HitTarget(float damage, float knockbackFactor)
    {
        rb2D.AddForce((GetDirectionToTarget(target.position).normalized * knockbackFactor), ForceMode2D.Impulse);
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

    IEnumerator Attack()
    {
        attacking = true;
        yield return new WaitForSeconds(Random.Range(3, 5));
        if (currentState == State.Attack)
        {
            Vector2 force = GetDirectionToTarget(target.position) * speed;
            rb2D.AddForce(force, ForceMode2D.Impulse);
            attacking = false;
        }
        else
        {
            StopCoroutine(Attack());
        }
    }




    IEnumerator KillUnit()
    {
        yield return new WaitUntil(() => CheckAnimation() == true);
        Destroy(this.gameObject);

    }
}
