using UnityEngine;

public class Enemy : AliveGameUnit
{
    public Base baseTarget;       // Static for all units
    public float moveSpeed;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // private void Update()
    // {
    //     rb.velocity = new Vector3(150,0,0) * Time.deltaTime;
    // }
}
