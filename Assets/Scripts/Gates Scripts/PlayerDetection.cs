using System.Collections;
using UnityEngine;

public class GatesScripts : MonoBehaviour
{
    public GameObject GateOne;
    public GameObject GateTwo;

    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GateOne.transform.position = new Vector3(GateOne.transform.position.x, -27.5f, 0);
            GateTwo.transform.position = new Vector3(GateTwo.transform.position.x, -27.5f, 0);
            SpawEnemys.Instance.canSpaw = true;
        }
    }

    private void Update() {
        StartCoroutine(CanOpenGates());
    }

    private IEnumerator CanOpenGates() {
        if (SpawEnemys.Instance.canOpenGates == true) {
            yield return new WaitForSeconds(1.0f);
            GateOne.transform.position = new Vector3(GateOne.transform.position.x, -37.5f, 0);
            GateTwo.transform.position = new Vector3(GateTwo.transform.position.x, -37.5f, 0);
            Destroy(gameObject);
        }
        yield return null;
    }

}
