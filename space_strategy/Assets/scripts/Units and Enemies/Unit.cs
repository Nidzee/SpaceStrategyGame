using UnityEngine;
using Pathfinding;


public class Unit : AliveGameUnit
{
    private static int unit_counter = 0; // Unit number in ibspector
    public static GameObject unitPrefab; // Unit prefab
    public static float moveSpeed = 1f;  // Const speed of all units

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











    // Unit life cycle
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (name == "Unit1")
            {
                Death();
            }
        }
        currentState = currentState.DoState(this);
    }








    public static void InitStaticFields()
    {
        unitPrefab = PrefabManager.Instance.unitPrefab;
    }

    public void CreateInGarage(Garage garage) // no need to reload sliders here or text field - everything is done in GARAGE function
    {
        unit_counter++;
        gameObject.name = "Unit" + unit_counter;
        tag = TagConstants.unitTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
        GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.unitEnemiesResourcesBulletsLayer;

        currentState = unitIdleState;
        storage = ResourceManager.Instance.shtabReference;
        
        garage.AddFreshUnit(this);
        
        rb = GetComponent<Rigidbody2D>();

        ResourceManager.Instance.unitsList.Add(this);
        ResourceManager.Instance.avaliableUnits.Add(this);

        ResourceManager.Instance.CreateUnitAndAddElectricityNeedCount();
    }





    private void Death() // Reload here because dead unit maybe was working at shaft
    {
        MineShaft temp = null;

        if (home)
        {
            Garage newHome = home;
            home.RemoveUnit(this);
            ResourceManager.Instance.SetHomelessUnitOnDeadUnitPlace(newHome);

            if (newHome.isMenuOpened)
            {
                Garage.garageMenuReference.ReloadPanel(newHome);
            }

            if (workPlace)
            {
                temp = workPlace;
                workPlace.RemoveUnit(this);

                if (temp.isMenuOpened)
                {
                    MineShaft.shaftMenuReference.ReloadUnitSlider();
                }
            }
            else
            {
                ResourceManager.Instance.avaliableUnits.Remove(this);
            }
        }

        else 
        {
            ResourceManager.Instance.homelessUnits.Remove(this);
        }

        ResourceManager.Instance.unitsList.Remove(this);

        if (resource)
        {
            Destroy(resource.gameObject);
        }

        ReloadUnitManageMenu(temp);
        // No need for reloading buildings manage menu

        Destroy(gameObject);
        ResourceManager.Instance.DestroyUnitAndRemoveElectricityNeedCount();
    }







    private void ReloadUnitManageMenu(MineShaft shaft)
    {
        GameViewMenu.Instance.ReloadUnitManageMenuAfterUnitDeath(shaft);
    } 
   
    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        if (collider.gameObject.tag == TagConstants.shaftDispenserTag && destination == collider.gameObject.transform.position)
        {
            GetComponent<AIDestinationSetter>().target = null;
            isApproachShaft = true;
        }

        if (collider.gameObject.tag == TagConstants.baseStorageTag && destination == collider.gameObject.transform.position)
        {
            GetComponent<AIDestinationSetter>().target = null;
            isApproachStorage = true;
        }

        if (collider.gameObject.tag == TagConstants.garageAngarTag && destination == collider.gameObject.transform.position)
        {
            GetComponent<AIDestinationSetter>().target = null;
            Debug.Log("IDLE");
            isApproachHome = true;
        }



        // Sets model unplacable
        if (collider.gameObject.tag == TagConstants.modelTag)
        {
            // Debug.Log("Unit intersects model!");
            GameHendler.Instance.buildingModel.isModelPlacable = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider) // For model correct placing
    {
        if (collider.gameObject.tag == TagConstants.modelTag)
        {
            // Debug.Log("Unit leaves model!");
            GameHendler.Instance.buildingModel.isModelPlacable = true;
            GameHendler.Instance.buildingModel.ChechForCorrectPlacement();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) // resource collision
    {
        if (collision.gameObject.tag == TagConstants.resourceTag && collision.gameObject == resource.gameObject) // correct resource
        {
            // Joint Logic
            Vector3 myVector = transform.position - collision.transform.position;
            collision.gameObject.AddComponent<HingeJoint2D>();
            collision.gameObject.GetComponent<HingeJoint2D>().connectedBody = rb;
            collision.gameObject.GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
            collision.gameObject.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0,0);
            collision.gameObject.GetComponent<HingeJoint2D>().anchor = new Vector2(myVector.x*4, myVector.y*4);

            isGatheringComplete = true;

            collision.gameObject.GetComponent<CircleCollider2D>().isTrigger = true; // to make resource go through other units
        }
    }
}