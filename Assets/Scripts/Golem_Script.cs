using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Golem_Script : MonoBehaviour
{

    public enum golemState {Dash, Attacking_1, None}
    [SerializeField]golemState state = golemState.None;

    [Header("Atributos")]
    [SerializeField]int MaxHelth = 15;
    [SerializeField]float speed = 2f;
    [SerializeField]float decendingSpeed = 1.5f;
    [SerializeField]float MinDistance = 10f;
    [SerializeField]float MaxDistance = 15f;
    [SerializeField]float groundYPosition;
    [SerializeField]float originalYPosition;
    [SerializeField]float duration_Earthquake = 2.0f;
    [SerializeField]int countEarthquake = 2;
    int count = 0;


    [Header("Variáveis booleanas")]
    [SerializeField]bool facingLeft = true;
    [SerializeField]bool isDecending = false;
    [SerializeField]bool isAcending = false;
    [SerializeField]bool canDash = true;

    [Header("Transforms")]
    [SerializeField]Transform Target;

    private FollowingClass follow;
    private EnemyClass Golem;
    Rigidbody2D rb;
    Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Golem = GetComponent<EnemyClass>();
        
    }
    void Start()
    {
        follow = new(Target, anim, speed, MaxDistance, MinDistance);
        Golem = new(MaxHelth);
    }
 
    void Update()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, Golem.enemyLayer, true);
        if (state == golemState.Attacking_1)
        {
            anim.SetBool("Run", false);
            return;
        }
        Flip();
        EnemyAttack();
        //CounterDash();
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
        if (TargetDistance() < 3f && !isDecending)
        {
            StartDecending(); 
        }

        if (isDecending)
        {
            Descending();
        }
        
        if (!isDecending && transform.position.y -1.35 <= groundYPosition && TargetDistance() <= 3f)
        {
            if (count < countEarthquake) {
                StartCoroutine(Earthquake());
                count++;
            }

            // Terá mais ataques para preencher a exceção
        }

        if (state == golemState.Attacking_1) {
            return ;
        }

        if (transform.position.y - 1.35 <= groundYPosition && TargetDistance() > 3f)
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
        if (TargetDistance() < 3)
        {
            anim.SetTrigger("Attack3");
        }
    }

    void CounterDash()
    {
        if (canDash && TargetDistance() < 3f) {
            StartCoroutine(CounterDashEvent());
        }
    }


    /************************ Ataque 1************************/

    IEnumerator Earthquake()
    {
        state = golemState.Attacking_1;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Attack");
        yield return  new WaitForSeconds(duration_Earthquake);
        state = golemState.None;
    }

    /**********************************************************/

    IEnumerator CounterDashEvent()
    {
        yield return new WaitForSeconds(0);
/*         state = golemState.Dash;
        Physics2D.IgnoreLayerCollision(gameObject.layer, Golem.playerLayer, true);
        anim.SetTrigger("Dash");
        canDash = false;
        yield return new WaitForSeconds(1.2f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, Golem.playerLayer, false);
        state = golemState.None;
        yield return new WaitForSeconds(5);
        canDash = true ; 
 */    }

    /************************ Dano do player ************************/

   
}
