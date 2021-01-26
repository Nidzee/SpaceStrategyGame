﻿using UnityEngine;

public class Misile : MonoBehaviour
{
    public Enemy target;
    private Rigidbody2D rb;

    private float moveSpeed = 4f;
    private float rotateSpeed = 250f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveMisile();
    }

    private void MoveMisile()
    {
        if (target == null)
        {
            Debug.Log("Destroy misile");
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (Vector2)target.transform.position - rb.position; 
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.right).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.right * moveSpeed;
    }
    
    private void OnTriggerEnter2D(Collider2D collider) // Detects if enemy left the combat range (Death or passing through)
    {
        if (collider.gameObject.tag == TagConstants.enemyTag)
        {
            Debug.Log("Enemy down!");
            Destroy(gameObject);
        }
    }
}
