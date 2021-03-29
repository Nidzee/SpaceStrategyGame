using UnityEngine;

public class BomberRange : MonoBehaviour
{
    private EnemyBomber myBomber;


    private void Awake()
    {
        myBomber = gameObject.transform.parent.GetComponent<EnemyBomber>();
    }

    private void OnTriggerEnter2D(Collider2D collider) // Detects enemy when it arrives in combat range
    {
        if (collider.gameObject.tag == TagConstants.buildingTag)
        {
            Debug.Log("Collided :" + collider.gameObject);

            if (collider.gameObject.GetComponent<PowerPlant>() != null) // this means we found power plant
            {
                Debug.Log("Collided Power Plant!");

                // myBomber._path = null;
                myBomber.buildingsInRange.Add(collider.gameObject);
                myBomber.ComparePathesToShtabAndToTargetBuilding(collider.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) // Detects if enemy left the combat range (Death or passing through)
    {
        if (collider.gameObject.tag == TagConstants.buildingTag)
        {
            if (collider.gameObject.GetComponent<PowerPlant>() != null) // this means we found power plant
            {
                myBomber.buildingsInRange.Remove(collider.gameObject);
            }
        }
    }
}
