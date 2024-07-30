using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingClass : MonoBehaviour
{
    private Transform player;
    private Animator enemyAnim;
    private float speed;
    private float maxDistance;

    public void Init(Transform player, Animator enemyAnim, float speed, float maxDistance)
    {
        this.player = player;
        this.enemyAnim = enemyAnim;
        this.speed = speed;
        this.maxDistance = maxDistance;
    }

    public void Follow(Transform enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.position, player.position);

        if (distanceToPlayer < maxDistance)
        {
            Vector3 direction = (player.position - enemy.position).normalized;
            enemy.position += speed * Time.deltaTime * direction;
            enemyAnim.SetBool("Run", distanceToPlayer != 0.5f);
            enemyAnim.SetBool("Idle", false);
        }
        else 
        {
            enemyAnim.SetBool("Run", false);
            try
            {
                enemyAnim.SetBool("Idle", true);
            }
            catch (System.Exception)
            {
                
            }
            
        }
    }
}
