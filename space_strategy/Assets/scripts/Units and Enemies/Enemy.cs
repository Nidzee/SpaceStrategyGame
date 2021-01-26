using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 1.5f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * _speed;
    }
}
