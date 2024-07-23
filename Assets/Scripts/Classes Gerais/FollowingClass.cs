using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingClass : MonoBehaviour
{
    [SerializeField]private Transform player;
    [SerializeField]private Animator enemyAnim;
    [SerializeField]private float speed;
    [SerializeField]private float maxDistance;
    [SerializeField]private float minDistance;

    public void Init(Transform player, Animator enemyAnim, float speed, float maxDistance, float minDistance)
    {
        this.player = player;
        this.enemyAnim = enemyAnim;
        this.speed = speed;
        this.maxDistance = maxDistance;
        this.minDistance = minDistance;
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
            enemyAnim.SetBool("Idle", true);
        }
    }
}
