using System.Collections;
using UnityEngine; 

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


    [SerializeField]Transform Target;
    Animator anim;
    FollowingClass skeleton;
    EnemyClass skEnemy;

    [SerializeField]bool canFollow;
    [SerializeField]bool facingLeft = true;
    [SerializeField]bool canFlip;


    void Start()
    {
        anim = GetComponent<Animator>();
        skEnemy = GetComponent<EnemyClass>();
        skeleton = gameObject.AddComponent<FollowingClass>();
        skeleton.Init(Target, anim, speed, maxDistance, minDistance);
    }

    void Update()
    {
        BattleLogic();
    }

    void BattleLogic()
    {
        FixedBoolean();
        FollowTarget();
        Shilded();
        Flip();
        Melee();
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

                break;
            case SkeletonStates.Attack1:
                canFollow = false;
                break;
            case SkeletonStates.Attack2:

                break;
            case SkeletonStates.Defensive:
                canFollow = false;
                break;
            default:
                canFollow = true;
                break;
        }
    }

    void Flip()
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

    void Melee()
    {
        if (skEnemy.DeltaDistance(Target) < 2)
        {
            StartCoroutine(MeleeSequence());
        }
    }

    IEnumerator MeleeSequence()
    {
        anim.SetTrigger("Attack 1");
        skStates = SkeletonStates.Attack1;
        yield return new WaitForSeconds(1.2f);
        skStates = SkeletonStates.None;
    }

}