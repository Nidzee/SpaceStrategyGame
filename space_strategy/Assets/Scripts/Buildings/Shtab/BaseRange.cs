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
        
        if (collider.gameObject.tag == TagConstants.unitTag && collider.GetComponent<Unit>().Target == this.gameObject)
        {
            Unit unitWhichBringsResource = collider.GetComponent<Unit>();



            // ONLY COMMENTED FOR PROJECT - LOGICALLY IT NEEDS TO BE HERE UNCOMMENTED!

            // switch (unitWhichBringsResource.resourceType)
            // {
            //     case 1:
            //     Debug.Log("We got CRYSTAL resource!");
            //     ResourceManager.Instance.AddCrystalResourcePoints();
            //     break;

            //     case 2:
            //     Debug.Log("We got IRON resource!");
            //     ResourceManager.Instance.AddIronResourcePoints();
            //     break;

            //     case 3:
            //     Debug.Log("We got GEL resource!");
            //     ResourceManager.Instance.AddGelResourcePoints();
            //     break;
            // }




            // Creating copy of resource
            shtabRef.resourceRef = GameObject.Instantiate(unitWhichBringsResource.resource.gameObject, unitWhichBringsResource.resource.transform.position, unitWhichBringsResource.resource.transform.rotation);
            Destroy (shtabRef.resourceRef.GetComponent<HingeJoint2D>());
            shtabRef.resourceRef.GetComponent<CircleCollider2D>().isTrigger = true;
            
            // Sending resource to consumer - OFFLINE
            shtabRef.resourceRef.AddComponent<ConsumerMover>();
            shtabRef.resourceRef.GetComponent<ConsumerMover>().cnsumerPosition = shtabRef.storageConsumer.transform.position;

            // reset reference
            shtabRef.resourceRef = null;
        }
    }
}
