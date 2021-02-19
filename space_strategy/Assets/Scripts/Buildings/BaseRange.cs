using UnityEngine;

public class BaseRange : MonoBehaviour
{
    private Base shtabRef;

    private void Awake()
    {
        shtabRef = gameObject.transform.parent.GetComponent<Base>();
    }

    private void OnTriggerEnter2D(Collider2D collider) // Unit collision (when Unit came to storage)
    {
        if (collider.gameObject.tag == TagConstants.unitTag && collider.GetComponent<Unit>().destination == transform.position)
        {
            Unit unitWhichBringsResource = collider.GetComponent<Unit>();

            // Creating copy of resource
            shtabRef.resourceRef = GameObject.Instantiate(unitWhichBringsResource.resource.gameObject, unitWhichBringsResource.resource.transform.position, unitWhichBringsResource.resource.transform.rotation);
            Destroy (shtabRef.resourceRef.GetComponent<HingeJoint2D>());
            shtabRef.resourceRef.GetComponent<CircleCollider2D>().isTrigger = true;
            
            // Sending resource to consumer - OFFLINE
            shtabRef.resourceRef.AddComponent<ConsumerMover>();
            shtabRef.resourceRef.GetComponent<ConsumerMover>().cnsumerPosition = shtabRef.storageConsumer.transform.position;
            
            
            shtabRef.resourceRef.GetComponent<ConsumerMover>().resourceType = unitWhichBringsResource.resourceType;

            // reset reference
            shtabRef.resourceRef = null;
        }
    }
}
