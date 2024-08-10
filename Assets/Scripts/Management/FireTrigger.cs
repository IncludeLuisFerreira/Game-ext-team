using System.Collections;
using UnityEngine;

public class FireTrigger : MonoBehaviour
{
    public GameObject firePrefab;
    public Animator _sparkAnimator;
    private Transform _player;
    private Collider2D _coll;
    private bool validColision = false;
    private bool canSpawFire = true;
    private int fireCount = 1; 

    private void Awake() 
    {
        _player = GameObject.Find("Player").transform;
        _coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && !validColision)
        {
            Vector2 exitDir = (other.transform.position - _coll.bounds.center).normalized;

            if (exitDir.x > 0)
            {
                _sparkAnimator.SetTrigger("CanStart");
                validColision = true;
            }
        }
    }

    private void Update() 
    {
        if (canSpawFire && validColision && fireCount < 4)
        {
            OnFire(fireCount++);
            canSpawFire = false;
            StartCoroutine(CanSpawFire()); 
        }
    }

    IEnumerator CanSpawFire()
    {
        yield return new WaitForSeconds(1);
        canSpawFire = true;
    }

    void OnFire(int i)
    {
        Vector3 pos = new Vector3(_sparkAnimator.transform.position.x + i, _sparkAnimator.transform.position.y, 0);
        Instantiate(firePrefab, pos, Quaternion.identity);
    }
}
