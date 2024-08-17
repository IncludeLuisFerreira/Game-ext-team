using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;


public class Death : MonoBehaviour {
    
    Rigidbody2D deathRb;
    
    Transform _target;
    

    private void Awake() {
        deathRb = GetComponent<Rigidbody2D>();
        _target = GameObject.Find("Player").transform;
    }
    
    
    
}