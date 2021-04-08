using UnityEngine;
using Pathfinding;

public class Unit : AliveGameUnit
{
    public UnitData unitData;
    public UnitSavingData unitSavingData;

    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void UnitDestroy(AliveGameUnit gameUnit);
    public event UnitDestroy OnUnitDestroyed = delegate{};



    public Seeker _seeker = null;
    public Path _path = null;
    public int _currentWaypoint = 0;

    public Rigidbody2D rb;


    public Vector3 Destination
    {
        get
        {
            return unitData.destination;
        }
    }

    public Garage Home
    {
        get
        {
            return unitData.home;
        }
        set
        {
            unitData.home = value;
        }
    }

    public MineShaft WorkPlace
    {
        get
        {
            return unitData.workPlace;
        }
        set
        {
            unitData.workPlace = value;
        }
    }


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

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);

        if (healthPoints <= 0)
        {
            DestroyUnit();
            return;
        }

        OnDamageTaken(this);
    }



































    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (name == "U0")
            {
                unitSavingData = new UnitSavingData();

                unitSavingData.position_x = gameObject.transform.position.x;
                unitSavingData.position_y = gameObject.transform.position.y;
                unitSavingData.position_z = gameObject.transform.position.z;

                unitSavingData.name = this.name;
                unitSavingData.ID = this.unitData.ID;
                unitSavingData.isShieldOn = this.isShieldOn;
                unitSavingData.shieldPoints = this.shieldPoints;
                unitSavingData.healthPoints = this.healthPoints;
                unitSavingData.maxCurrentHealthPoints = this.maxCurrentHealthPoints;
                unitSavingData.maxCurrentShieldPoints = this.maxCurrentShieldPoints;

                // unitSavingData.isApproachShaft = false;
                // unitSavingData.isApproachStorage = false;
                // unitSavingData.isApproachHome = false;
                unitSavingData.isGatheringComplete = false;

                if (unitData.resource)
                {
                    unitSavingData.resourceType = unitData.resourceType;
                
                    unitSavingData.resourcePosition_x = unitData.resource.transform.position.x;
                    unitSavingData.resourcePosition_y = unitData.resource.transform.position.y;
                    unitSavingData.resourcePosition_z = unitData.resource.transform.position.z;
                }

                if (unitData.currentState == unitData.unitIdleState)
                {
                    unitSavingData.currentState_ID = (int)UnitStates.UnitIdleState;
                }

                else if (unitData.currentState == unitData.unitIGoToState)
                {
                    unitSavingData.currentState_ID = (int)UnitStates.UnitIGoToState;
                }

                else if (unitData.currentState == unitData.unitIGatherState)
                {
                    unitSavingData.currentState_ID = (int)UnitStates.UnitIGatherState;
                    unitSavingData.resourceType = -1; // Means this will not create resource    
                }

                else if (unitData.currentState == unitData.unitResourceLeavingState)
                {
                    unitSavingData.currentState_ID = (int)UnitStates.UnitResourceLeavingState;
                }

                else if (unitData.currentState == unitData.unitIHomelessState)
                {
                    unitSavingData.currentState_ID = (int)UnitStates.UnitIHomelessState;
                }



                if (unitData.destination != null)
                {
                    unitSavingData.destination_x = unitData.destination.x;
                    unitSavingData.destination_y = unitData.destination.y;
                    unitSavingData.destination_z = unitData.destination.z;
                }

                if (GetComponent<AIDestinationSetter>().target != null)
                    unitSavingData.targetObjectTransformName = GetComponent<AIDestinationSetter>().target.gameObject.name;



                GameHendler.Instance.unitSavingData = this.unitSavingData;



                ResourceManager.Instance.unitsList.Remove(this);
                ResourceManager.Instance.homelessUnits.Remove(this);
                ResourceManager.Instance.avaliableUnits.Remove(this);

                if (unitData.resource)
                {
                    Destroy(unitData.resource.gameObject);
                }
                Destroy(gameObject);
            }
        }

        unitData.LifeCycle(this);
    }

    public void SaveUnitData()
    {
        
    }

    public void CreateUnitFromFile(UnitSavingData savingData)
    {
        name = savingData.name;

        InitGameUnitFromFile(
        savingData.healthPoints, 
        savingData.maxCurrentHealthPoints,
        savingData.shieldPoints,
        savingData.maxCurrentShieldPoints,
        savingData.deffencePoints,
        savingData.isShieldOn,
        savingData.shieldGeneratorInfluencers);

        _seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        _path = null;
        _currentWaypoint = 0;

        unitData = new UnitData();
        unitData.InitNewData(this);
        unitData.InitUnitFromFile(savingData);


        
        ResourceManager.Instance.unitsList.Add(this);
        ResourceManager.Instance.homelessUnits.Add(this);
        ResourceManager.Instance.avaliableUnits.Add(this);
    }






















































    public void ChangeDestination(int destinationID)
    {
        unitData.ChangeDestination(destinationID);
    }

    public void CreateInGarage(Garage garage) // no need to reload sliders here or text field - everything is done in GARAGE function
    {
        CreateGameUnit(StatsManager.maxUnit_Health, StatsManager.maxUnit_Shield, StatsManager.maxUnit_defense);
        unitData = new UnitData();

        unitData.CreateUnit(garage, this);


        _seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();


        ResourceManager.Instance.unitsList.Add(this);
        ResourceManager.Instance.avaliableUnits.Add(this);

        ResourceManager.Instance.CreateUnitAndAddElectricityNeedCount();
    }

    // Add intermidiary calculations - unit work and home = null
    private void DestroyUnit() // Reload here because dead unit maybe was working at shaft
    {
        unitData.DestroyUnit();


        Destroy(gameObject);
        ResourceManager.Instance.DestroyUnitAndRemoveElectricityNeedCount();
    }

    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
        
        unitData.OnTriggerEnter(collider);
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
        unitData.OnCollisionEnter(collision);
    }
}