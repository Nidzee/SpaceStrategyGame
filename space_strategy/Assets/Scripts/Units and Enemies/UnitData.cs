using UnityEngine;
using Pathfinding;


public class UnitData
{
    public int ID;
    
    public GameObject resource;          // reference for calculating
    public int resourceType;
    
    private Rigidbody2D rb;

    #region Key waypoints
        public Base storage;       // Static for all units
        public Garage home;        // Garage reference
        public MineShaft workPlace;// Shaft reference
        public Vector3 destination;
    #endregion

    #region State machine variables
        public bool isApproachShaft = false;
        public bool isApproachStorage = false;
        public bool isApproachHome = false;
        public bool isGatheringComplete = false;

        public UnitIdleState unitIdleState = new UnitIdleState();
        public UnitIGoToState unitIGoToState = new UnitIGoToState();
        public UnitIHomelessState unitIHomelessState = new UnitIHomelessState();
        public UnitResourceLeavingState unitResourceLeavingState = new UnitResourceLeavingState();
        public UnitIGatherState unitIGatherState = new UnitIGatherState();
        public IUnitState currentState;
    #endregion

    public int currentState_ID;


    public Unit _myUnit;


    public void InitNewData(Unit unit)
    {
        _myUnit = unit;

        _myUnit.gameObject.name = unit.name;
        _myUnit.tag = TagConstants.unitTag;
        _myUnit.gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
        _myUnit.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.unitEnemiesResourcesBulletsLayer;
    }

    public void InitUnitFromFile(UnitSavingData savingData)
    {
        storage = ResourceManager.Instance.shtabReference;
        rb = _myUnit.GetComponent<Rigidbody2D>();


        ID = savingData.ID;
    
        Vector3 resourcePosition = new Vector3(savingData.resourcePosition_x, savingData.resourcePosition_y, savingData.resourcePosition_z);
        GameObject resourceFromFile = null;
        switch (savingData.resourceType)
        {
            case 1:
            resourceFromFile = GameObject.Instantiate(PrefabManager.Instance.crystalResourcePrefab, resourcePosition, Quaternion.identity);
            break;

            case 2:
            resourceFromFile = GameObject.Instantiate(PrefabManager.Instance.ironResourcePrefab, resourcePosition, Quaternion.identity);
            break;

            case 3:
            resourceFromFile = GameObject.Instantiate(PrefabManager.Instance.gelResourcePrefab, resourcePosition, Quaternion.identity);
            break;
        }

        if (resourceFromFile)
        {
            Vector3 myVector = _myUnit.transform.position - resourceFromFile.transform.position;
            resourceFromFile.gameObject.AddComponent<HingeJoint2D>();
            HingeJoint2D jointRef = resourceFromFile.gameObject.GetComponent<HingeJoint2D>();
            jointRef.connectedBody = rb;
            jointRef.autoConfigureConnectedAnchor = false;
            jointRef.connectedAnchor = new Vector2(0,0);
            jointRef.anchor = new Vector2(myVector.x*4, myVector.y*4);

            resourceFromFile.gameObject.GetComponent<CircleCollider2D>().isTrigger = true; // to make resource go through other units
            resource = resourceFromFile;
            resourceType = savingData.resourceType;
        }




        if (destination != null)
            destination = new Vector3(savingData.destination_x,savingData.destination_y,savingData.destination_z);


        if (savingData.targetObjectTransformName != null)
            _myUnit.GetComponent<AIDestinationSetter>().target = GameObject.Find(savingData.targetObjectTransformName).transform;


        isApproachShaft = false;
        isApproachStorage = false;
        isApproachHome = false;
        isGatheringComplete = false;
    }




















    public void OnTriggerEnter(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        if (collider.gameObject.tag == TagConstants.shaftDispenserTag && destination == collider.gameObject.transform.position)
        {
            _myUnit.GetComponent<AIDestinationSetter>().target = null;
            isApproachShaft = true;
        }

        if (collider.gameObject.tag == TagConstants.baseStorageTag && destination == collider.gameObject.transform.position)
        {
            _myUnit.GetComponent<AIDestinationSetter>().target = null;
            isApproachStorage = true;
        }

        if (collider.gameObject.tag == TagConstants.garageAngarTag && destination == collider.gameObject.transform.position)
        {
            _myUnit.GetComponent<AIDestinationSetter>().target = null;
            isApproachHome = true;
        }

        // Sets model unplacable
        if (collider.gameObject.tag == TagConstants.modelTag)
        {
            // Debug.Log("Unit intersects model!");
            GameHendler.Instance.buildingModel.isModelPlacable = false;
        }
    }

    public void OnCollisionEnter(Collision2D collision) // resource collision
    {
        if (collision.gameObject.tag == TagConstants.resourceTag && collision.gameObject == resource.gameObject) // correct resource
        {
            Debug.Log("Collides with resource!");
            // Joint Logic
            Vector3 myVector = _myUnit.transform.position - collision.transform.position;
            collision.gameObject.AddComponent<HingeJoint2D>();
            var temp = collision.gameObject.GetComponent<HingeJoint2D>();

            temp.connectedBody = rb;
            temp.autoConfigureConnectedAnchor = false;
            temp.connectedAnchor = new Vector2(0,0);
            temp.anchor = new Vector2(myVector.x*4, myVector.y*4);

            isGatheringComplete = true;


            collision.gameObject.GetComponent<CircleCollider2D>().isTrigger = true; // to make resource go through other units
        }
    }


    public void ChangeDestination(int destinationID)
    {
        switch (destinationID)
        {
            case (int)UnitDestinationID.Home :
            _myUnit.GetComponent<AIDestinationSetter>().target = home.GetUnitDestination();
            destination = home.GetUnitDestination().position;
            break;

            case (int)UnitDestinationID.WorkPlace :
            _myUnit.GetComponent<AIDestinationSetter>().target = workPlace.GetUnitDestination();
            destination = workPlace.GetUnitDestination().position;
            break;

            case (int)UnitDestinationID.Storage :
            _myUnit.GetComponent<AIDestinationSetter>().target = storage.GetUnitDestination();
            destination = storage.GetUnitDestination().position;
            break;

            case (int)UnitDestinationID.Null :
            // _myUnit.GetComponent<AIDestinationSetter>().target = _myUnit.transform;
            _myUnit.GetComponent<AIDestinationSetter>().target = null;
            _myUnit.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            destination = _myUnit.transform.position;
            break;
        }
    }

    public void LifeCycle(Unit unit)
    {
        Debug.Log(currentState);
        currentState = currentState.DoState(unit);
        Debug.Log(currentState);
    }

    public void CreateUnit(Garage garage, Unit unit)
    {
        ID = UnitStaticData.unit_counter;
        
        _myUnit = unit;

        _myUnit.gameObject.name = "U" + UnitStaticData.unit_counter;
        UnitStaticData.unit_counter++;

        _myUnit.tag = TagConstants.unitTag;
        _myUnit.gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
        _myUnit.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.unitEnemiesResourcesBulletsLayer;

        currentState = unitIdleState;
        storage = ResourceManager.Instance.shtabReference;
        
        garage.AddCreatedByButtonUnit(_myUnit);
        
        rb = _myUnit.GetComponent<Rigidbody2D>();
    }


    // Redo Add intermidiary calculations to reduce functions calls 

    public void DestroyUnit()
    {
        MineShaft temp = null;

        if (home)
        {
            Garage newHome = home;
            home.RemoveUnit(_myUnit); // Executes Event
            ResourceManager.Instance.SetHomelessUnitOnDeadUnitPlace(newHome); // Executes Event

            if (workPlace)
            {
                temp = workPlace;
                workPlace.RemoveUnit(_myUnit); // Executes Event
            }
            else
            {
                ResourceManager.Instance.avaliableUnits.Remove(_myUnit);
            }
        }

        else 
        {
            ResourceManager.Instance.homelessUnits.Remove(_myUnit);
        }

        ResourceManager.Instance.unitsList.Remove(_myUnit);

        if (resource)
        {
            GameObject.Destroy(resource.gameObject);
        }
        // No need for reloading buildings manage menu





        //REDO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        GameViewMenu.Instance.ReloadUnitManageMenuAfterUnitDeath(temp);
    }
}
