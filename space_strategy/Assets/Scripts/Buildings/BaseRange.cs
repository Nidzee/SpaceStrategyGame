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
        if (collider.gameObject.tag == TagConstants.unitTag && collider.GetComponent<Unit>().unitData.destination == transform.position)
        {
            Unit unitWhichBringsResource = collider.GetComponent<Unit>();

            // Creating copy of resource
            shtabRef.shtabData.resourceRef = GameObject.Instantiate(unitWhichBringsResource.unitData.resource.gameObject, unitWhichBringsResource.unitData.resource.transform.position, unitWhichBringsResource.unitData.resource.transform.rotation);
            Destroy (shtabRef.shtabData.resourceRef.GetComponent<HingeJoint2D>());
            shtabRef.shtabData.resourceRef.GetComponent<CircleCollider2D>().isTrigger = true;
            
            // Sending resource to consumer - OFFLINE
            shtabRef.shtabData.resourceRef.AddComponent<ConsumerMover>();
            shtabRef.shtabData.resourceRef.GetComponent<ConsumerMover>().cnsumerPosition = shtabRef.shtabData.storageConsumer.transform.position;
            
            
            shtabRef.shtabData.resourceRef.GetComponent<ConsumerMover>().resourceType = unitWhichBringsResource.unitData.resourceType;

            // reset reference
            shtabRef.shtabData.resourceRef = null;
        }
    }
}
