using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public int maxHelth;
    public float reculForce;
    public bool canBeDamage;
    public bool dead = false;
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
    private void Start()
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
        {   rb.velocity = new Vector2(-transform.localScale.x *reculForce, rb.velocity.y);
            currentHelth -= damage;
            anim.SetTrigger("Hit");
            canBeDamage = false;
            StartCoroutine(ResetCanBeDamaged());
        }
        else if (!canBeDamage) 
        {
            Debug.Log("Enemy can not be damaged!");
        }
        
        if (currentHelth <= 0 && dead == false)
        {   
            Die();
            dead = true;
            StartCoroutine(BugPrevent());
        }
    }

    IEnumerator ResetCanBeDamaged()
    {
        yield return new WaitForSeconds(0.5f);
        canBeDamage = true;
    }

    void Die()
    {
        anim.SetTrigger("Death");
    }

    void Destroy()
    {
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 1.5f);
    }

    IEnumerator BugPrevent()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy();
    }
}
