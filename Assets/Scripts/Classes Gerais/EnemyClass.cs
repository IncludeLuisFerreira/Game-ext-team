using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public int maxHelth;
    public float reculForce;
    public bool canBeDamage;
    public bool dead = false;
    public int currentHelth;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform target;
    public SpawEnemys spawner;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
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
        {   rb.velocity = new Vector2(target.localScale.x *reculForce, rb.velocity.y);
            currentHelth -= damage;
            anim.SetTrigger("Hit");
            canBeDamage = false;
            StartCoroutine(ResetCanBeDamaged());
        }
                
        if (currentHelth <= 0 && dead == false)
        {   
            Die();
            ReduceSpawCounter();
            dead = true;
            //StartCoroutine(BugPrevent());
        }
    }

    public void ReduceSpawCounter() {
        if (spawner != null && spawner.enemiesInRoom != 0) {
            spawner.enemiesInRoom--;
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
        rb.simulated = false;
    }

    void Destroy()
    {
        StopAllCoroutines();
        Destroy(gameObject, 1.5f);
    }

    IEnumerator BugPrevent()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy();
    }
}
