using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfitityCenario : MonoBehaviour
{
    public float cenarioSpeed;
    
    private void Update() {
        MoveCenario();
    }

    private void MoveCenario() {
        Vector2 deslocated = new Vector2(Time.time * cenarioSpeed, 0);
        GetComponent<Renderer>().material.mainTextureOffset = deslocated;
    }
}
