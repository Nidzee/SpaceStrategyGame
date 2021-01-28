using System.Collections.Generic;
using UnityEngine;

public class Base : AliveGameUnit, IBuilding
{
    public static GameObject basePrefab;

    public GameObject resourceRef;            // Reference to Unit resource object (for creating copy and consuming)
    public float resourceLeavingSpeed = 2f;   // Resource object consuming speed    
    public List<GameObject> resourcesToSklad; // List of resource objects for consuming
    
    public Vector3 storageConsumerPosition;    // Place for resource consuming and dissappearing


    public static void InitStaticFields()
    {
        basePrefab = PrefabManager.Instance.basePrefab;
    }


    public void Creation()
    {
        resourcesToSklad = new List<GameObject>();
        
        transform.tag = TagConstants.buildingTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;

        HelperObjectInit();
    }


    private void Update()
    {
        ResourceConsuming();
    }


    private void HelperObjectInit()
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.baseStorageTag;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            
            storageConsumerPosition = gameObject.transform.GetChild(0).transform.position;
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }


    private void ResourceConsuming()                   // Resource consuming logic
    {
        if (resourcesToSklad.Count != 0) // resource taking logic
        {
            for (int i = resourcesToSklad.Count - 1; i >= 0; i--)
            {
                GameObject temp = resourcesToSklad[resourcesToSklad.Count - 1];
                if (resourcesToSklad[i].transform.position == storageConsumerPosition)
                {
                    Debug.Log("We got resource!");
                    resourcesToSklad.Remove(resourcesToSklad[i]);
                    GameObject.Destroy(temp);
                }
                else 
                {
                    temp.transform.position = Vector3.MoveTowards(
                                                    temp.transform.position, 
                                                    storageConsumerPosition, 
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
