using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public int MaxHelth;
    public int currentHelth;
    public bool canBeDamage;

    public Animator anim;
    public Rigidbody2D rb;
    public LayerMask enemyLayer;

    public EnemyClass(int MaxHelth) 
    {
        this.MaxHelth = MaxHelth;
    }

    protected void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }   
    public void Start()
    {
        currentHelth = MaxHelth;
    }

    protected float DeltaDistance(Transform Target)
    {
        return Vector3.Distance(transform.position, Target.position);
    }

    public void TakeDamage(int damage, float localScale, bool canBeDamage) 
    {
        if (currentHelth > 0 && canBeDamage)
        {    rb.velocity = new Vector2(localScale * 2.5f, rb.velocity.y);
            currentHelth -= damage;
            anim.SetTrigger("Hit");
        }
        else if (!canBeDamage)
        {
            Debug.Log("The enemy escape from your attack!");
        }
        else if (currentHelth < 0)
        {   
            Die();
        }
    }

    void Die()
    {
        anim.SetTrigger("Death");
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
