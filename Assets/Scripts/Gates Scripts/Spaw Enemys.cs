using System.Collections;
using UnityEngine;
using System;

public class SpawEnemys : MonoBehaviour
{
    public GameObject[] enemies;
    public bool trigger;
    public bool canOpenGates {get; private set;}
    public float spawTime;
    public int enemiesInRoom = 0;
    public int enemiesCount;

    private int count = 0;
    private bool isSpawning = false;

    private void Update() {
        if (trigger && count < enemiesCount && !isSpawning) {
            StartCoroutine(SpawEnemyEvent());
        }

        if (count == enemiesCount && enemiesInRoom == 0) {
            canOpenGates = true;
        } 
    }

    IEnumerator SpawEnemyEvent() {
        isSpawning = true;
        yield return new WaitForSeconds(spawTime);

        if (count < enemiesCount) {
            SpawEnemy();
        }

        isSpawning = false;
    }

    void SpawEnemy() {
        if (count < enemiesCount){
            int index = UnityEngine.Random.Range(0, enemies.Length);
            GameObject teste = Instantiate(enemies[index], transform.position, Quaternion.identity);
            teste.GetComponent<EnemyClass>().spawner = this;
            count++;
            enemiesInRoom++;
        }
    }
}
