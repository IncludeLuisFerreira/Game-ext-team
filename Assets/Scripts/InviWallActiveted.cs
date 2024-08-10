using UnityEngine;

public class InviWallActiveted : MonoBehaviour
{

    public Collider2D Inv1;
    public Collider2D Inv2;
    public SpawEnemys spawner;


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && Inv1 != null && Inv2 != null) {
            Inv1.enabled = true;
            Inv2.enabled = true;
            spawner.trigger = true;
        }
    }

    private void Update() {
        if (spawner.canOpenGates) {
            Inv1.enabled = false;
            Inv2.enabled = false;
        }
    }
}
