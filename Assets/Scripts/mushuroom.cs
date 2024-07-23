using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushuroom : MonoBehaviour
{

    Rigidbody2D rb;

    [SerializeField]float speed;
    [SerializeField]Transform player;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Andar();
        Follow();
    }

    void Andar()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    void Follow()
    {
        if(player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1,1,0);
            rb.velocity = new Vector2(transform.localScale.x * speed,rb.velocity.y);
        }
        else
        {
            transform.localScale = new Vector3(-1,1,0);
            rb.velocity = new Vector2(transform.localScale.x * speed,rb.velocity.y);

        }
    }
    
}

// teste de colab
