using System.Collections.Generic;
using UnityEngine;

public class Base : AliveGameUnit, IBuilding
{
    [SerializeField] private RectTransform basePanelReference; // Reference to UI panel

    private GameObject resourceRef;            // Reference to Unit resource object (for creating copy and consuming)
    private float resourceLeavingSpeed = 2f;   // Resource object consuming speed
    
    private List<GameObject> resourcesToSklad; // List of resource objects for consuming
    public Vector3 dispenserPosition;          // Place for resource consuming and dissappearing


    private void Awake()
    {
        resourcesToSklad = new List<GameObject>();
        gameObject.transform.GetChild(0).tag = TagConstants.baseStorageTag;
        gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.radiusLayer);
        dispenserPosition = gameObject.transform.GetChild(0).position;
    }

    private void Update()
    {
        ResourceConsuming();
    }

    private void OnTriggerEnter2D(Collider2D collider) // Unit collision (when Unit came to storage)
    {
        if (collider.gameObject.tag == TagConstants.unitTag) // if Unit intersects our collider
        {
            // Creating copy of unit.resource
            resourceRef = GameObject.Instantiate(collider.GetComponent<Unit>().resourcePrefab, 
                                                 collider.GetComponent<Unit>().resource.transform.position,
                                                 collider.GetComponent<Unit>().resource.transform.rotation);
            resourceRef.GetComponent<CircleCollider2D>().isTrigger = true;
            resourcesToSklad.Add(resourceRef);
        }
    }

    private void ResourceConsuming()                   // Resource consuming logic
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
        Debug.Log("Selected Base - go menu now");
    }
}
