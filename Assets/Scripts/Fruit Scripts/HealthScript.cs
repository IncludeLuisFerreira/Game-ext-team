using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int Hp = 10;
    private int maxHeath;
    private int currentHelth;
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (Input.GetKeyDown(KeyCode.F)) {
                if (other.TryGetComponent<PlayerClass>(out var playerComponent)) {
                    maxHeath = playerComponent.MaxHelth;
                    currentHelth = playerComponent.currentHelth;

                    if (currentHelth < maxHeath && maxHeath - currentHelth >= Hp) {
                        playerComponent.currentHelth += Hp;
                    }
                    else if (maxHeath - currentHelth < Hp) {
                        playerComponent.currentHelth = maxHeath;
                    }
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
    

