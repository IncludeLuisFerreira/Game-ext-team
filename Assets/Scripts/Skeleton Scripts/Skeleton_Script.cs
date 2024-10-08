using System.Collections;
using UnityEngine; 

// Listas de afazeres: 
/*
    - Fazer a defesa;
    - Talvez dificultar fazendo com que as vezes o clip hit não seja tocado, assim continuando o ataque;
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
    float Seconds;

    [Header("Meleee")]
    [SerializeField]float meleeDistance = 4;
    [SerializeField]float TimeDownAttack1 = 2.5f;
    [SerializeField]float SwordRange;


    [SerializeField]Transform Sword;
    private Transform Target;
    private Animator anim;
    private FollowingClass skeleton;
    private EnemyClass skEnemy;
    private Rigidbody2D rb;
    [SerializeField]LayerMask playerLayer;

    [SerializeField]bool canFollow;
    [SerializeField]bool facingLeft = true;
    [SerializeField]bool canFlip;
    [SerializeField]bool canMelee;
    [SerializeField]bool canDefende;

    void Start()
    {
        Target = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        skEnemy = GetComponent<EnemyClass>();
        skeleton = gameObject.AddComponent<FollowingClass>();
        skeleton.Init(Target, anim, speed, maxDistance);
    }

    void Update()
    {
        if (TryGetComponent<EnemyClass>(out var Control))
        {
            if (Control.dead) {return ;}
        }
        BattleLogic(); 
    }

// Necessário melhor estruturação futura
    void BattleLogic()
    {
        FixedBoolean();
        FollowTarget();
        Flip();
        RandomAbility();
        StartCoroutine(CoolDown());
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
                canFlip = false;
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

    void RandomAbility()
    {
        float randomNum = Random.Range(0, 1);

        if (randomNum < 0.7f)
            Melee();
        else
            Defensive();
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
            StartCoroutine(DefenseSequence());
        }
    }

    IEnumerator DefenseSequence()
    {
        anim.SetBool("Defense", true);
        anim.SetBool("Run", false);
        skStates = SkeletonStates.Defensive;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("Defense", false);
        skStates = SkeletonStates.None;
    }
}