using UnityEngine;

class InitScene : MonoBehaviour
{
    Rigidbody2D CameraRb;

    void Awake()
    {
        CameraRb = gameObject.AddComponent<Rigidbody2D>();
    }

    void Start()
    {
        transform.localPosition = new Vector3(15, 11, -1);
        CameraRb.gravityScale = 0;
    }

    void Update()
    {
        CameraRb.velocity = new Vector2(-2.5f, CameraRb.velocity.y);

        if (transform.localPosition.x < -7)
        {
            CameraRb.velocity = Vector2.up;
        }
    }
}