using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Golem_Script : MonoBehaviour
{

    public enum golemState {Dash, Attacking_1, None}
    [SerializeField]golemState state = golemState.None;

    [Header("Atributos")]
    [SerializeField]int MaxHelth = 15;
    [SerializeField]float speed = 2f;
    [SerializeField]int damage = 5;
    [SerializeField]float decendingSpeed = 1.5f;
    [SerializeField]float MinDistance = 10f;
    [SerializeField]float MaxDistance = 15f;
    [SerializeField]float groundYPosition;
    [SerializeField]float originalYPosition;
    [SerializeField]float duration_Earthquake = 2.0f;
    [SerializeField]float coolDownEarthquake = 3.25f;
    [SerializeField]float dashForce = 10f;
    [SerializeField]float dashTime = 0.5f;
    [SerializeField]float dashCoolDown = 2.5f;
    [SerializeField]int countEarthquake = 2;
    [SerializeField]float fastHitRange;
    int count = 0;


    [Header("Variáveis booleanas")]
    [SerializeField]bool facingLeft = true;
    [SerializeField]bool isDecending = false;
    [SerializeField]bool isAcending = false;
    public bool canDash = true;
    public bool canEarthquake = true;

    [Header("Transforms")]
    [SerializeField]Transform Target;
    [SerializeField]LayerMask playerLayer;
    [SerializeField]Transform attackPoint;

    private FollowingClass follow;
    private EnemyClass Golem;
    Rigidbody2D rb;
    Animator anim;
    Collider2D col2d;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Golem = GetComponent<EnemyClass>();
        col2d = GetComponent<Collider2D>();
    }
    void Start()
    {
        follow = new(Target, anim, speed, MaxDistance, MinDistance);
        Golem = new(MaxHelth);        
    }
 
    void Update()
    {
        Golem.canBeDamage = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, Golem.enemyLayer, true);
        if (state == golemState.Attacking_1 || state == golemState.Dash)
        {
            anim.SetBool("Run", false);
            return;
        }
        Flip();
        EnemyAttack();
        CounterDash();
        follow.Follow(transform);
    }

    float TargetDistance()
    {
        return Vector3.Distance(transform.position, Target.position);
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

    void Descending()
    {
        float newYPosition = Mathf.MoveTowards(transform.position.y, groundYPosition, decendingSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newYPosition, 0);

        if (transform.position.y - 1.35f <= groundYPosition)
        {
            isDecending = false;
        }
    }

    void Acending()
    {
        float newYPosition = Mathf.MoveTowards(transform.position.y,originalYPosition, decendingSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);

        if (transform.position.y == originalYPosition)
        {
            isAcending = false;
        }
        count = 0;
    }

    void StartDecending()
    {
        isDecending = true;
    }
    

    void StartAcending()
    {
        isAcending = true;
    }

    void Attack1()
    {
        if (TargetDistance() < 5f && !isDecending)
        {
            StartDecending(); 
        }

        if (isDecending)
        {
            Descending();
        }
        
        if (!isDecending && transform.position.y -1.35 <= groundYPosition && TargetDistance() <= 5f && canEarthquake && !canDash)
        {
            if (count < countEarthquake) {
                StartCoroutine(Earthquake());
                //count++;
            }
        } 

        if (state == golemState.Attacking_1) {
            return ;
        }

        if (transform.position.y - 1.35 <= groundYPosition && TargetDistance() > 5f)
        {
           StartAcending();
        }

        if (isAcending)
        {
            Acending();
        }
    }

    void Attack3()
    {
        if (TargetDistance() < 5 && canEarthquake)
        {
            anim.SetTrigger("Attack3");
        }
    }

    void CounterDash()
    {
        if (canDash && TargetDistance() < 2f && Input.GetButtonDown("Attack")) {
            StopCoroutine(Earthquake());
            StartCoroutine(CounterDashEvent());
        }
    }


    /************************ Ataque 1************************/

    IEnumerator Earthquake()
    {
        canEarthquake = false;
        state = golemState.Attacking_1;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Attack");
        col2d.enabled = false;
        yield return  new WaitForSeconds(duration_Earthquake);
        col2d.enabled = true;
        state = golemState.None;
        yield return new WaitForSeconds(coolDownEarthquake);
        canEarthquake = true;
    }

    /**********************************************************/

    IEnumerator CounterDashEvent()
    {
        state = golemState.Dash;
        canDash = false;

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), true);
        col2d.enabled = false;
        if (facingLeft)
        {
            rb.velocity = new Vector2(-dashForce, rb.velocity.y);
            transform.localScale = new Vector3(1, 1, 0);
        }
        else
        {
            rb.velocity = new Vector2(dashForce, rb.velocity.y);
            transform.localScale = new Vector3(-1, 1, 0);

        }
        
        anim.SetTrigger("Dash");
        yield return new WaitForSeconds(dashTime);
        Physics2D.IgnoreLayerCollision(gameObject.layer,LayerMask.NameToLayer("Player"), false);
        col2d.enabled = true;
        rb.velocity = Vector2.zero;
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, fastHitRange, LayerMask.NameToLayer("Player"));
        foreach (Collider2D player in hit)
        {
            player.GetComponent<PlayerClass>().TakeDamage(damage);
        }
        yield return new WaitForSeconds(1);
        state = golemState.None;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true ; 
     }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, fastHitRange);
    }

    /************************ Dano do player ************************/

   
}
