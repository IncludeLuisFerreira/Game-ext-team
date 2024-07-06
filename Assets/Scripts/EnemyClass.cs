using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public int MaxHelth;
    public bool canBeDamage = true;
    int currentHelth;

    Animator anim;
    Rigidbody2D rb;
    public LayerMask enemyLayer;
    

    public EnemyClass(int MaxHelth) 
    {
        this.MaxHelth = MaxHelth;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }   
    void Start()
    {
        currentHelth = MaxHelth;
    }


    public void TakeDamage(int damage, float localScale, bool canBeDamage) 
    {
        if (canBeDamage)
        {    rb.velocity = new Vector2(localScale * 2.5f, rb.velocity.y);
            currentHelth -= damage;
            anim.SetTrigger("Hit");
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
