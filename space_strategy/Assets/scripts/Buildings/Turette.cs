using UnityEngine;

public class Turette : AliveGameUnit, IBuilding
{
    // public GameUnit gameUnit;
    public TurretData turretData;

    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void TurretDestroy(AliveGameUnit gameUnit);
    public event TurretDestroy OnTurretDestroyed = delegate{};


    public void InitStatsAfterShtabUpgrade()
    {
        Debug.Log("InitStatsAfterShtabUpgrade");
        int newHealth = 0;
        int newShield = 0;
        int newDefense = 0;

        turretData.UpgradeStatsAfterShtabUpgrade(out newHealth, out newShield, out newDefense);

        UpgradeStats(newHealth, newShield, newDefense);

        OnDamageTaken(this);
    }

    private void Update()
    {
        if (turretData != null)
        turretData.TurretLifeCycle();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (name == "TL1")
            {
                TakeDamage(10);
                // DestroyBuilding();
            }
        }

        // if (isCreated)
        // {
        //     if (isPowerON)
        //     {
        //         if (attackState)
        //         {
        //             CombatMode();
        //         }
        //         else
        //         {
        //             IdleMode();
        //         }
        //     }

        //     else
        //     {
        //         NoElectricityMode();
        //     }
        // }
    }

    public void StartUpgrade()
    {
        turretData.StartUpgrade();
    }

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);

        if (healthPoints <= 0)
        {
            DestroyBuilding();
            return;
        }

        OnDamageTaken(this);
    }

    public virtual void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("TurretMenu");

        TurretStaticData.turretMenuReference.ReloadPanel(this);
    }

    public override void UpgradeStats(int newHealth, int NewShield, int newDefense)
    {
        base.UpgradeStats(newHealth, NewShield, newDefense);
    }

    public virtual void ConstructBuilding(Model model)
    {
        turretData.ConstructBuilding(model);



        
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = model.BTileZero.transform;




        // Add events here or in classes children
        OnDamageTaken += TurretStaticData.turretMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        // OnUpgraded += TurretStaticData.turretMenuReference.ReloadTurretLevelVisuals; // update buttons and visuals
        OnTurretDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;

        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.turretRange;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer); // Means that it is noninteractible
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretRangeLayer;

            turretData.HelperObjectInit(gameObject.transform.GetChild(1).gameObject);
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }


        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
        AstarPath.active.Scan();
    }

    public virtual void DestroyBuilding()
    {
        turretData.DestroyBuilding();

        // Execute events here or in classes children
        OnTurretDestroyed(this);

        // ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }
















    public virtual void ResetCombatMode(){}
 
    public virtual void Attack(){}


    // private void IdleMode()
    // {
    //     RandomIdleTurn();
    // }

    // private void CombatMode()
    // {
    //     TurnTowardsEnemy();
        
    //     if (isFacingEnemy)
    //     {
    //         Attack();
    //     }
    // }

    // private void NoElectricityMode()
    // {
    //     Debug.Log("I have no power!");
    // }

    // private void RandomIdleTurn()
    // {
    //     if (isTurnedInIdleMode)
    //     {
    //         coolDownTurnTimer -= Time.deltaTime;
    //         if (coolDownTurnTimer < 0)
    //         {
    //             coolDownTurnTimer = 5f;
    //             isTurnedInIdleMode = false;
    //             idleRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, -360f)));
    //         }
    //     }
    //     else
    //     {
    //         center.transform.rotation = Quaternion.RotateTowards(center.transform.rotation, idleRotation, turnSpeed * Time.deltaTime);

    //         if (center.transform.rotation == idleRotation)
    //         {
    //             isTurnedInIdleMode = true;
    //         }
    //     }
    // }

    // // Turning turret logic - correct!
    // private void TurnTowardsEnemy()
    // {
    //     if (target)
    //     {
    //         Vector3 targetPosition = target.transform.position;
    //         targetPosition.z = 0f;
    
    //         Vector3 turretPos = center.transform.position;
    //         targetPosition.x = targetPosition.x - turretPos.x;
    //         targetPosition.y = targetPosition.y - turretPos.y;
    
    //         float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

    //         targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    //         center.transform.rotation = Quaternion.RotateTowards(center.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

    //         if (center.transform.rotation == targetRotation && !isFacingEnemy)
    //         {
    //             isFacingEnemy = true;
    //         }
    //     }
    // }

    // Every turret has its own attack pattern
   
}
