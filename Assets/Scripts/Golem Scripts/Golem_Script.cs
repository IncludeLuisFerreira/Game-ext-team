using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


/* Tenho certeza que quando pensei nesse personagem me perdi em ideias,
muitas boas e muitas ruins, tantas que me perdi na minha própria ideia.*/
public class Golem_Script : MonoBehaviour
{
    public enum GolemState {Idle,Following, Dash,Charging, Attacking_1, Hit, None}
    [SerializeField] GolemState state = GolemState.None;

    [Header("Atributos")]
    [SerializeField] float speed = 2f;
    [SerializeField] int damage = 5;
    [SerializeField] float descendingSpeed = 1.5f;
    [SerializeField] float maxDistance = 15f;

    [Header("Variáveis booleanas")]
    public bool facingLeft = true;
    public bool canDash = true;
    public bool canEarthquake = true;
    public bool canExplode = true;
    public bool grounded;
    public bool canFollow;
    public bool canAttack;
    public bool charged;

    [Header("Transforms")]
    [SerializeField] Transform target;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform ice;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask whatIsGround;

    private FollowingClass GolemFollow;
    private EnemyClass golem;
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col2d;
    private GameObject Player;
    private Ofensive_Golem pointers;


    private int playerLayerIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        golem = GetComponent<EnemyClass>();
        col2d = GetComponent<Collider2D>();
        pointers = GetComponent<Ofensive_Golem>();
        GolemFollow = gameObject.AddComponent<FollowingClass>();

        playerLayerIndex = LayerMask.NameToLayer("Player");
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        GolemFollow.Init(target, anim, speed, maxDistance);
        state = GolemState.Idle;
    }

    void Update()
    {
        /* FixedBoolean();
        BattleLogic(); */
        if (Input.GetKeyDown(KeyCode.Z))
            StartCoroutine(ReloadAbility());
        //FastAttack();
    }

    void FixedBoolean() 
    {
        switch (state)
        {
            case GolemState.Idle:
            case GolemState.None:
            case GolemState.Following:
                canFollow = true;
                break;
            case GolemState.Dash:
            case GolemState.Attacking_1:
            case GolemState.Hit:
            case GolemState.Charging:
                canFollow = false;
                break;
        }

        if (Grounded()) {grounded = true;}
        else {grounded = false;}
        anim.SetBool("Grounded", grounded);
    }

    void BattleLogic() 
    {
        FollowTarget();
    }

    // Segue o inimigo se canFollow = true e a distância do player for menor que maxDistance.
    void FollowTarget()
    {
        if (canFollow & TargetDistance() < maxDistance)
        {
            Flip();
            GolemFollow.Follow(transform);
            state = GolemState.Following;
        }
        else if (TargetDistance() > maxDistance || !canFollow)
        {
            state = GolemState.Idle;
            anim.SetBool("Run", false);
        }
    }

    float TargetDistance()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    public bool Grounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, whatIsGround);
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

    IEnumerator ReloadAbility()
    {
        anim.SetTrigger("Ability");
        state = GolemState.Charging;
        yield return new WaitForEndOfFrameUnit();
        float duracion = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duracion);
        charged = true;
        Debug.Log(duracion);
    }

    void FastAttack()
    {
        if (canAttack && charged)
        {
            state = GolemState.Attacking_1;
            anim.SetTrigger("Attack");
            charged = false;
        }
        
    }
}

