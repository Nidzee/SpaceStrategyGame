using UnityEngine;

public class Unit : AliveGameUnit
{
    private static int unit_counter = 0;
    public static GameObject unitPrefab;  // Unit sprite
    public static float moveSpeed = 1f;  // Const speed of all units

    public GameObject resourcePrefab;     // workplace.prefab - need for holding 
    public GameObject resource;           // reference for calculating
    
    private Rigidbody2D rb;

    #region Key waypoints
        public Base storage = null;       // static for all units
        public Garage home = null;        // Garage reference
        public MineShaft workPlace = null;   // Shaft reference
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



    private void Update()
    {        
        currentState = currentState.DoState(this);
        
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     // if (gameObject.name == "Unit1")
        //     // {
        //     //     Death();
        //     // }
        // }
    }

    public static void InitStaticFields()
    {
        unitPrefab = PrefabManager.Instance.unitPrefab;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void CreateInGarage(Garage garage) // no need to reload sliders here or text field - everything is done in GARAGE function
    {
        unit_counter++;

        gameObject.name = "Unit" + unit_counter;

        tag = TagConstants.unitTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
        GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.unitEnemiesResourcesBulletsLayer;

        GetComponent<Unit>().currentState = GetComponent<Unit>().unitIdleState;
        GetComponent<Unit>().storage = PrefabManager.Instance.shtab;
        GetComponent<Unit>().home = garage;

        garage.garageMembers.Add(GetComponent<Unit>());

        ResourceManager.Instance.unitsList.Add(GetComponent<Unit>());
        ResourceManager.Instance.avaliableUnits.Add(GetComponent<Unit>());
    }


    private void Death() // Reload here because dead unit maybe was working at shaft
    {
        MineShaft temp = null;

        if (home)
        {
            if (home.isMenuOpened)
            {
                home.RemoveUnit(this);
                Garage.garageMenuReference.ReloadUnitManage();
            }
            else
            {
                home.RemoveUnit(this);
            }

            if (workPlace)
            {
                temp = workPlace;

                if (workPlace.isMenuOpened)
                {
                    workPlace.RemoveUnit(this);
                    MineShaft.shaftMenuReference.ReloadUnitSlider();
                }
                else
                {
                    workPlace.RemoveUnit(this);
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


        if (GameHendler.Instance.isUnitManageMenuOpened)
        {
            // Because 1 unit died
            GameHendler.Instance.unitManageMenuReference.ReloadMainUnitCount();

            // If he was working
            if (temp)
            {
                if (GameHendler.Instance.isMenuAllResourcesTabOpened)
                {
                    ReloadMenuSlider(); // can be better
                }

                else
                {
                    switch (temp.type)
                    {
                        case 1:
                        GameHendler.Instance.unitManageMenuReference.FindSLiderAndReload(temp, 1);
                        break;

                        case 2:
                        GameHendler.Instance.unitManageMenuReference.FindSLiderAndReload(temp, 2);
                        break;

                        case 3:
                        GameHendler.Instance.unitManageMenuReference.FindSLiderAndReload(temp, 3);
                        break;
                    }
                }
            }
        }
        
        // Reload base menu button if it is opened and unit dies
        if (GameHendler.Instance.isBaseMenuOpened)
        {
            GameHendler.Instance.baseMenuReference.ReloadButtonManager();
        }

        Destroy(gameObject);
    }


    // Find out which type of shaft it is and reload that Slider
    public void ReloadMenuSlider()
    {
        GameHendler.Instance.unitManageMenuReference.ReloadCrystalSlider();   
        GameHendler.Instance.unitManageMenuReference.ReloadGelSlider();
        GameHendler.Instance.unitManageMenuReference.ReloadIronSlider();
    }


    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        if (collider.gameObject.tag == TagConstants.shaftDispenserTag && destination == collider.gameObject.transform.position)
        {
            //Debug.Log("I am near shaft!");
            isApproachShaft = true;
        }

        if (collider.gameObject.tag == TagConstants.baseStorageTag && destination == collider.gameObject.transform.position)
        {
            //Debug.Log("I am near storage!");
            isApproachStorage = true;
        }

        if (collider.gameObject.tag == TagConstants.garageAngarTag && destination == collider.gameObject.transform.position)
        {
            //Debug.Log("I am near home!");
            isApproachHome = true;
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

            //Debug.Log("Resource is attached!");
            isGatheringComplete = true;

            collision.gameObject.GetComponent<CircleCollider2D>().isTrigger = true; // to make resource go through other units
        }
    }
}