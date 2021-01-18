using System.Collections.Generic;
using UnityEngine;

public class Base : AliveGameUnit, IBuilding
{
    private GameObject resourceRef;
    private float resourceLeavingSpeed = 2f;
    
    private List<GameObject> resourcesToSklad = new List<GameObject>(); // to store multiple resources for taking
    public Vector3 dispenserPosition; // Place for resource consuming


    private void Awake() // Maybe useless
    {
        gameObject.transform.GetChild(0).tag = "SkladRadius";
        dispenserPosition = gameObject.transform.GetChild(0).position;
    }

    private void Update()
    {
        ResourceConsuming();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Unit") // if Unit intersects our collider
        {
            // Creating copy of unit.resource
            resourceRef = GameObject.Instantiate(collider.GetComponent<Unit>().resourcePrefab, 
                                                collider.GetComponent<Unit>().resource.transform.position,
                                                collider.GetComponent<Unit>().resource.transform.rotation);
            resourceRef.GetComponent<CircleCollider2D>().isTrigger = true;
            resourcesToSklad.Add(resourceRef);
        }
    }

    private void ResourceConsuming()
    {
        if (resourcesToSklad.Count != 0) // resource taking logic
        {
            for (int i = resourcesToSklad.Count - 1; i >= 0; i--)
            {
                GameObject temp = resourcesToSklad[resourcesToSklad.Count - 1];
                if (resourcesToSklad[i].transform.position == dispenserPosition)
                {
                    Debug.Log("We got resource!");
                    resourcesToSklad.Remove(resourcesToSklad[i]);
                    GameObject.Destroy(temp);
                }
                else 
                {
                    temp.transform.position = Vector3.MoveTowards(
                                                    temp.transform.position, 
                                                    dispenserPosition, 
                                                    resourceLeavingSpeed*Time.deltaTime);
                }
            }
        }
    }

       
    public void Invoke()
    {
        
    }
}
