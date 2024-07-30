using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinScript : MonoBehaviour
{
    EnemyClass GoblinEnemy;
    FollowingClass Goblim;
    Animator anim;
    
    public enum GoblimStates {Idle, Attack1, Attack2, Run, None}

    public GoblimStates state;
    [SerializeField]float speed;
    [SerializeField]float maxDistance;
    [SerializeField]int damage;
    [SerializeField]bool facingLeft = true;
    [SerializeField]bool canAttack1;
    [SerializeField]bool canAttack2;
    [SerializeField]bool canFollow;
    [SerializeField]private float DownTimeAttack1;
    [SerializeField]private float DodgeForce;
    [SerializeField]private float DownTimeAttack2;
    [SerializeField]private float coolDownAttack1;
    [SerializeField]private float coolDownAttack2;

    [SerializeField]Transform Target;
    Rigidbody2D rb;
    private Collider2D cold;
    [SerializeField]private bool canSort = true;
    private int countAttack1 = 0, countAttack2 = 0;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"));
        
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cold = GetComponent<Collider2D>();
        GoblinEnemy = GetComponent<EnemyClass>();
        Goblim = gameObject.AddComponent<FollowingClass>();

        Goblim.Init(Target, anim, speed, maxDistance);
    }

    void Update()
    { 
        if (TryGetComponent<EnemyClass>(out var Control))
        {
            if (Control.dead) {return ;}
        }
       
        BattleLogic();
        FixedBoolean(); 
    }

    void BattleLogic()
    {
        if (canAttack1 && canAttack2 && canSort)
        {
            float probability = Random.Range(0f,1f);
            
            // attack1: 70%
            // attack2: 30%

            if (probability > 0.3f) {canAttack2 = false; countAttack1++;}
            else {canAttack1 = false; countAttack2++;}

            canSort = false;
        }

        // Seguir o player quando entrar no range: a função tem limitações para que preserva a lógica de combate
        FollowTarget();
        Attack1();
        Attack2();
    }

    void FollowTarget()
    {   
        Flip();
        if (canFollow && (state == GoblimStates.None || state == GoblimStates.Idle || state == GoblimStates.Run))
        {
            Goblim.Follow(transform);
            if (anim.GetBool("Run") == true) {state = GoblimStates.Run;}
        }
    }

    void FixedBoolean()
    {
        switch (state)
        {
            case GoblimStates.Idle:
            case GoblimStates.Run:
            case GoblimStates.None:
                canFollow = true;
                break;
            case GoblimStates.Attack1:
            case GoblimStates.Attack2:
                canFollow = false;
                break;
        }
    }

    void Flip() 
    {
        if (canFollow && GoblinEnemy.DeltaDistance(Target) < 10f)
        {
            if (transform.position.x > Target.position.x && !facingLeft)
            {
                transform.localScale = new Vector3(-1, 1, 0);
                facingLeft = true;
            }
            if (transform.position.x < Target.position.x && facingLeft)
            {
                transform.localScale = new Vector3(1, 1, 0);
                facingLeft = false;
            }
        }
    }
    void Attack1()
    {
        if (GoblinEnemy.DeltaDistance(Target) < 1.75f && canAttack1 && state == GoblimStates.Run) 
        {
            canAttack1 = false;
            StartCoroutine(Attack1Sequence());
        }
    }

    IEnumerator Attack1Sequence()
    {
        state = GoblimStates.Attack1;
        anim.SetBool("Run", false);
        anim.SetTrigger("Attack 1");
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(DownTimeAttack1);
        state = GoblimStates.None;
        yield return new WaitForSeconds(coolDownAttack1);
        canSort = true;
        canAttack1 = true;
    }
    

    void Attack2()
    {
        if (GoblinEnemy.DeltaDistance(Target) < 2f && canAttack2)
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(Attack2Sequence());
        }
    }

    IEnumerator Attack2Sequence()
    {
        state = GoblimStates.Attack2;
        canAttack2 = false;
        anim.SetBool("Run", false);
        rb.velocity = new Vector2(-DodgeForce * transform.localScale.x, rb.velocity.y);
        anim.SetTrigger("Attack 2");
        yield return new WaitForSeconds(DownTimeAttack2);
        state = GoblimStates.None;
        yield return new WaitForSeconds(coolDownAttack2);
        canSort = true;
        canAttack2 = true;
    }

    void DodgeIn()
    {
        rb.velocity = new Vector2(DodgeForce * transform.localScale.x, rb.velocity.y);
    }
    
    void Stop()
    {
        rb.velocity = Vector2.zero;
    }

    void Damage()
    {
        Ofensive_Goblim attacks = GetComponent<Ofensive_Goblim>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(attacks.attackPoint.position, attacks.attackRange);
        foreach (var player in hits)
        {
            if (player.TryGetComponent<PlayerClass>(out var playerComponent))
            {
                playerComponent.TakeDamage(damage, player.GetComponent<Player>().isDefending);
            }
        }

    }

    void Damage2()
    {
        Ofensive_Goblim attacks = GetComponent<Ofensive_Goblim>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(attacks.attackPoint2.position, attacks.attackRange2);
        foreach (var player in hits)
        {
            if (player.TryGetComponent<PlayerClass>(out var playerComponent))
            {
                playerComponent.TakeDamage(damage, player.GetComponent<Player>().isDefending);
            }
        }
        cold.enabled = true;
    }
}
