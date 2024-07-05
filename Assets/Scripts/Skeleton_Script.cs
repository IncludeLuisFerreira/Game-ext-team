using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Skeleton_Script : MonoBehaviour
{
    [SerializeField]Transform Target;
    Animator Anim;
    [SerializeField]float speed = 1.5f;
    [SerializeField]float maxDistance = 20f;
    [SerializeField]float minDistance = 15f;
    public bool facingLeft = true;

    private FollowingClass follow; 
    private Rigidbody2D rb;   
    
    void Awake()
    {
       Anim = GetComponent<Animator>();
    }
    void Start()
    {
        follow = new(Target, Anim, speed, maxDistance, minDistance);
    }

    void Update()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        follow.Follow(transform);
        Flip();
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

}
