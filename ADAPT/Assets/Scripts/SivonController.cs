using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SivonController : MonoBehaviour
{
    private Animator m_sprite;
    private Rigidbody2D m_rigidBody;
    private CircleCollider2D m_clawsCollider;
    private CapsuleCollider2D m_bodyCollider;
    private Vector3 m_velocity = Vector3.zero;
    private List<int> m_dNACount = new List<int>(4);
    private float m_facing = 1;
    private float m_currentDashCooldown;
    private bool m_isDead = false;
    private bool m_canJump = false;
    private bool m_canDash = true;
    private bool m_isDashing = false;
    private bool m_isJumping = false;
    private bool m_isGrounded = false;
    private bool m_isAttacking = false;
    private bool m_canMultiJump = true;

    public enum DNATypes { Alas, Armatus, Bellum, Spiculum }

    [Header("Movement")]

    [SerializeField]
    private float m_walkAcceleration;
    [SerializeField]
    private float m_maxWalkSpeed;
    [SerializeField]
    private float m_deceleration;
    [SerializeField]
    private float m_deadZone;
    [SerializeField]
    private float m_jumpForce;
    [SerializeField]
    private float m_baseGravity;
    [SerializeField]
    private float m_jumpGravity;
    [SerializeField]
    private float m_terminalVelocity;

    [Header("Combat")]

    [SerializeField]
    private float m_baseAttackDamage;
    [SerializeField]
    private float m_attackCooldown;
    [SerializeField]
    private float m_baseHealth;

    [Header("Progression")]

    [SerializeField]
    private int m_mutationThreshold;
    [SerializeField]
    private int m_mutationCap;

    [Header("Wings")]

    [SerializeField]
    private float m_jumpHeightModifier;

    [Header("Armor")]

    [SerializeField]
    private float m_healthModifier;

    [Header("Claws")]

    [SerializeField]
    private float m_damageModifier;

    [Header("Spikes")]

    [SerializeField]
    private float m_dashSpeed;
    [SerializeField]
    private float m_dashDuration;
    [SerializeField]
    private float m_dashCooldown;
    [SerializeField]
    private float m_dashCooldownModifier;

    private void Awake()
    {
        m_sprite = GetComponentInChildren<Animator>();
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_clawsCollider = GetComponentInChildren<CircleCollider2D>();
        m_bodyCollider = GetComponent<CapsuleCollider2D>();
        for (int i = 0; i < m_dNACount.Count; i++)
        {
            m_dNACount[i] = 0;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    void Update()
    {
        Movement();
        UpdateAnimator();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    /*
       _____                   _                    _                      _____    _                     _              
      / ____|                 | |                  | |           ___      |  __ \  | |                   (_)             
     | |        ___    _ __   | |_   _ __    ___   | |  ___     ( _ )     | |__) | | |__    _   _   ___   _    ___   ___ 
     | |       / _ \  | '_ \  | __| | '__|  / _ \  | | / __|    / _ \/\   |  ___/  | '_ \  | | | | / __| | |  / __| / __|
     | |____  | (_) | | | | | | |_  | |    | (_) | | | \__ \   | (_>  <   | |      | | | | | |_| | \__ \ | | | (__  \__ \
      \_____|  \___/  |_| |_|  \__| |_|     \___/  |_| |___/    \___/\/   |_|      |_| |_|  \__, | |___/ |_|  \___| |___/
    -----------------------------------------------------------------------------------------__/ |------------------------
                                                                                            |___/                        
    */

    private void Movement()
    {
        // Dash control
        if (Input.GetKeyDown(KeyCode.C) && m_canDash)
        {
            StartCoroutine(Dash());
        }

        if (!m_isDashing)
        {
            if (Physics2D.Raycast(transform.position - transform.up * m_bodyCollider.size.y / 2, -transform.up, m_jumpGravity * Time.fixedDeltaTime))
            {
                m_isGrounded = true;
                m_isJumping = false;
                m_velocity.y = 0;
            }
            else
            {
                m_isGrounded = false;
            }

            // Handles Input for Horizontal Movement & Jump

            if (Input.GetKeyDown(KeyCode.Z) && m_isGrounded)
            {
                m_velocity.y = m_jumpForce;
                m_isGrounded = false;
                m_isJumping = true;
            }
            if (Input.GetKeyUp(KeyCode.Z))
            {
                m_isJumping = false;
            }
            if (!m_isDashing && !m_isGrounded)
            {
                if (m_isJumping)
                {
                    m_velocity.y -= m_jumpGravity;
                }
                else
                {
                    m_velocity.y -= m_baseGravity;
                }
            }
            
            if (m_deadZone < Mathf.Abs(Input.GetAxis("Horizontal")))
            {
                m_velocity.x = Mathf.Clamp(m_velocity.x + Mathf.Sign(Input.GetAxis("Horizontal")) * m_walkAcceleration, -m_maxWalkSpeed, m_maxWalkSpeed);
            }
            else if (m_deceleration < Mathf.Abs(m_velocity.x))
            {
                m_velocity.x -= Mathf.Sign(m_velocity.x) * m_deceleration;
            }
            else
            {
                m_velocity.x = 0;
            }
            
            if(m_deadZone < Mathf.Abs(m_velocity.x))
            {
                m_facing = Mathf.Sign(m_velocity.x);
            }

            if (Mathf.Sign(transform.localScale.x) == m_facing)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void ApplyMovement()
    {
        m_rigidBody.velocity = new Vector3(Mathf.Clamp(m_velocity.x, -m_terminalVelocity, m_terminalVelocity), Mathf.Clamp(m_velocity.y, -m_terminalVelocity, m_terminalVelocity), 0);
    }

    private void UpdateAnimator()
    {
        // Sends values to animation controller
        m_sprite.SetFloat("verticalVelocity", m_velocity.y);
        m_sprite.SetBool("isAttacking", m_isAttacking);
        m_sprite.SetBool("isGrounded", m_isGrounded);
        m_sprite.SetBool("isDashing", m_isDashing);
        m_sprite.SetBool("isJumping", m_isJumping);
        m_sprite.SetBool("isDead", m_isDead);
        if (m_deadZone < Mathf.Abs(m_velocity.x))
        {
            m_sprite.SetBool("isMoving", true);
        }
        else
        {
            m_sprite.SetBool("isMoving", false);
        }
    }

    IEnumerator Dash()
    {
        // Halts all other movement and dashes the player forward at a set speed for a set duration
        m_isDashing = true;
        m_velocity.y = 0;
        float dashDir = m_facing;
        for (float i = 0; i < m_dashDuration; i += Time.deltaTime)
        {
            m_velocity.x += dashDir * (m_dashSpeed - i / m_dashDuration * m_dashSpeed);
            yield return new WaitForEndOfFrame();
        }
        m_isDashing = false;
    }

    /*
      _____                                                     _                 
     |  __ \                                                   (_)                
     | |__) |  _ __    ___     __ _   _ __    ___   ___   ___   _    ___    _ __  
     |  ___/  | '__|  / _ \   / _` | | '__|  / _ \ / __| / __| | |  / _ \  | '_ \ 
     | |      | |    | (_) | | (_| | | |    |  __/ \__ \ \__ \ | | | (_) | | | | |
     |_|      |_|     \___/   \__, | |_|     \___| |___/ |___/ |_|  \___/  |_| |_|
    ---------------------------__/ |-----------------------------------------------
                              |___/                                               
     */

    public void ConsumeDNA(DNATypes dNAType)
    {
        switch (dNAType)
        {
            case DNATypes.Alas:
                m_dNACount[0]++;
                if (m_mutationThreshold <= m_dNACount[0])
                {
                    m_canMultiJump = true;
                }
                break;
            case DNATypes.Armatus:
                m_dNACount[1]++;
                if (m_mutationThreshold <= m_dNACount[1])
                {
                    
                }
                break;
            case DNATypes.Bellum:
                m_dNACount[2]++;
                if (m_mutationThreshold <= m_dNACount[2])
                {
                    
                }
                break;
            case DNATypes.Spiculum:
                m_dNACount[3]++;
                if (m_mutationThreshold <= m_dNACount[3])
                {
                    m_canDash = true;
                }
                break;
            default: break;
        }
    }
}
