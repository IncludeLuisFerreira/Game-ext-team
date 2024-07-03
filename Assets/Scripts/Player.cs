using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    public enum states {Attacking, Hurt, Running, Jumping}
    [Header("Atributos do player")]
    [SerializeField]float vida = 10;
    [SerializeField]float estamina = 5;
    

    [SerializeField]float speed = 5f;
    [SerializeField]float rollForce = 6f;
    [SerializeField]float jumpForce = 5f;
    bool isRight = true;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform groundCheck;
    public bool grounded;
    Vector3 back;

    Rigidbody2D rb;
    Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {    
        back = transform.position;
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);
        anim.SetBool("Grounded", grounded == true);
        Move();
        Jump();
        Attack();
    }

    void FixedUpdate()
    {
        
    }

    void Move()
    {
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

    void Attack()
    {
        if (grounded && Input.GetButtonDown("Attack"))
        {
            rb.velocity = new Vector2(0,0);
            anim.SetTrigger("Attack");
        }
    } 

}