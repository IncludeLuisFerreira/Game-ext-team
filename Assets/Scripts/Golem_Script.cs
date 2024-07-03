using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;

public class Golem_Script : MonoBehaviour
{
    [SerializeField]float vida = 15f;
    [SerializeField]Transform Target;
    [SerializeField]float MinDistance = 10f;
    Rigidbody2D rb;
    Animator anim;
    bool facingLeft = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
    }

    void Update()
    {
        Flip();
        EnemyAttack();        
        Follow();
    }

    float TargetDistance()
    {
        return Mathf.Abs(Target.position.x - transform.position.x);
    }

    void Flip()
    {
        if (Target.position.x > transform.position.x && facingLeft)
        {
            transform.localScale = new Vector3(1, 1, 0);
            facingLeft = false;
        }

        if (Target.position.x < transform.position.x && !facingLeft)
        {
            transform.localScale = new Vector3(-1, 1, 0);
            facingLeft = true;
        }
    }

    void EnemyAttack()
    {
        Attack1();
        Attack3();
    }

    void Attack1()
    {
        if (TargetDistance() < 5)
        {
            anim.SetTrigger("Attack");
        }
    }

    void Follow()
    {
        if (TargetDistance() < MinDistance)
        {
            rb.velocity = Vector2.MoveTowards(transform.position, Target.position, MinDistance - 1f);
        }
    }

    void Attack3()
    {
        if (TargetDistance() < 3)
        {
            anim.SetTrigger("Attack3");
        }
    }
}
