using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Add timer and death if shoot passes through dead Enemy
    
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
