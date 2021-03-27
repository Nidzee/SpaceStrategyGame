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


    public Unit _myUnit;








    // public void SetWorkPlaceToNull()
    // {
    //     workPlace = null;
    // }

    // public void SetHomeToNull()
    // {
    //     home = null;
    // }

    // public void SetNewWorkPlace(MineShaft newWorkPlace)
    // {
    //     workPlace = newWorkPlace;
    // }

    // public void SetNewHome(Garage newHome)
    // {
    //     home = newHome;
    // }

    // public MineShaft GetWorkPlace()
    // {
    //     return workPlace;
    // }
















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
        currentState = currentState.DoState(unit);
    }

    public void CreateUnit(Garage garage, Unit unit)
    {
        _myUnit = unit;

        UnitStaticData.unit_counter++;

        _myUnit.gameObject.name = "Unit - " + UnitStaticData.unit_counter;
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
