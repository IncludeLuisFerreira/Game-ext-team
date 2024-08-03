using System;
using System.Collections;
using UnityEngine;

// Personagem de patrulha 
public class Patrol : MonoBehaviour
{

    public float speed;
    public int walkPointRange;
    public float sightRange;
    public float coolDownAttack;
    public int damage;
    public bool negativePoint = true, positivePoint = false;
    public bool facingLeft;
    public Transform player;
    public LayerMask groundLayer, playerLayer;
    public Animator anim;
    public Transform positiveTransform;
    public Transform negativeTransform;
    public Transform attackPoint;
    public float attackPointRange;

    private bool walkPointSet;
    private FollowingClass PatrolingMushuroom;
    private EnemyClass patrolEnemy;
    private bool alreadyAttacked;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        PatrolingMushuroom = GetComponent<FollowingClass>();
        patrolEnemy = GetComponent<EnemyClass>();
    }

    private void Start()
    {
        PatrolingMushuroom.Init(player, anim, speed,walkPointRange);
    }
    void Update()
    {
        if (patrolEnemy.dead) {return ;}


        bool playerInSightRange = Physics2D.OverlapCircle(transform.position, sightRange, playerLayer);
        bool playerInAttackRange = Physics2D.OverlapCircle(attackPoint.position, attackPointRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            // patrulhar
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
           
            // seguir o player
            ChasePlayer();
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            //Atacar o player
            AttackPlayer();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            // definir a distância do patrulhamento
            DefineWalkPoint();
        }

        if (walkPointSet)
        {
            Vector3 direction = Vector3.zero;
            
            
            if (positivePoint)
            {
                direction = (positiveTransform.position- transform.position).normalized;
                if (Convert.ToInt16(transform.position.x) == Convert.ToInt16(positiveTransform.position.x)) 
                {
                    facingLeft = false;
                    positivePoint = false; 
                    negativePoint = true;
                }
            }

            if (negativePoint)
            {
                direction =  (negativeTransform.position - transform.position).normalized;
                if (Convert.ToInt16(transform.position.x) == Convert.ToInt16(negativeTransform.position.x)) 
                {
                    facingLeft = true;
                    positivePoint = true; 
                    negativePoint = false;
                }

            }
            transform.localScale = new Vector3(1 * direction.x, transform.localScale.y);
            transform.position += speed * Time.deltaTime * direction;
        }
    }

    private void DefineWalkPoint()
    {
        float randowX = UnityEngine.Random.Range(0f, walkPointRange);
        positiveTransform.position = new Vector3(positiveTransform.position.x + randowX, positiveTransform.position.y, 0);
        negativeTransform.position = new Vector3(negativeTransform.position.x - randowX, negativeTransform.position.y, 0);
        
        
        walkPointSet = true;
    }

    private void ChasePlayer()
    {
        PatrolingMushuroom.Follow(transform);
        Flip();
    }

    private void Flip()
    {
        if (transform.localPosition.x > player.position.x && facingLeft)
        {
            transform.localScale = new Vector3(-1, 1, 0);
            facingLeft = false;
        }

        if (transform.localPosition.x < player.position.x && !facingLeft)
        {
            transform.localScale = new Vector3(1, 1, 0);
            facingLeft = true;
        }
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            Debug.Log("Ataque!");
            alreadyAttacked = true;
            anim.SetTrigger("Attack");
            StartCoroutine(ResetAttack());
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(coolDownAttack);
        alreadyAttacked = false;
    }

    public void DoDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackPointRange);

        foreach (var player in hits)
        {
            if (TryGetComponent<PlayerClass>(out var playerComponent))
            {
                playerComponent.TakeDamage(damage, player.GetComponent<Player>().isDefending);
            }
        }
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackPointRange);
    }

}