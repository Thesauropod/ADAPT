using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmatusAI : MonoBehaviour
{
    public enum State { Patrol, Dead }
    public State currentState;

    Rigidbody2D rb2D;
    Animator anim;
    public GameObject graphics;

    [SerializeField] private GameObject[] waypoints;
    public int index = 0;
    private Vector3 patrolTarget, startLocation;
    [SerializeField] private float health = 5, speed = 250f, patrolRange = 2f;
    private bool attacking, patrolling;
    public bool activeDamage = true;
    public Transform target;

    private Vector3 targetDirection;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb2D.AddForce(new Vector2(100, 0));
        }

        FlipSprite();
        switch (currentState)
        {
            case State.Patrol:
                if (waypoints.Length > 0)
                {
                    targetDirection = waypoints[index].transform.position - transform.position;
                    rb2D.AddForce(targetDirection.normalized * speed * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg));
                }
                break;
            case State.Dead:
                StopAllCoroutines();
                StartCoroutine(KillUnit());
                break;
        }


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided?");
        if (other.gameObject == waypoints[index])
        {
            index = (index + 1) % waypoints.Length;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Patrol:
         /*       if (!patrolling)
                {
                    StartCoroutine(Patrol());
                }
         */

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
    private Vector2 GetDirectionToTarget(Vector3 tempTarget)
    {
        Vector2 temp = (Vector2)tempTarget - rb2D.position;
        return temp.normalized;

    }

 /*   IEnumerator Patrol()
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
    }*/

    IEnumerator KillUnit()
    {
        yield return new WaitUntil(() => CheckAnimation() == true);
        Destroy(this.gameObject);

    }
}
