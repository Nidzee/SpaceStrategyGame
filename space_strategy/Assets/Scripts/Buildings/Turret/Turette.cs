using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Turette : AliveGameUnit, IBuilding
{
    // Events
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public delegate void TurretDestroy(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};
    public event TurretDestroy OnTurretDestroyed = delegate{};


    // Turret data
    public TurretSavingData savingData      = null;

    public int rotationBuilding             = 0;
    public float rotationCenter             = 0f;
    public float rotationCenterW            = 0f;
    public GameObject _tileOccupied         = null;
    public GameObject center;               // Init in inspector
    public Enemy target                     = null;
    public List<Enemy> enemiesInsideRange   = new List<Enemy>();
    
    public bool isCreated                   = false;
    public bool isFacingEnemy               = false;
    public bool isAttackState               = false;
    public bool isPowerON                   = false;
    public bool isMenuOpened                = false;
    public bool isTurnedInIdleMode          = true;
    
    public Quaternion idleRotation          = new Quaternion();
    public Quaternion targetRotation        = new Quaternion();
    public float coolDownTurnTimer          = 0f;
    public float upgradeTimer               = 0f;    
    public int level                        = 0;
    public int type                         = 0;
    public TurretCombatState combatState    = new TurretCombatState();
    public TurretIdleState idleState        = new TurretIdleState();
    public TurretPowerOffState powerOffState= new TurretPowerOffState();
    public ITurretState currentState        = null;


    // UI
    public GameObject canvas;           // Init in inspector
    public GameObject bars;             // Init in inspector
    public Slider healthBar;            // Init in inspector
    public Slider shieldhBar;           // Init in inspector
    public GameObject powerOffIndicator;// Init in inspector




    // Save logic
    public void SaveData()
    {
        savingData = new TurretSavingData();


        savingData.healthPoints = healthPoints;
        savingData.shieldPoints = shieldPoints;
        savingData.maxCurrentHealthPoints = maxCurrentHealthPoints;
        savingData.maxCurrentShieldPoints = maxCurrentShieldPoints;
        savingData.deffencePoints = deffencePoints;
        savingData.isShieldOn = isShieldOn;
        savingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;

        savingData.name = name;
        savingData.positionAndOccupationTileName = _tileOccupied.name;
        savingData.rotationBuilding = rotationBuilding;
        savingData.rotationCenter = center.transform.rotation.z;
        savingData.rotationCenterW = center.transform.rotation.w;
        savingData.type = type;
        savingData.level = level;
        savingData.isPowerOn = isPowerON;
        savingData.upgradeTimer = upgradeTimer;


        GameHendler.Instance.turretsSaved.Add(savingData);
    }



    // Upgrade logic after SHTAB upgrade - CORRECT
    public void InitStatsAfterBaseUpgrade()
    {
        int newHealth = 0;
        int newShield = 0;
        int newDefense = 0;

        switch(type)
        {
            case 1:
            InitStatsAfterBaseUpgrade_LT(out newHealth, out newShield, out newDefense);
            break;

            case 2:
            InitStatsAfterBaseUpgrade_MT(out newHealth, out newShield, out newDefense);
            break;
        }

        UpgradeStats(newHealth, newShield, newDefense);
        OnDamageTaken(this); // KOSTUL'
    }

    public void InitStatsAfterBaseUpgrade_LT(out int newHealth, out int newShield, out int newDefense)
    {
        newHealth = 0;
        newShield = 0;
        newDefense = 0;
        
        switch (level)
        {
            case 1:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl1_LaserTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl1_LaserTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl1_LaserTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl1_LaserTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl1_LaserTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl1_LaserTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl1_LaserTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl1_LaserTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl1_LaserTurret_Base_Lvl_3;
                break;
            }
            break;

            case 2:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_3;
                break;
            }
            break;


            case 3:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl3_LaserTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl3_LaserTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl3_LaserTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl3_LaserTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl3_LaserTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl3_LaserTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl3_LaserTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl3_LaserTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl3_LaserTurret_Base_Lvl_3;
                break;
            }
            break;
        }
    }

    public void InitStatsAfterBaseUpgrade_MT(out int newHealth, out int newShield, out int newDefense)
    {
        newHealth = 0;
        newShield = 0;
        newDefense = 0;
        
        switch (level)
        {
            case 1:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl1_MisileTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl1_MisileTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl1_MisileTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl1_MisileTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl1_MisileTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl1_MisileTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl1_MisileTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl1_MisileTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl1_MisileTurret_Base_Lvl_3;
                break;
            }
            break;

            case 2:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl2_MisileTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl2_MisileTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl2_MisileTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl2_MisileTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl2_MisileTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl2_MisileTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl2_MisileTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl2_MisileTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl2_MisileTurret_Base_Lvl_3;
                break;
            }
            break;


            case 3:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_3;
                break;
            }
            break;
        }
    }



    // Upgrade logic - CORRECT
    public void StartUpgrade()
    {
        StartCoroutine(UpgradeLogic());
    }

    public IEnumerator UpgradeLogic()
    {
        while (upgradeTimer < 1)
        {
            upgradeTimer += StatsManager._timerStep_Turret * Time.deltaTime;

            if (isMenuOpened)
            {
                switch(level)
                {
                    case 1:
                    TurretStaticData.turretMenuReference.level2.fillAmount = upgradeTimer;
                    break;

                    case 2:
                    TurretStaticData.turretMenuReference.level3.fillAmount = upgradeTimer;
                    break;

                    case 3:
                    Debug.Log("Error! Didnt fin circle to reload!");
                    break;
                }
            }

            yield return null;
        }

        upgradeTimer = 0;

        TurretUpgrading();
    }

    private void TurretUpgrading()
    {
        Turette temp = null;

        switch (type)
        {
            case (int)TurretType.LaserTurret:
            {
                GameObject turretLaser = null;
                switch (level)
                {
                    case 1:
                    turretLaser = Instantiate(PrefabManager.Instance.doubleTuretteLaserPrefab, transform.position, transform.rotation);
                    turretLaser.GetComponent<TurretLaserDouble>().ConstructBuildingAfterUpgrade(this);
                    break;

                    case 2:
                    turretLaser = Instantiate(PrefabManager.Instance.tripleTuretteLaserPrefab, transform.position, transform.rotation);
                    turretLaser.GetComponent<TurretLaserTriple>().ConstructBuildingAfterUpgrade(this);
                    break;
                }
                // Sets center object rotation as previous turret
                turretLaser.gameObject.transform.GetChild(2).transform.rotation = gameObject.transform.GetChild(2).transform.rotation;
                temp = turretLaser.GetComponent<Turette>();
            }
            break;
            
            case (int)TurretType.MisileTurret:
            {
                GameObject turretMisile = null;
                switch (level)
                {
                    case 1:
                    turretMisile = Instantiate(PrefabManager.Instance.doubleturetteMisilePrefab, transform.position, transform.rotation);
                    turretMisile.GetComponent<TurretMisileDouble>().ConstructBuildingAfterUpgrade(this);
                    break;

                    case 2:
                    turretMisile = Instantiate(PrefabManager.Instance.truipleturetteMisilePrefab, transform.position, transform.rotation);
                    turretMisile.GetComponent<TurretMisileTriple>().ConstructBuildingAfterUpgrade(this);
                    break;
                }
                // Sets center object rotation as previous turret
                turretMisile.gameObject.transform.GetChild(2).transform.rotation = gameObject.transform.GetChild(2).transform.rotation;
                temp = turretMisile.GetComponent<Turette>();
            }
            break;
        }

        // If menu was opened for this turret reinitializing menu data with new turret
        if (isMenuOpened)
        {
            TurretStaticData.turretMenuReference.ReloadPanel(temp);
        }

        BuildingsManageMenu.Instance.ReplaceTurretScrollItem(this, temp);

        GameObject.Destroy(gameObject);
    }




    // Constructing building and destroying logic - CORRECT
    public virtual void ConstructBuilding(Model model)
    {
        // Data initialization
        level = 1;
        rotationBuilding = model.rotation;
        _tileOccupied = model.BTileZero;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        
        isCreated = true;
        isFacingEnemy = false;
        isAttackState = false;
        isMenuOpened = false;
        isTurnedInIdleMode = true;

        coolDownTurnTimer = 3f;
        
        // Init rest of Data
        InitData();

        // Resource manager manipulation
        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();

        isPowerON = ResourceManager.Instance.IsPowerOn();
        if (isPowerON)
        {
            currentState = idleState;
        }
        else
        {
            currentState = powerOffState;
        }
    }

    public void ConstructBuildingFromFile(TurretSavingData turretSavingData)
    {
        // Data initialization
        InitGameUnitFromFile(
        turretSavingData.healthPoints, 
        turretSavingData.maxCurrentHealthPoints,
        turretSavingData.shieldPoints,
        turretSavingData.maxCurrentShieldPoints,
        turretSavingData.deffencePoints,
        turretSavingData.isShieldOn,
        turretSavingData.shieldGeneratorInfluencers);
        name = turretSavingData.name;
        
        isCreated = true;
        isFacingEnemy = false;
        isAttackState = false;
        isMenuOpened = false;
        isTurnedInIdleMode = true;

        _tileOccupied = GameObject.Find(turretSavingData.positionAndOccupationTileName);
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        rotationBuilding = turretSavingData.rotationBuilding;
        type = turretSavingData.type;
        level = turretSavingData.level;
        upgradeTimer = turretSavingData.upgradeTimer;
        coolDownTurnTimer = 3f;
        isPowerON = turretSavingData.isPowerOn;
        if (isPowerON)
        {
            currentState = idleState;
        }
        else
        {
            currentState = powerOffState;
        }
        

        // Rest data initialization
        InitData();


        // Turning center into right direction
        center.transform.rotation = new Quaternion(0f, 0f, turretSavingData.rotationCenter, turretSavingData.rotationCenterW);
    }

    public virtual void DestroyBuilding()
    {
        // Close UI menu
        if (isMenuOpened)
        {
            TurretStaticData.turretMenuReference.ExitMenu();
        }


        // Map info deleting   
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;


        // Manipulations
        OnTurretDestroyed(this);
        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    public void InitTurretDataFromPreviousTurret(Turette previousTurret)
    {
        // Data initialization
        rotationBuilding = previousTurret.rotationBuilding;
        level = (previousTurret.level + 1);
        type = previousTurret.type;
        _tileOccupied = previousTurret._tileOccupied;
        name = previousTurret.name;
        
        isCreated = true;
        isFacingEnemy = false;
        isAttackState = false;
        isMenuOpened = false;
        isTurnedInIdleMode = true;

        coolDownTurnTimer = 3f;
        isPowerON = ResourceManager.Instance.IsPowerOn();
        if (isPowerON)
        {
            currentState = idleState;
        }
        else
        {
            currentState = powerOffState;
        }
    }

    public void InitData()
    {
        // UI
        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(true);
        powerOffIndicator.SetActive(false);
        bars.SetActive(false);

        
        
        // Events
        OnDamageTaken += TurretStaticData.turretMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += BuildingsManageMenu.Instance.ReloadHPSP;
        OnTurretDestroyed += BuildingsManageMenu.Instance.RemoveFromBuildingsMenu;




        // Building map info initialization
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = _tileOccupied.transform;
    }




    // Other functions - CORRECT
    public void TurnTurretOFF()
    {
        isPowerON = false;
    }

    public void TurnTurretON()
    {
        isPowerON = true;
    }
    
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("TurretMenu");

        TurretStaticData.turretMenuReference.ReloadPanel(this);
    }

    private void Update()
    {
        if (isCreated)
        {
            currentState = currentState.DoState(this);
        }

        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     if (name == "LT1")
        //     {
        //         TakeDamage(10);
        //     }
        // }
    }

    public virtual void ResetCombatMode(){}
 
    public virtual void Attack(){}





    // Damage logic functions - CORRECT (maybe replace getComponent in damage logic with static EnemyDamage variable)
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);

        if (healthPoints <= 0)
        {
            DestroyBuilding();
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
}

public enum TurretType
{
    LaserTurret = 1,
    MisileTurret = 2
}