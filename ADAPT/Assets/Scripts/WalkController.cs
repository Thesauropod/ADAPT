using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkController : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    private CapsuleCollider m_capsuleCollider;
    private Vector3 velocity = Vector3.zero;
    private float facing = 1;
    private bool grounded = false;
    private bool jumping = false;
    private bool dashing = false;

    [SerializeField]
    private float m_walkAcceleration;
    [SerializeField]
    private float m_maxWalkSpeed;
    [SerializeField]
    private float m_dashSpeed;
    [SerializeField]
    private float m_dashDuration;
    [SerializeField]
    private float m_jumpForce;
    [SerializeField]
    private float m_baseGravity;
    [SerializeField]
    private float m_jumpGravity;
    [SerializeField]
    private float m_terminalVelocity;
    [SerializeField]
    private float m_deceleration;
    [SerializeField]
    private float m_deadZone;

    private void Awake()
    {
        m_capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        m_rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Gravity & Jump
        grounded = false;
        if (Physics.Raycast(transform.position, -transform.up, m_capsuleCollider.height / 2 + 0.1f))
        {
            grounded = true;
        }
        if (Input.GetKeyDown(KeyCode.Z) && grounded && !dashing)
        {
            velocity.y += m_jumpForce;
            grounded = false;
            jumping = true;
        }
        if (!Input.GetKey(KeyCode.Z))
        {
            jumping = false;
        }
        if (!grounded && !dashing)
        {
            if (jumping)
            {
                velocity.y = Mathf.Clamp(velocity.y - m_jumpGravity, -m_terminalVelocity, m_terminalVelocity);
            }
            else
            {
                velocity.y = Mathf.Clamp(velocity.y - m_baseGravity, -m_terminalVelocity, m_terminalVelocity);
            }
        }

        // Horizontal Movement
        if (m_deadZone < Mathf.Abs(Input.GetAxis("Horizontal")) && !dashing)
        {
            velocity.x = Mathf.Clamp(velocity.x + Mathf.Sign(Input.GetAxis("Horizontal")) * m_walkAcceleration, -m_maxWalkSpeed, m_maxWalkSpeed);
            facing = Mathf.Sign(Input.GetAxis("Horizontal"));
        }
        else if (m_deceleration <= Mathf.Abs(velocity.x))
        {
            velocity.x -= Mathf.Sign(velocity.x) * m_deceleration;
        }
        else
        {
            velocity.x = 0;
        }

        //Dash control
        if (Input.GetKeyDown(KeyCode.C))
        {
            dashing = true;
            StartCoroutine(Dash());
        }

        // Nullifying velocity when colliding with objects (stops player from sticking to walls, being able to jump around ceilings, etc.)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Mathf.Sign(velocity.x) * transform.right, out hit, m_capsuleCollider.radius + 0.1f))
        {
            velocity.x = 0;
        }
        if (Physics.Raycast(transform.position, Mathf.Sign(velocity.y) * transform.up, m_capsuleCollider.height / 2 + 0.1f))
        {
            velocity.y = 0;
        }

        // Applying velocity to Rigidbody
        m_rigidBody.velocity = velocity;

        IEnumerator Dash()
        {
            // Dash

            velocity.y = 0;
            float dashDir = facing;
            for (float i = 0; i < m_dashDuration; i += Time.deltaTime)
            {
                velocity.x += dashDir * (m_dashSpeed - i / m_dashDuration * m_dashSpeed);
                yield return new WaitForEndOfFrame();
            }
            dashing = false;
        }
    }
}