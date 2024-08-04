using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    private BoxCollider2D boxCol;
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }

   private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            boxCol.enabled = true;
        }
   }
}
