using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Ofensive_Goblim : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange;
    public Transform attackPoint2;
    public float attackRange2;


    void OnDrawGizmosSelected()
    {
        if (attackPoint && attackPoint2)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
            Gizmos.DrawWireSphere(attackPoint2.position, attackRange2);

        }
    }
}