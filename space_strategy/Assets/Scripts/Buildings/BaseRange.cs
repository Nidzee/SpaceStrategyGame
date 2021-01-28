using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRange : MonoBehaviour
{
    private Base myBase;

    private void Awake()
    {
        myBase = gameObject.transform.parent.GetComponent<Base>();
    }

    private void OnTriggerEnter2D(Collider2D collider) // Unit collision (when Unit came to storage)
    {
        if (collider.gameObject.tag == TagConstants.unitTag) // if Unit intersects our collider
        {
            // Creating copy of unit.resource
            myBase.resourceRef = GameObject.Instantiate(collider.GetComponent<Unit>().resourcePrefab, 
                                                 collider.GetComponent<Unit>().resource.transform.position,
                                                 collider.GetComponent<Unit>().resource.transform.rotation);
            myBase.resourceRef.GetComponent<CircleCollider2D>().isTrigger = true;
            myBase.resourcesToSklad.Add(myBase.resourceRef);
        }
    }
}
