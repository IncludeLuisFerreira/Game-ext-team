using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int Hp = 10;
    private int maxHeath;
    private int currentHelth;
    PlayerClass playerComponent  = null;
    
    private void Update() {
        if (playerComponent && Input.GetKeyDown(KeyCode.F)) {
            currentHelth = playerComponent.currentHelth;
            maxHeath = playerComponent.MaxHelth;
            
            if (currentHelth < maxHeath && (maxHeath - currentHelth) >= Hp) {
                playerComponent.currentHelth += Hp;
            }
            else if (maxHeath - currentHelth < Hp) {
                playerComponent.currentHelth = maxHeath;
            }
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerComponent = other.GetComponent<PlayerClass>();       
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerComponent = null;
        }
    }
}

