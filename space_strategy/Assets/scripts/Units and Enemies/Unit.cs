using UnityEngine;
using Pathfinding;

public class Unit : AliveGameUnit
{
    // public GameUnit gameUnit;
    public UnitData unitData;
    public UnitSavingData unitSavingData;

    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void UnitDestroy(AliveGameUnit gameUnit);
    public event UnitDestroy OnUnitDestroyed = delegate{};


    // public bool isPathInit = false;

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

    // Unit life cycle
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     if (name == "Unit - 1")
        //     {
        //         // DestroyUnit();
        //         // isPathInit = false;
        //     }
        // }

        // Debug.Log(unitData.currentState);

        unitData.LifeCycle(this);
    }

    // public void RebuildPath()
    // {
    //     isPathInit = false;
    // }

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