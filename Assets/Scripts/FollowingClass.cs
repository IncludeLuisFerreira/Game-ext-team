using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingClass : MonoBehaviour
{
    private Transform player;
    private Animator enemyAnim;
    private float speed;
    private float maxDistance;
    private float minDistance;

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

        if (distanceToPlayer < minDistance && distanceToPlayer < maxDistance)
        {
            Vector3 direction = (player.position - enemy.position).normalized;
            enemy.position += speed * Time.deltaTime * direction;
            enemyAnim.SetBool("Run", true);
        }
        else
            enemyAnim.SetBool("Run", false);
    }
}
