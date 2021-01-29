using UnityEngine;

public class ConsumerMover : MonoBehaviour
{
    public Vector3 cnsumerPosition;          // Consumer position

    private float resourceLeavingSpeed = 2f; // Resource object consuming speed

    private void Update()
    {
        if (transform.position == cnsumerPosition)
        {
            Debug.Log("We got resource!");
            GameObject.Destroy(this.gameObject);
        }

        transform.position = Vector3.MoveTowards(transform.position, cnsumerPosition, resourceLeavingSpeed*Time.deltaTime);
    }
}
