using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public int MaxHelth;
    int currentHelth;

    Animator anim;
    
    public LayerMask enemyLayer;
    

    public EnemyClass(int MaxHelth) 
    {
        this.MaxHelth = MaxHelth;
    }
    void Start()
    {
        currentHelth = MaxHelth;
        Debug.Log($"{currentHelth}");
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage) 
    {
        Debug.Log($"{name} recivid damage!");
        currentHelth -= damage;
        Debug.Log($"Health: {currentHelth}");
        anim.SetTrigger("Hit");

        if (currentHelth <= 0) 
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        anim.SetTrigger("Death");
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
