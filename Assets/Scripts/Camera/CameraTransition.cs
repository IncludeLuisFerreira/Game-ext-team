using UnityEngine;

public class CameraManeger : MonoBehaviour
{ 
  public GameObject virtualCam;
  
  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      Debug.Log("Player in!\n");
      virtualCam.SetActive(true);
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      Debug.Log("Player out!\n");
      virtualCam.SetActive(false);
    }
  }
}
