using System.Collections.Generic;
using UnityEngine;

public class Sklad : MonoBehaviour
{
    private GameObject resourceRef;
    private Vector3 resourceTakerPlace;
    private float resourceLeavingSpeed = 2f;
    
    private List<GameObject> resourcesToSklad = new List<GameObject>(); // to store multiple resources for taking

    private void Awake()
    {
        resourceTakerPlace = gameObject.transform.GetChild(0).position; // static field
    }

    private void Update()
    {
        if (resourcesToSklad.Count != 0) // resource taking logic
        {
            for (int i = resourcesToSklad.Count - 1; i >= 0; i--)
            {
                GameObject temp = resourcesToSklad[resourcesToSklad.Count - 1];
                if (resourcesToSklad[i].transform.position == resourceTakerPlace)
                {
                    Debug.Log("We got resource!");
                    resourcesToSklad.Remove(resourcesToSklad[i]);
                    GameObject.Destroy(temp);
                }
                else 
                {
                    temp.transform.position = Vector3.MoveTowards(
                                                    temp.transform.position, 
                                                    resourceTakerPlace, 
                                                    resourceLeavingSpeed*Time.deltaTime);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Unit") // if Unit intersects our collider
        {
            // Creating copy of unit.resource
            // Deleting unit.resource game object
            resourceRef = GameObject.Instantiate(collider.GetComponent<Unit>().resourcePrefab, 
                                            collider.transform.GetComponent<Unit>().resource.transform.position,
                                            collider.transform.GetComponent<Unit>().resource.transform.rotation);
            resourceRef.GetComponent<CircleCollider2D>().isTrigger = true;
            resourcesToSklad.Add(resourceRef);

            GameObject.Destroy(collider.GetComponent<Unit>().resource); 
        }
    }
}
