using System.Collections;
using UnityEngine; 

// Listas de afazeres: 
/*
    - Fazer a defesa;
    - Fazer som dos movimentos dos personagens;

*/
public class SkeletonClass : MonoBehaviour
{
    public enum SkeletonStates {Idle, Follow, Attack1, Attack2, Death, Defensive, None}
    public SkeletonStates skStates;

    [Header("Atributos base dos esqueletos")]
    [SerializeField]protected float speed;
    [SerializeField]int damage;
    [SerializeField]protected float maxDistance;
    [SerializeField]protected float minDistance;
    [SerializeField]float downTimeAttack1;
    [SerializeField]float coolDownShild;
    [SerializeField]float durationShild;
    [SerializeField]float defenseReculForce;
    [SerializeField]float delay;
    float Seconds;

    [Header("Meleee")]
    [SerializeField]float meleeDistance = 4;
    [SerializeField]float TimeDownAttack1 = 2.5f;
    [SerializeField]float SwordRange;


    [SerializeField]Transform Target;
    [SerializeField]Transform Sword;
    Animator anim;
    FollowingClass skeleton;
    EnemyClass skEnemy;
    Rigidbody2D rb;
    [SerializeField]LayerMask playerLayer;

    [SerializeField]bool canFollow;
    [SerializeField]bool facingLeft = true;
    [SerializeField]bool canFlip;
    [SerializeField]bool canMelee;
    [SerializeField]bool canDefende;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        skEnemy = GetComponent<EnemyClass>();
        skeleton = gameObject.AddComponent<FollowingClass>();
        skeleton.Init(Target, anim, speed, maxDistance, minDistance);
    }

    void Update()
    {
        /* BattleLogic(); */
        if (Input.GetKeyDown(KeyCode.F))
        {
            
        }
    }

    void BattleLogic()
    {
        FixedBoolean();
        FollowTarget();
        Shilded();
        Flip();
        Melee();
        StartCoroutine(CoolDown());
    }

    private void Shilded()
    {
        //skStates = SkeletonStates.None;

    }

    private void FollowTarget()
    {
        if (canFollow)
        {
            if (anim.GetBool("Run") == true) {skStates = SkeletonStates.Follow;}
            skeleton.Follow(transform);
        }
    }

    void FixedBoolean()
    {
        switch (skStates)
        {
            case SkeletonStates.Idle:

                break;
            case SkeletonStates.Follow:
                canFollow = true;
                canFlip = true;
                break;
            case SkeletonStates.Attack1:
                canFollow = false;
                canFlip = false;
                canMelee = false;
                break;
            case SkeletonStates.Attack2:

                break;
            case SkeletonStates.Defensive:
                canFollow = false;
                break;
            default:
                canFollow = true;
                canFlip = true;
                canMelee = true;
                break;
        }
    }

    void Flip()
    {
        if (canFlip)
        {
            if (transform.position.x > Target.position.x && facingLeft)
            {
                transform.localScale = new Vector3(-1, 1, 0);
                facingLeft = false;
            }
            if (transform.position.x < Target.position.x && !facingLeft)
            {
                transform.localScale = new Vector3(1, 1, 0);
                facingLeft = true;
            }
        }
    }

    void RandomAbility()
    {

    }

    void Melee()
    {
        if (skEnemy.DeltaDistance(Target) < meleeDistance && canMelee)
        {
            StartCoroutine(MeleeSequence());
        }
    }

    IEnumerator MeleeSequence()
    {
        anim.SetTrigger("Attack 1");
        anim.SetBool("Run", false);
        skStates = SkeletonStates.Attack1;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(TimeDownAttack1);
        skStates = SkeletonStates.Follow;
    }

    IEnumerator CoolDown()
    {
        switch (skStates)
        {
            case SkeletonStates.Attack1:
                canMelee = false;
                Seconds = 3;
                yield return new WaitForSeconds(Seconds);
                canMelee = true;
                break;
            case SkeletonStates.Defensive:
                canDefende = false;
                Seconds = 5;
                yield return new WaitForSeconds(Seconds);
                canDefende = true;
                break;
            default:
                
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(Sword.position, SwordRange);
    }

    public void MeleeHit()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(Sword.position, SwordRange, playerLayer);
       foreach (Collider2D player in hits)
       {
            PlayerClass pclass = player.GetComponent<PlayerClass>();
            if (pclass)
            {
                pclass.TakeDamage(damage, player.GetComponent<Player>().isDefending);
            }
       }
    }
    
    void Defensive()
    {
        if (skEnemy.DeltaDistance(Target) < meleeDistance && canDefende == true)
        {

        }
    }

    IEnumerator DefenseSequence()
    {
        anim.SetTrigger("Defense");
        anim.SetBool("Run", false);
        skStates = SkeletonStates.Defensive;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.5f);
    }
}