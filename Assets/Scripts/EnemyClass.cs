using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public int maxHelth;
    public float reculForce;
    public bool canBeDamage;
    int currentHelth;

    public Animator anim;
    public Rigidbody2D rb;

    public EnemyClass(int MaxHelth) 
    {
        this.maxHelth = MaxHelth;
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }   
    public void Start()
    {
        currentHelth = maxHelth;
    }

    public float DeltaDistance(Transform Target)
    {
        return Vector3.Distance(transform.position, Target.position);
    }

    public void TakeDamage(int damage) 
    {
        if (currentHelth > 0 && canBeDamage)
        {    rb.velocity = new Vector2(transform.localScale.x *reculForce, rb.velocity.y);
            currentHelth -= damage;
            anim.SetTrigger("Hit");
        }
        else if (!canBeDamage) 
        {
            Debug.Log("The enemy escape from your attack!");
        }
        
        if (currentHelth <= 0)
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
