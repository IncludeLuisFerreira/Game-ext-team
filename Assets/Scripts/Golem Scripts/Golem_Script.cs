using System.Collections;
using UnityEngine;

public class Golem_Script : MonoBehaviour
{
    public enum GolemState { Dash, Attacking_1, None }
    [SerializeField] GolemState state = GolemState.None;

    [Header("Atributos")]
    [SerializeField] float speed = 2f;
    [SerializeField] int damage = 5;
    [SerializeField] float descendingSpeed = 1.5f;
    [SerializeField] float minDistance = 10f;
    [SerializeField] float maxDistance = 15f;
    [SerializeField] float groundYPosition;
    [SerializeField] float originalYPosition;
    [SerializeField] float durationEarthquake = 2.0f;
    [SerializeField] float cooldownEarthquake = 3.25f;
    [SerializeField] float dashForce = 10f;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float dashCooldown = 2.5f;
    [SerializeField] int countEarthquake = 2;
    [SerializeField] float fastHitRange;
    [SerializeField] float restTime = 1f;
    int count = 0;

    [Header("Variáveis booleanas")]
    [SerializeField] bool facingLeft = true;
    [SerializeField] bool isDescending = false;
    [SerializeField] bool isAscending = false;
    public bool canDash = true;
    public bool canEarthquake = true;
    public bool canExplode = true;
    public bool grounded;

    [Header("Transforms")]
    [SerializeField] Transform target;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform ice;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask whatIsGround;

    private FollowingClass follow;
    private EnemyClass golem;
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col2d;
    private GameObject Player;
    private Ofensive_Golem pointers;
    Vector3 origin;


    private int playerLayerIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        golem = GetComponent<EnemyClass>();
        col2d = GetComponent<Collider2D>();
        pointers = GetComponent<Ofensive_Golem>();
        

        playerLayerIndex = LayerMask.NameToLayer("Player");
        if (playerLayerIndex == -1)
        {
            Debug.LogError("Layer 'Player' not found. Please ensure you have a layer named 'Player'.");
        }
    }

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        follow = gameObject.AddComponent<FollowingClass>();
        follow.Init(target, anim, speed, maxDistance, minDistance);
    }

    void Update()
    {
        grounded = Grounded();
        anim.SetBool("Grounded", grounded);
        if (playerLayerIndex != -1)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, playerLayerIndex, true);
        }

        if (state == GolemState.Attacking_1 || state == GolemState.Dash)
        {
            anim.SetBool("Run", false);
            return;
        }
        golem.canBeDamage = true;
        /* Flip();
        EnemyAttack();
        CounterDash();
        follow.Follow(transform); */ 

        // Teste de lógica
        if (Input.GetKeyDown(KeyCode.P))
            anim.SetTrigger("Attack");
    }


    float TargetDistance()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    void Flip()
    {
        if (target.position.x > transform.position.x && facingLeft)
        {
            transform.localScale = new Vector3(1, 1, 0);
            facingLeft = false;
        }

        if (target.position.x < transform.position.x && !facingLeft)
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
        float newYPosition = Mathf.MoveTowards(transform.position.y, groundYPosition, descendingSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newYPosition, 0);

        if (transform.position.y - 1.35f <= groundYPosition)
        {
            isDescending = false;
        }
    }

    void Ascending()
    {
        float newYPosition = Mathf.MoveTowards(transform.position.y, originalYPosition, descendingSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);

        if (transform.position.y == originalYPosition)
        {
            isAscending = false;
        }
        count = 0;
    }

    void StartDescending()
    {
        isDescending = true;
    }

    void StartAscending()
    {
        isAscending = true;
    }

    void Attack1()
    {
        if (TargetDistance() < 5f && !isDescending)
        {
            StartDescending();
        }

        if (isDescending)
        {
            Descending();
        }

        if (!isDescending && transform.position.y - 1.35f <= groundYPosition && TargetDistance() <= 5f && canEarthquake && !canDash)
        {
            if (count < countEarthquake)
            {
                StartCoroutine(Earthquake());
            }
        }

        if (state == GolemState.Attacking_1)
        {
            return;
        }

        if (transform.position.y - 1.35f <= groundYPosition && TargetDistance() > 5f)
        {
            StartAscending();
        }

        if (isAscending)
        {
            Ascending();
        }
    }

    void Attack3()
    {
        if (TargetDistance() < 5f && canEarthquake)
        {
            anim.SetTrigger("Attack 3");
        }
    }

    void CounterDash()
    {
        if (canDash && TargetDistance() < 2f && Input.GetButtonDown("Attack"))
        {
            StopCoroutine(Earthquake());
            StartCoroutine(CounterDashEvent());
        }
    }

    IEnumerator Earthquake()
    {
        canEarthquake = false;
        state = GolemState.Attacking_1;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Attack");
        col2d.enabled = false;
        yield return new WaitForSeconds(durationEarthquake);
        col2d.enabled = true;
        state = GolemState.None;
        yield return new WaitForSeconds(cooldownEarthquake);
        canEarthquake = true;
    }

    IEnumerator CounterDashEvent()
    {
        Dashing();
        yield return new WaitForSeconds(dashTime);

        AfterDash();
        golem.canBeDamage = true;
        yield return new WaitForSeconds(restTime);

        state = GolemState.None;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void AfterDash()
    {
        if (playerLayerIndex != -1)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, playerLayerIndex, false);
        }
        col2d.enabled = true;
        rb.velocity = Vector2.zero;
    }

    private void Dashing()
    {
        state = GolemState.Dash;
        canDash = false;
        golem.canBeDamage = false;
        if (playerLayerIndex != -1)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, playerLayerIndex, true);
        }

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
    }


    public void FastAttack()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, fastHitRange, playerLayer);
        foreach (Collider2D player in hit)
        {
            if (player.TryGetComponent<PlayerClass>(out var playerComponent))
            {
                playerComponent.TakeDamage(damage, player.GetComponent<Player>().isDefending); 
            }
            else
            {
                Debug.Log("Aconteceu algo errado");
            }
        }
    }

    public void IceAttack()
    {
    }

    // Referênciada na animação de ataque do golem
    public void ExplosionCollider()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(pointers.explosionPoint.position, pointers.explosionRange, playerLayer);
        foreach (Collider2D player in hits)
        {
            if (player.TryGetComponent<PlayerClass>(out var playerComponent))
            {
                playerComponent.TakeDamage(damage, player.GetComponent<Player>().isDefending);
            }
        }
    }

    public bool Grounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, whatIsGround);
    }

    public void AttachedToGround()
    {
        if (!Grounded())
        {
            rb.velocity = new Vector2(0, -15);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }


}
