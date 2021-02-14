using UnityEngine;

public class ConsumerMover : MonoBehaviour
{
    public Vector3 cnsumerPosition;          // Consumer position

    public int resourceType;

    private float resourceLeavingSpeed = 2f; // Resource object consuming speed

    private void Update()
    {
        if (transform.position == cnsumerPosition)
        {
            GameObject.Destroy(this.gameObject);
            switch (resourceType)
            {
                case 1:
                Debug.Log("We got CRYSTAL resource!");
                ResourceManager.Instance.AddCrystalResourcePoints();
                break;

                case 2:
                Debug.Log("We got IRON resource!");
                ResourceManager.Instance.AddIronResourcePoints();
                break;

                case 3:
                Debug.Log("We got GEL resource!");
                ResourceManager.Instance.AddGelResourcePoints();
                break;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, cnsumerPosition, resourceLeavingSpeed*Time.deltaTime);
    }
}
