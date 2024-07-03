using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Golem_Script : MonoBehaviour
{

    public enum golemState {Attacking_1, None}
    golemState state = golemState.None;
    [SerializeField]float vida = 15f;
    [SerializeField]float speed = 2f;
    [SerializeField]float decendingSpeed = 1.5f;
    [SerializeField]Transform Target;
    [SerializeField]float MinDistance = 10f;
    [SerializeField]float MaxDistance = 15f;
    Rigidbody2D rb;
    Animator anim;
    bool facingLeft = true;
    bool isDecending = false;
    bool isAcending = false;
    [SerializeField]float groundYPosition;
    [SerializeField]float originalYPosition;

    [Header("Campos de duração das habilidades")]
    [SerializeField]float duration_Earthqueak = 2.0f;

    private FollowingClass follow;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        follow = new(Target, anim, speed, MaxDistance, MinDistance);
    }
 
    void Update()
    {
        Flip();
        EnemyAttack();
        if (state == golemState.None) {
            follow.Follow(transform);
        }
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
            anim.SetTrigger("Attack");
            Earthqueak();
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


    /************************ Ataque 1************************/


    IEnumerable Earthqueak()
    {
        state = golemState.Attacking_1;
        rb.velocity = new Vector2(0f, 0f);
        yield return  new WaitForSeconds(duration_Earthqueak);
        state = golemState.None;
        
    }
}
