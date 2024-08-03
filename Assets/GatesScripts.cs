using UnityEngine;

public class GatesScripts : MonoBehaviour
{
    public GameObject GateOne;
    public GameObject GateTwo;
    public int countEnemys;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GateOne.transform.position = new Vector3(GateOne.transform.position.x, -27.5f, 0);
            GateTwo.transform.position = new Vector3(GateTwo.transform.position.x, -27.5f, 0);
        }
    }

    private void Update() {
        if (countEnemys == 0) {
            GateOne.transform.position = new Vector3(GateOne.transform.position.x, -37.5f, 0);
            GateTwo.transform.position = new Vector3(GateTwo.transform.position.x, -37.5f, 0);

        }
    }

}
