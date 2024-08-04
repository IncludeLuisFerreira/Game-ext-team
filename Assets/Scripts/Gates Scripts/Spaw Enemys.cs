using System.Security.Cryptography;
using UnityEngine;

public class SpawEnemys : MonoBehaviour
{
    public static SpawEnemys Instance {get; set;}
    public GameObject enemy;
    public float spawRate;
    public int maxCountEnemy = 3;
    
    private int currentCountEnemy = 0;
    private float nextSpawn = 0f;
    public bool canSpaw  = false;
    public int enemysAlive;
    public bool canOpenGates = false;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        
        if (Time.time > nextSpawn && currentCountEnemy < maxCountEnemy && canSpaw) {
            nextSpawn = Time.time + spawRate;
            Instantiate(enemy, transform.position, Quaternion.identity);
            currentCountEnemy++;
            enemysAlive++;
        }
        OpenGates();
    }

    void OpenGates() {
        if (enemysAlive == 0 && currentCountEnemy == maxCountEnemy) {
            canOpenGates = true;
        }
    }
}
