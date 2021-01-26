using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed;
    
    private void Update()
    {
        gameObject.transform.position += Vector3.right * Time.deltaTime;
    }

}
