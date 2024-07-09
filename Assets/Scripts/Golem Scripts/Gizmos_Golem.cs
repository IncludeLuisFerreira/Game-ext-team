using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ofensive_Golem : MonoBehaviour
{
    public Transform attackPoint;
    [SerializeField]public Transform explosionPoint;
    [SerializeField]public Transform iceAttackPoint;
    public Rigidbody2D iceRb;
    [SerializeField]float fastHitRange;
    [SerializeField]float explosionRange;
    [SerializeField]float iceRange;

    void OnDrawGizmosSelected()
    {
        if (attackPoint && explosionPoint && iceAttackPoint)
        {
            Gizmos.DrawWireSphere(attackPoint.position, fastHitRange);
            Gizmos.DrawWireSphere(explosionPoint.position, explosionRange);
            Gizmos.DrawWireSphere(iceAttackPoint.position, iceRange);

        }
    }

   

}
