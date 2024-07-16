using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ofensive_Golem : MonoBehaviour
{
    public Transform attackPoint;
    public Transform explosionPoint;
    public Transform iceAttackPoint;
    public Rigidbody2D iceRb;
    [SerializeField]float fastHitRange;
    public float explosionRange;
    [SerializeField]float iceRange; 


    [SerializeField] GameObject Ice;
    GameObject projectile;
    Rigidbody2D rb ;
    Animator IceAnim;
    [SerializeField] float IceSpeed;
    [SerializeField] float IceTime;

    bool grounded = false;


    void Update()
    {
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint && explosionPoint && iceAttackPoint)
        {
            Gizmos.DrawWireSphere(attackPoint.position, fastHitRange);
            Gizmos.DrawWireSphere(explosionPoint.position, explosionRange);
            Gizmos.DrawWireSphere(iceAttackPoint.position, iceRange);
        }
    }

    public void IceAttack()
    {
        grounded = GetComponent<Golem_Script>().Grounded();
        if (grounded)
        {
            projectile = Instantiate(Ice, iceAttackPoint.position, iceAttackPoint.rotation);
            rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(transform.localScale.x * IceSpeed, 0f);
            StartCoroutine(IceAttackTime());
        }
    }

    IEnumerator IceAttackTime() 
    {
        Animator IceAnim = projectile.GetComponent<Animator>();
        IceAnim.SetTrigger("Send");
        yield return new WaitForSeconds(IceTime);
        IceAnim.SetTrigger("Explode");
        rb.velocity = Vector2.zero;
       
    }

    public void Destroy()
    {
        Destroy(projectile);
    }


}
