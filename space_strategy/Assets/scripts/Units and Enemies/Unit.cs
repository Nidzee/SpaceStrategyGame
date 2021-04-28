using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pathfinding;

public class Unit : AliveGameUnit
{
    private UnitSavingData unitSavingData = null;

    private Base storage        = null;        // Static for all units
    private Garage home         = null;        // Garage reference
    private MineShaft workPlace = null;        // Shaft reference
    // private Vector3 destination;

    public bool isApproachShaft     = false;
    public bool isApproachStorage   = false;
    public bool isApproachHome      = false;
    public bool isGatheringComplete = false;
    public bool isHomeChanged       = false;
    public bool isAtGarage          = false;

    public UnitIdleState idleState                       = new UnitIdleState();
    public UnitMovingState movingState                   = new UnitMovingState();
    public UnitNoSignalState noSignalState               = new UnitNoSignalState();
    public UnitResourceLeavingState resourceLeavingState = new UnitResourceLeavingState();
    public UnitExtractingState extractingState           = new UnitExtractingState();
    public IUnitState currentState                       = null;


    // Particular Unit data
    public int currentStateID       = 0;
    public Rigidbody2D rigidBodyRef = null;
    public Seeker seeker            = null;
    public Path path                = null;
    public int currentWaypoint      = 0;
    public int ID                   = 0;
    public GameObject resource      = null;
    public int resourceType         = 0;


    // UI
    public GameObject canvas;           // Init in inspector
    public GameObject bars;             // Init in inspector
    public Slider healthBar;            // Init in inspector
    public Slider shieldhBar;           // Init in inspector
    public GameObject powerOffIndicator;// Init in inspector


    // Properties
    public GameObject Target   {get {return target;}}
    public Garage Home         { get {return home;}     set {home = value;}}
    public MineShaft WorkPlace { get {return workPlace;}set {workPlace = value;}}
    public Base Storage        { get {return storage;}}
    

    private GameObject target = null;


    
    public void SaveData()
    {
        unitSavingData = new UnitSavingData();

        unitSavingData.position_x = gameObject.transform.position.x;
        unitSavingData.position_y = gameObject.transform.position.y;
        unitSavingData.position_z = gameObject.transform.position.z;

        unitSavingData.name = this.name;
        unitSavingData.ID = this.ID;
        unitSavingData.isShieldOn = this.isShieldOn;
        unitSavingData.shieldPoints = this.shieldPoints;
        unitSavingData.healthPoints = this.healthPoints;
        unitSavingData.maxCurrentHealthPoints = this.maxCurrentHealthPoints;
        unitSavingData.maxCurrentShieldPoints = this.maxCurrentShieldPoints;

        unitSavingData.isGatheringComplete = false;

        if (resource)
        {
            unitSavingData.resourceType = resourceType;
        
            unitSavingData.resourcePosition_x = resource.transform.position.x;
            unitSavingData.resourcePosition_y = resource.transform.position.y;
            unitSavingData.resourcePosition_z = resource.transform.position.z;
        }

        if (currentState == idleState)
        {
            unitSavingData.currentStateID = (int)UnitStates.UnitIdleState;
        }

        else if (currentState == movingState)
        {
            unitSavingData.currentStateID = (int)UnitStates.UnitMovingState;
        }

        else if (currentState == extractingState)
        {
            unitSavingData.currentStateID = (int)UnitStates.UnitExtractingState;
            unitSavingData.resourceType = -1; // Means this will not create resource    
        }

        else if (currentState == resourceLeavingState)
        {
            unitSavingData.currentStateID = (int)UnitStates.UnitResourceLeavingState;
        }

        else if (currentState == noSignalState)
        {
            unitSavingData.currentStateID = (int)UnitStates.UnitNoSignalState;
        }



        // if (destination != null)
        // {
        //     unitSavingData.destination_x = destination.x;
        //     unitSavingData.destination_y = destination.y;
        //     unitSavingData.destination_z = destination.z;
        // }

        if (target != null)
        {
            unitSavingData.targetObjectTransformName = target.gameObject.transform.parent.name;
        }
        
        GameHendler.Instance.unitsSaved.Add(unitSavingData);
    }

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);

        if (healthPoints <= 0)
        {
            DestroyUnit();
            return;
        }

        bars.SetActive(true);

        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        StopCoroutine("UICanvasmaintaining");
        uiCanvasDissapearingTimer = 0f;
        StartCoroutine("UICanvasmaintaining");
    }

    IEnumerator UICanvasmaintaining()
    {
        while (uiCanvasDissapearingTimer < 3)
        {
            uiCanvasDissapearingTimer += Time.deltaTime;
            yield return null;
        }
        uiCanvasDissapearingTimer = 0;
        
        bars.SetActive(false);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState = currentState.DoState(this);
        }
    }


    void OnTriggerEnter2D(Collider2D collider)     // or ShaftRadius or SkladRadius or HomeRadius or Model
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
        
        if (collider.gameObject.tag == TagConstants.shaftDispenserTag && target == collider.gameObject)
        {
            isApproachShaft = true;
        }

        if (collider.gameObject.tag == TagConstants.baseStorageTag && target == collider.gameObject)
        {
            isApproachStorage = true;
        }

        if (collider.gameObject.tag == TagConstants.garageAngarTag && target == collider.gameObject)
        {
            isApproachHome = true;
        }

        if (home)
        {
            if (collider.gameObject.tag == TagConstants.garageAngarTag && home.angar == collider.gameObject)
            {
                isAtGarage = true;
            }
        }

        // Sets model unplacable
        if (collider.gameObject.tag == TagConstants.modelTag)
        {
            Debug.Log("Intersects with UNIT");
            GameHendler.Instance.buildingModel.isModelPlacable = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider)      // For model correct placing
    {
        if (collider.gameObject.tag == TagConstants.modelTag)
        {
            Debug.Log("STOP intersects with UNIT");

            GameHendler.Instance.buildingModel.isModelPlacable = true;
            GameHendler.Instance.buildingModel.ChechForCorrectPlacement();
        }

        if (home)
        {
            if (home.angar == collider.gameObject)
            {
                Debug.Log("Left home angar!");
                isAtGarage = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) // Resource collision
    {
        if (collision.gameObject.tag == TagConstants.resourceTag && collision.gameObject == resource.gameObject) // correct resource
        {
            // Resource joint Logic
            Vector3 myVector = transform.position - collision.transform.position;
            collision.gameObject.AddComponent<HingeJoint2D>();
            var temp = collision.gameObject.GetComponent<HingeJoint2D>();

            temp.connectedBody = rigidBodyRef;
            temp.autoConfigureConnectedAnchor = false;
            temp.connectedAnchor = new Vector2(0,0);
            temp.anchor = new Vector2(myVector.x*4, myVector.y*4);

            isGatheringComplete = true;

            collision.gameObject.GetComponent<CircleCollider2D>().isTrigger = true; // to make resource go through other units
        }
    }



    // Creating and destroying logic
    public void CreateInGarage(Garage garage) // no need to reload sliders here or text field - everything is done in GARAGE function
    {
        // Data initialization
        CreateGameUnit(StatsManager.maxUnit_Health, StatsManager.maxUnit_Shield, StatsManager.maxUnit_defense);
        ID = UnitStaticData.unit_counter;
        name = "U" + UnitStaticData.unit_counter;
        UnitStaticData.unit_counter++;
        currentState = idleState;
        storage = ResourceManager.Instance.shtabReference;
        garage.AddCreatedByButtonUnit(this);
        seeker = GetComponent<Seeker>();
        rigidBodyRef = GetComponent<Rigidbody2D>();


        // UI initialization
        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(true);
        powerOffIndicator.SetActive(false);
        bars.SetActive(false);


        // Resource manager lists maintaining
        ResourceManager.Instance.unitsList.Add(this);
        ResourceManager.Instance.avaliableUnits.Add(this);
        ResourceManager.Instance.CreateUnitAndAddElectricityNeedCount();
    }

    public void CreateUnitFromFile(UnitSavingData savingData)
    {
        // Data initialization
        InitGameUnitFromFile(
        savingData.healthPoints, 
        savingData.maxCurrentHealthPoints,
        savingData.shieldPoints,
        savingData.maxCurrentShieldPoints,
        savingData.deffencePoints,
        savingData.isShieldOn,
        savingData.shieldGeneratorInfluencers);

        ID = savingData.ID;
        name = savingData.name;
        currentStateID = savingData.currentStateID;
        storage = ResourceManager.Instance.shtabReference;
        seeker = GetComponent<Seeker>();
        rigidBodyRef = GetComponent<Rigidbody2D>();
    
    
        // Check if resource exist
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
            // Resource joint logic
            Vector3 myVector = transform.position - resourceFromFile.transform.position;
            resourceFromFile.gameObject.AddComponent<HingeJoint2D>();
            HingeJoint2D jointRef = resourceFromFile.gameObject.GetComponent<HingeJoint2D>();
            jointRef.connectedBody = rigidBodyRef;
            jointRef.autoConfigureConnectedAnchor = false;
            jointRef.connectedAnchor = new Vector2(0,0);
            jointRef.anchor = new Vector2(myVector.x*4, myVector.y*4);
            resourceFromFile.gameObject.GetComponent<CircleCollider2D>().isTrigger = true; // to make resource go through other units
            resource = resourceFromFile;
            resourceType = savingData.resourceType;
        }


        // Destination set logic
        if (savingData.targetObjectTransformName != null)
        {
            // Sets target to child object of Shaft(dispenser), Base(storage) or Garage(angar)
            target = GameObject.Find(savingData.targetObjectTransformName).transform.GetChild(0).gameObject;
        }


        // UI
        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(true);
        powerOffIndicator.SetActive(false);
        bars.SetActive(false);


        // Resource manager lists maintaining
        ResourceManager.Instance.unitsList.Add(this);
        ResourceManager.Instance.homelessUnits.Add(this);
        ResourceManager.Instance.avaliableUnits.Add(this);
    }

    private void DestroyUnit() // Reload here because dead unit maybe was working at shaft
    {
        DestroyUnitData();

        GameViewMenu.Instance.ReloadMainUnitCount(); // Event

        Destroy(gameObject);

        ResourceManager.Instance.DestroyUnitAndRemoveElectricityNeedCount();
    }

    public void DestroyUnitData()
    {
        if (home)
        {
            Garage newHome = home;
            home.RemoveDeadUnitFromGarage(this); // Executes Event
            ResourceManager.Instance.SetHomelessUnitOnDeadUnitPlace(newHome); // Executes Event

            if (workPlace)
            {
                workPlace.RemoveUnit(this); // Executes Event
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
            GameObject.Destroy(resource.gameObject);
        }
    }


    // A* Path maintaining logic
    public void RebuildPath()
    {
        path = null;
        
        if (target != null)
        {
            seeker.StartPath(transform.position, target.transform.position, OnPathBuilded);
        }
    }

    private void OnPathBuilded(Path newPath)/////////////////////////////////////////////////////////////////////////
    {
        if (!newPath.error)
        {
            path = newPath;
            currentWaypoint = 0;
        }
        // Get current hex with unit position
        // Check distance between tile on standing and first point in path
        // logic
        // c = pixel_to_pointy_hex(redPoint.transform.position.x, redPoint.transform.position.y);
        // CurrentHex = GameObject.Find(c.q + "." +c.r + "." + c.s);
    }

    public void ChangeDestination(int destinationID)
    {
        switch (destinationID)
        {
            case (int)UnitDestinationID.Home :
            target = home.GetUnitDestination();
            break;

            case (int)UnitDestinationID.WorkPlace :
            target = workPlace.GetUnitDestination();
            break;

            case (int)UnitDestinationID.Storage :
            target = storage.GetUnitDestination();
            break;

            case (int)UnitDestinationID.Null :
            target = null;
            isAtGarage = false;
            isApproachHome = false;
            isApproachStorage = false;
            isApproachShaft = false;
            rigidBodyRef.velocity = Vector2.zero;
            break;
        }
    }
}