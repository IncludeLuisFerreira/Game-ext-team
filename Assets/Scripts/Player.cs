using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    public enum States {Attacking, Hurt, Running, Jumping, Rolling, Idle}
    States playerStates;
    [Header("Atributos do player")]
    [SerializeField]float MaxHelth = 50;
    [SerializeField]float rollTime = 1f;
    [SerializeField]float rollForce = 6f;
    [SerializeField]float rollCoolDown = 3f;
    [SerializeField]float speed = 5f;
    [SerializeField]float jumpForce = 5f;
    [SerializeField]float attackRange = 0.5f;
    [SerializeField]int attackDamage = 5;

    [Header("LayerMasks usadads")]
    [SerializeField]LayerMask whatIsGround;
    [SerializeField]Transform groundCheck;
    [SerializeField]Transform attackPoint;
    [SerializeField]LayerMask enemyLayer;

    [Header("Variáveis booleanas")]
    public bool grounded;
    public bool isRight = true;
    public bool canRoll;

    Rigidbody2D rb;
    Animator anim;
    void Start()
    {
        canRoll = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {    
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);
        anim.SetBool("Grounded", grounded == true);
        if (playerStates == States.Rolling) {
            return;
        }
        Move();
        Jump();
        Attack();
        Roll();
    }

    /****************************** Movimentos do Player ******************************/
    void Move()
    {
        if (playerStates == States.Attacking) {
            return;
        }
        float axis = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(5 * axis, rb.velocity.y); 
        anim.SetFloat("Velocity.x", Mathf.Abs(rb.velocity.x));
        Flip(axis);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
        anim.SetFloat("Velocity.y", rb.velocity.y);
    }

    void Flip(float axis)
    {
        if (axis > 0 && !isRight)
        {
            transform.localScale = new Vector3(1, transform.localScale.y);
            isRight = true; 
        }

        if (axis < 0 && isRight)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y);
            isRight = false;
        }
    }

    void Roll()
    {
        if (playerStates == States.Attacking) {
            return;
        }

        if (grounded && Input.GetKeyDown(KeyCode.C) && canRoll)
        {
            
            StartCoroutine(RollTime());
            StopCoroutine(AttackTime());
            
        }
    }

    IEnumerator RollTime()
    {
        anim.SetBool("Roll", true);
        anim.SetFloat("Velocity.x", 0);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        canRoll = false;
        playerStates = States.Rolling;
        if (isRight) {
            rb.velocity = new Vector2(rollForce, rb.velocity.y); 
        }
        else {
            rb.velocity = new Vector2(-rollForce, rb.velocity.y);
        }
        yield return new WaitForSeconds(rollTime);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
        playerStates = States.Idle;
        anim.SetBool("Roll", false);
        anim.SetFloat("Velocity.x", rb.velocity.x);
        yield return new WaitForSeconds(rollCoolDown);
        canRoll = true;
    }

    /*******************************************************************************/

    void Attack()
    {
        if (grounded && Input.GetButtonDown("Attack"))
        {
            Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach(Collider2D enemy in hitEnemy) {
                EnemyClass teste = enemy.GetComponent<EnemyClass>();

                if (teste) {
                    teste.TakeDamage(attackDamage);
                }
            }
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Attack");
            StartCoroutine(AttackTime());
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }

    IEnumerator AttackTime()
    {
        playerStates = States.Attacking;
        yield return new WaitForSeconds(0.5f);
        playerStates = States.Idle;
    }
}