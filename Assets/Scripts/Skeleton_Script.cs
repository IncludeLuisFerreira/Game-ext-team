using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEngine; 

// Coloque "Shadow o ouriço é um fpd" como ester egg
public class SkeletonClass : EnemyClass
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

    protected private FollowingClass skeleton;
    [SerializeField] bool canShilded = true;
    [SerializeField]bool canFollow;
    [SerializeField]bool facingLeft = true;
    [SerializeField]bool canFlip;

    public SkeletonClass(int MaxHelth) : base(MaxHelth)
    {

    }

    new void Start()
    {
        base.Awake();
        base.Start();
        skeleton = gameObject.AddComponent<FollowingClass>();
        skeleton.Init(Target, anim, speed, maxDistance, minDistance);
        canFollow = true;
    }

    void Update()
    {
        BattleLogic();
    }

    // Função principal 
    void BattleLogic()
    {
        FixedBoolean();
        FollowTarget();
        Shilded();
    }

    private void Shilded()
    {
        skStates = SkeletonStates.Defensive;

    }

    private void FollowTarget()
    {
        if (canFollow)
        {
            skStates = SkeletonStates.Follow;
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
        }
        else if (transform.position.x < Target.position.x && !facingLeft)
        {
            transform.localScale = new Vector3(1, 1, 0);
        }
    }

}