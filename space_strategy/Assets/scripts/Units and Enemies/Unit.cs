using UnityEngine;

public class Unit : AliveGameUnit
{
    public static GameObject unitPrefab;  // Unit sprite
    public static float moveSpeed = 1f;  // Const speed of all units

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

    public GameObject resourcePrefab;     // workplace.prefab - need for holding 
    public GameObject resource;           // reference for calculating
    
    private Rigidbody2D rb;


    public static void InitStaticFields()
    {
        unitPrefab = PrefabManager.Instance.unitPrefab;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = unitIdleState;
    }

    private void Update()
    {        
        currentState = currentState.DoState(this);
    }


    public void AddToJob(MineShaft workPlace)
    {
        this.workPlace = workPlace;
        ResourceManager.Instance.avaliableUnits.Remove(this);
    }

    public void RemoveFromJob(JobLostCode code)
    {
        workPlace = null;
        resourcePrefab = null;
        
        if (code == JobLostCode.ShaftDestroy || code == JobLostCode.Slider)
        {
            ResourceManager.Instance.avaliableUnits.Add(this); 
        }
        // else if garage destroy or death they are already homeless and dont need for avaliable list
    }

    public void AddToGarage(Garage home)
    {
        this.home = home;
        ResourceManager.Instance.homelessUnits.Remove(this);
        ResourceManager.Instance.avaliableUnits.Add(this);
    }

    public void RemoveFromGarage(HomeLostCode code)
    {
        home = null;

        if (code == HomeLostCode.GarageDestroy)
        {
            if (workPlace)
            {
                workPlace.RemoveHomelessUnit(this);
            }
            else
            {
                ResourceManager.Instance.avaliableUnits.Remove(this);
            }

            ResourceManager.Instance.homelessUnits.Add(this);
        }
    }


    private void Death() // REDO
    {
        if (home) // he can have job or not
        {
            home.RemoveDeadUnit(this);

            if (workPlace)
            {
                workPlace.RemoveDeadUnit(this);
            }
            else
            {
                ResourceManager.Instance.avaliableUnits.Remove(this);
            }
        }
        else // if he is homeless - he dont have job and home, he is not avaliable
        {
            ResourceManager.Instance.homelessUnits.Remove(this);
        }

        ResourceManager.Instance.unitsList.Remove(this);

        // Destruction logic (particles / animation)
    }



    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        if (collider.gameObject.tag == TagConstants.shaftDispenserTag && destination == collider.gameObject.transform.position)
        {
            Debug.Log("I am near shaft!");
            isApproachShaft = true;
        }

        if (collider.gameObject.tag == TagConstants.baseStorageTag && destination == collider.gameObject.transform.position)
        {
            Debug.Log("I am near storage!");
            isApproachStorage = true;
        }

        if (collider.gameObject.tag == TagConstants.garageAngarTag && destination == collider.gameObject.transform.position)
        {
            Debug.Log("I am near home!");
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

            Debug.Log("Resource is attached!");
            isGatheringComplete = true;
        }
    }
}

public enum JobLostCode
{
    Death,
    ShaftDestroy,
    GarageDestroy,
    Slider
}

public enum HomeLostCode
{
    Death,
    GarageDestroy,
}
