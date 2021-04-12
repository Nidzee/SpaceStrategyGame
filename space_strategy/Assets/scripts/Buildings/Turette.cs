using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Turette : AliveGameUnit, IBuilding
{
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};
    public delegate void TurretDestroy(AliveGameUnit gameUnit);
    public event TurretDestroy OnTurretDestroyed = delegate{};
    public TurretSavingData newSavingData = new TurretSavingData();
    public TurretSavingData savingData;

    public int ID;
    public int rotation_building;
    public float rotation_center;
    public float rotation_center_w;
    public GameObject _tileOccupied;
    public GameObject center;
    public Enemy target;
    public List<Enemy> enemiesInsideRange;
    public bool isCreated;
    public bool isFacingEnemy;
    public bool attackState;
    public bool isPowerON;
    public bool isMenuOpened;
    public bool isTurnedInIdleMode;
    public Quaternion idleRotation = new Quaternion();
    public Quaternion targetRotation = new Quaternion();
    public float coolDownTurnTimer;
    public float upgradeTimer;    
    public int level;
    public int type;
    public TurretCombatState combatState = new TurretCombatState();
    public TurretIdleState idleState = new TurretIdleState();
    public TurretPowerOffState powerOffState = new TurretPowerOffState();
    public ITurretState currentState = null;

    public GameObject canvas;
    public GameObject bars;
    public Slider healthBar; 
    public Slider shieldhBar;
    public GameObject powerOffIndicator;


    public void InitStatsAfterShtabUpgrade()
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

        OnDamageTaken(this);
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








    private void Update()
    {
        if (currentState != null)
        {
            currentState = currentState.DoState(this);
        }

        // if (name == "LT0" &&(Input.GetKeyDown(KeyCode.K)))
        // {
        //     TakeDamage(10);
        // }
    }

    public void SaveData()
    {
        savingData = new TurretSavingData();
                
        newSavingData.healthPoints = healthPoints;
        newSavingData.shieldPoints = shieldPoints;
        newSavingData.maxCurrentHealthPoints = maxCurrentHealthPoints;
        newSavingData.maxCurrentShieldPoints = maxCurrentShieldPoints;
        newSavingData.deffencePoints = deffencePoints;
        newSavingData.isShieldOn = isShieldOn;
        newSavingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;

        newSavingData.name = name;
        newSavingData.positionAndOccupationTileName = _tileOccupied.name;
        newSavingData.rotation_building = rotation_building;
        newSavingData.rotation_center = center.transform.rotation.z;
        newSavingData.rotation_center_w = center.transform.rotation.w;
        newSavingData.type = type;
        newSavingData.level = level;
        newSavingData.isPowerOn = isPowerON;
        newSavingData.upgradeTimer = upgradeTimer;

        GameHendler.Instance.turretsSaved.Add(newSavingData);
        
        // Destroy(gameObject);
    }






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
                TurretLaser turretLaser = null;
                switch (level)
                {
                    case 1:
                    turretLaser = GameObject.Instantiate(PrefabManager.Instance.doubleTuretteLaserPrefab, transform.position, transform.rotation).GetComponent<TurretLaserDouble>();
                    turretLaser.GetComponent<TurretLaserDouble>().ConstructBuildingAfterUpgrade(this);
                    break;

                    case 2:
                    turretLaser = GameObject.Instantiate(PrefabManager.Instance.tripleTuretteLaserPrefab, transform.position, transform.rotation).GetComponent<TurretLaserTriple>();
                    turretLaser.GetComponent<TurretLaserTriple>().ConstructBuildingAfterUpgrade(this);
                    break;
                }
                turretLaser.gameObject.transform.GetChild(1).transform.rotation = gameObject.transform.GetChild(1).transform.rotation;
                temp = turretLaser;
            }
            break;
            // Upgrade misile turret
            case (int)TurretType.MisileTurret:
            {
                TurretMisile turretMisile = null;
                switch (level)
                {
                    case 1:
                    turretMisile = GameObject.Instantiate(PrefabManager.Instance.doubleturetteMisilePrefab, transform.position, transform.rotation).GetComponent<TurretMisileDouble>();
                    turretMisile.GetComponent<TurretMisileDouble>().ConstructBuildingAfterUpgrade(this);
                    break;

                    case 2:
                    turretMisile = GameObject.Instantiate(PrefabManager.Instance.truipleturetteMisilePrefab, transform.position, transform.rotation).GetComponent<TurretMisileTriple>();
                    turretMisile.GetComponent<TurretMisileTriple>().ConstructBuildingAfterUpgrade(this);
                    break;
                }
                turretMisile.gameObject.transform.GetChild(1).transform.rotation = gameObject.transform.GetChild(1).transform.rotation;
                temp = turretMisile;
            }
            break;
        }

        // If menu was opened for this turret reinitializing menu data with new turret
        if (isMenuOpened)
        {
            TurretStaticData.turretMenuReference.ReloadPanel(temp);
        }

        GameViewMenu.Instance.buildingsManageMenuReference.ReplaceTurretScrollItem(this, temp);
        GameObject.Destroy(gameObject);
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

    public virtual void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("TurretMenu");

        TurretStaticData.turretMenuReference.ReloadPanel(this);
    }






    public virtual void ConstructBuilding(Model model)
    {
        level = 1;
        rotation_building = model.rotation;
        _tileOccupied = model.BTileZero;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        HelperObjectInit();


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = _tileOccupied.transform;
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        


        isCreated = true;
        
        isFacingEnemy = false;
        attackState = false;
        isMenuOpened = false;
        isTurnedInIdleMode = true;

        enemiesInsideRange = new List<Enemy>();

        idleRotation = new Quaternion();
        targetRotation = new Quaternion();
        coolDownTurnTimer = 3f;
        upgradeTimer = 0f;    

        combatState = new TurretCombatState();
        idleState = new TurretIdleState();
        powerOffState = new TurretPowerOffState();
        currentState = null;
        
        isPowerON = ResourceManager.Instance.IsPowerOn();

        if (isPowerON)
        {
            currentState = idleState;
        }
        else
        {
            currentState = powerOffState;
        }




        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        canvas.SetActive(true);
        powerOffIndicator.SetActive(false);
        bars.SetActive(false);




        OnDamageTaken += TurretStaticData.turretMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnTurretDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;



        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    public virtual void DestroyBuilding()
    {
        if (isMenuOpened)
        {
            TurretStaticData.turretMenuReference.ExitMenu();
        }
                
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        OnTurretDestroyed(this);

        Destroy(gameObject);

        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    public void InitTurretDataFromPreviousTurret_AlsoInitHelperObj_AlsoInitTurretData(Turette previousTurret)
    {
        rotation_building = previousTurret.rotation_building;
        level = (previousTurret.level + 1);
        type = previousTurret.type;
        _tileOccupied = previousTurret._tileOccupied;

        gameObject.name = previousTurret.name;
        tag = TagConstants.buildingTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;
        HelperObjectInit();

        OnDamageTaken += TurretStaticData.turretMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnTurretDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;

        
        isCreated = true;
        
        isFacingEnemy = false;
        attackState = false;
        isMenuOpened = false;
        isTurnedInIdleMode = true;

        enemiesInsideRange = new List<Enemy>();

        idleRotation = new Quaternion();
        targetRotation = new Quaternion();
        coolDownTurnTimer = 3f;
        upgradeTimer = 0f;    

        combatState = new TurretCombatState();
        idleState = new TurretIdleState();
        powerOffState = new TurretPowerOffState();
        currentState = null;
        
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







    public void TurnTurretOFF()
    {
        isPowerON = false;
    }

    public void TurnTurretON()
    {
        isPowerON = true;
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }

    public virtual void ResetCombatMode(){}
 
    public virtual void Attack(){}














    public void ConstructBuildingFromFile(TurretSavingData savingData)
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

        

        isCreated = true;
        
        isFacingEnemy = false;
        attackState = false;
        isMenuOpened = false;
        isTurnedInIdleMode = true;

        enemiesInsideRange = new List<Enemy>();
        idleRotation = new Quaternion();
        targetRotation = new Quaternion();
        combatState = new TurretCombatState();
        idleState = new TurretIdleState();
        powerOffState = new TurretPowerOffState();
        currentState = null;

        _tileOccupied = GameObject.Find(savingData.positionAndOccupationTileName);
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = _tileOccupied.transform;
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        rotation_building = savingData.rotation_building;

        type = savingData.type;
        level = savingData.level;
        upgradeTimer = savingData.upgradeTimer;
        coolDownTurnTimer = 3f;

        isPowerON = savingData.isPowerOn;

        if (isPowerON)
        {
            currentState = idleState;
        }
        else
        {
            currentState = powerOffState;
        }


        HelperObjectInit();
        center.transform.rotation = new Quaternion(0f, 0f, savingData.rotation_center, savingData.rotation_center_w);

    

    
        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        canvas.SetActive(true);
        powerOffIndicator.SetActive(false);
        bars.SetActive(false);


        OnDamageTaken += TurretStaticData.turretMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnTurretDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
    }

    public void HelperObjectInit()
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.turretRange;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer); // Means that it is noninteractible
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretRangeLayer;

            center = (gameObject.transform.GetChild(1).gameObject);
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }
}


public enum TurretType
{
    LaserTurret = 1,
    MisileTurret = 2
}