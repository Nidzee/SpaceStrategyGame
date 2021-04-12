using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pathfinding;

public class Unit : AliveGameUnit
{
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};
    public delegate void UnitDestroy(AliveGameUnit gameUnit);
    public event UnitDestroy OnUnitDestroyed = delegate{};
    private UnitSavingData unitSavingData;

    #region Key waypoints
        public Base storage;        // Static for all units
        private Garage home;        // Garage reference
        private MineShaft workPlace;// Shaft reference
        private Vector3 destination;
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
    public Seeker _seeker = null;
    public Path _path = null;
    public int _currentWaypoint = 0;
    public Rigidbody2D _rb;

    public Vector3 Destination { get {return destination;}}
    public Garage Home         { get {return home;}         set { home = value;} }
    public MineShaft WorkPlace { get {return workPlace;}    set {workPlace = value;} }
    
    public int ID;
    public GameObject resource;
    public int resourceType;



    public GameObject canvas;
    public GameObject bars;
    public Slider healthBar; 
    public Slider shieldhBar;
    public GameObject powerOffIndicator;



    public void RebuildPath()
    {
        _path = null;
        
        if (GetComponent<AIDestinationSetter>().target != null)
        {
            _seeker.StartPath(transform.position, GetComponent<AIDestinationSetter>().target.position, OnPathBuilded);
        }
    }

    private void OnPathBuilded(Path path)
    {
        if (!path.error)
        {
            _path = path;
            _currentWaypoint = 0;
        }
    }

    public void ChangeDestination(int destinationID)
    {
        switch (destinationID)
        {
            case (int)UnitDestinationID.Home :
            GetComponent<AIDestinationSetter>().target = home.GetUnitDestination();
            destination = home.GetUnitDestination().position;
            break;

            case (int)UnitDestinationID.WorkPlace :
            GetComponent<AIDestinationSetter>().target = workPlace.GetUnitDestination();
            destination = workPlace.GetUnitDestination().position;
            break;

            case (int)UnitDestinationID.Storage :
            GetComponent<AIDestinationSetter>().target = storage.GetUnitDestination();
            destination = storage.GetUnitDestination().position;
            break;

            case (int)UnitDestinationID.Null :
            GetComponent<AIDestinationSetter>().target = null;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            destination = transform.position;
            break;
        }
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

        OnDamageTaken(this);
    }

    float uiCanvasDissapearingTimer = 0f;
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
        currentState = currentState.DoState(this);

        if (name == "U0" &&(Input.GetKeyDown(KeyCode.K)))
        {
            TakeDamage(10);
        }
    }

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

        if (currentState == unitIdleState)
        {
            unitSavingData.currentState_ID = (int)UnitStates.UnitIdleState;
        }

        else if (currentState == unitIGoToState)
        {
            unitSavingData.currentState_ID = (int)UnitStates.UnitIGoToState;
        }

        else if (currentState == unitIGatherState)
        {
            unitSavingData.currentState_ID = (int)UnitStates.UnitIGatherState;
            unitSavingData.resourceType = -1; // Means this will not create resource    
        }

        else if (currentState == unitResourceLeavingState)
        {
            unitSavingData.currentState_ID = (int)UnitStates.UnitResourceLeavingState;
        }

        else if (currentState == unitIHomelessState)
        {
            unitSavingData.currentState_ID = (int)UnitStates.UnitIHomelessState;
        }



        if (destination != null)
        {
            unitSavingData.destination_x = destination.x;
            unitSavingData.destination_y = destination.y;
            unitSavingData.destination_z = destination.z;
        }

        if (GetComponent<AIDestinationSetter>().target != null)
            unitSavingData.targetObjectTransformName = GetComponent<AIDestinationSetter>().target.gameObject.name;

        

        if (resource)
        {
            Destroy(resource.gameObject);
        }


        GameHendler.Instance.unitsSaved.Add(unitSavingData);

        // Destroy(gameObject);
    }





    public void CreateInGarage(Garage garage) // no need to reload sliders here or text field - everything is done in GARAGE function
    {
        CreateGameUnit(StatsManager.maxUnit_Health, StatsManager.maxUnit_Shield, StatsManager.maxUnit_defense);


        ID = UnitStaticData.unit_counter;
        gameObject.name = "U" + UnitStaticData.unit_counter;
        UnitStaticData.unit_counter++;
        
        tag = TagConstants.unitTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
        GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.unitEnemiesResourcesBulletsLayer;

        currentState = unitIdleState;
        storage = ResourceManager.Instance.shtabReference;
        garage.AddCreatedByButtonUnit(this);
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        _currentWaypoint = 0;


        
        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        canvas.SetActive(true);
        powerOffIndicator.SetActive(false);
        bars.SetActive(false);



        ResourceManager.Instance.unitsList.Add(this);
        ResourceManager.Instance.avaliableUnits.Add(this);

        ResourceManager.Instance.CreateUnitAndAddElectricityNeedCount();
    }

    public void CreateUnitFromFile(UnitSavingData savingData)
    {
        storage = ResourceManager.Instance.shtabReference;
        home = null;
        workPlace = null;

        isApproachShaft = false;
        isApproachStorage = false;
        isApproachHome = false;
        isGatheringComplete = false;

        unitIdleState = new UnitIdleState();
        unitIGoToState = new UnitIGoToState();
        unitIHomelessState = new UnitIHomelessState();
        unitResourceLeavingState = new UnitResourceLeavingState();
        unitIGatherState = new UnitIGatherState();
        currentState = null;
        currentState_ID = savingData.currentState_ID;

        _seeker = GetComponent<Seeker>();
        _path = null;
        _currentWaypoint = 0;
        _rb = GetComponent<Rigidbody2D>();

        resource = null;
        resourceType = 0;

        

        gameObject.name = savingData.name;
        name = savingData.name;
        tag = TagConstants.unitTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
        GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.unitEnemiesResourcesBulletsLayer;



        InitGameUnitFromFile(
        savingData.healthPoints, 
        savingData.maxCurrentHealthPoints,
        savingData.shieldPoints,
        savingData.maxCurrentShieldPoints,
        savingData.deffencePoints,
        savingData.isShieldOn,
        savingData.shieldGeneratorInfluencers);


        
        InitUnitFromFile(savingData);




        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        canvas.SetActive(true);
        powerOffIndicator.SetActive(false);
        bars.SetActive(false);


        ResourceManager.Instance.unitsList.Add(this);
        ResourceManager.Instance.homelessUnits.Add(this);
        ResourceManager.Instance.avaliableUnits.Add(this);
    }

    public void InitUnitFromFile(UnitSavingData savingData)
    {
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
            Vector3 myVector = transform.position - resourceFromFile.transform.position;
            resourceFromFile.gameObject.AddComponent<HingeJoint2D>();
            HingeJoint2D jointRef = resourceFromFile.gameObject.GetComponent<HingeJoint2D>();
            jointRef.connectedBody = _rb;
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
            GetComponent<AIDestinationSetter>().target = GameObject.Find(savingData.targetObjectTransformName).transform;

    }

    private void DestroyUnit() // Reload here because dead unit maybe was working at shaft
    {
        DestroyUnitData();

        Destroy(gameObject);
        ResourceManager.Instance.DestroyUnitAndRemoveElectricityNeedCount();
    }

    public void DestroyUnitData()
    {
        MineShaft temp = null;

        if (home)
        {
            Garage newHome = home;
            home.RemoveUnit(this); // Executes Event
            ResourceManager.Instance.SetHomelessUnitOnDeadUnitPlace(newHome); // Executes Event

            if (workPlace)
            {
                temp = workPlace;
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
        // No need for reloading buildings manage menu





        //REDO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        GameViewMenu.Instance.ReloadUnitManageMenuAfterUnitDeath(temp);
    }




    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius or Model
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
        
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
            isApproachHome = true;
        }


        // Sets model unplacable
        if (collider.gameObject.tag == TagConstants.modelTag)
        {
            GameHendler.Instance.buildingModel.isModelPlacable = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider) // For model correct placing
    {
        if (collider.gameObject.tag == TagConstants.modelTag)
        {
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
            var temp = collision.gameObject.GetComponent<HingeJoint2D>();

            temp.connectedBody = _rb;
            temp.autoConfigureConnectedAnchor = false;
            temp.connectedAnchor = new Vector2(0,0);
            temp.anchor = new Vector2(myVector.x*4, myVector.y*4);

            isGatheringComplete = true;

            collision.gameObject.GetComponent<CircleCollider2D>().isTrigger = true; // to make resource go through other units
        }
    }
}