using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShieldGenerator : AliveGameUnit, IBuilding
{
    public ShieldGeneratorData shieldGeneratorData;
    public ShieldGeneratorSavingData shieldGeneratorSavingData;

    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void Upgraded();
    public event Upgraded OnUpgraded = delegate{};

    public delegate void ShieldGeneraorDestroy(AliveGameUnit gameUnit);
    public event ShieldGeneraorDestroy OnSGDestroyed = delegate{};

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (name == "SG0")
            {
                shieldGeneratorSavingData = new ShieldGeneratorSavingData();

                shieldGeneratorSavingData.name = this.name;

                shieldGeneratorSavingData.isShieldOn = this.isShieldOn;
                shieldGeneratorSavingData.shieldGeneratorInfluencers = this.shieldGeneratorInfluencers;
                shieldGeneratorSavingData.shieldPoints = this.shieldPoints;
                shieldGeneratorSavingData.healthPoints = this.healthPoints;
                shieldGeneratorSavingData.maxCurrentHealthPoints = this.maxCurrentHealthPoints;
                shieldGeneratorSavingData.maxCurrentShieldPoints = this.maxCurrentShieldPoints;
                shieldGeneratorSavingData.deffencePoints = this.deffencePoints;


                shieldGeneratorSavingData._tileOccupied_name = shieldGeneratorData._tileOccupied.name;        
                shieldGeneratorSavingData._tileOccupied1_name = shieldGeneratorData._tileOccupied1.name; 
                shieldGeneratorSavingData._tileOccupied2_name = shieldGeneratorData._tileOccupied2.name;

                shieldGeneratorSavingData.rotation = shieldGeneratorData.rotation;
                shieldGeneratorSavingData.level = shieldGeneratorData.level;

                if (shieldGeneratorData.isDisablingInProgress)
                {
                    // disabling
                    shieldGeneratorSavingData.shield_state = 4;
                }
                else if (shieldGeneratorData.isShieldCreationInProgress)
                {
                    // creating
                    shieldGeneratorSavingData.shield_state = 3;
                }
                else if (!shieldGeneratorData.shieldGeneratorRangeRef)
                {
                    // not created
                    shieldGeneratorSavingData.shield_state = 2;
                }
                else if (shieldGeneratorData.shieldGeneratorRangeRef && !shieldGeneratorData.isDisablingInProgress && !shieldGeneratorData.isShieldCreationInProgress)
                {
                    // created
                    shieldGeneratorSavingData.shield_state = 1;
                }
                
                shieldGeneratorSavingData.upgradeTimer = shieldGeneratorData.upgradeTimer;


                if (shieldGeneratorData.shieldGeneratorRangeRef)
                {
                    shieldGeneratorSavingData.shieldScale_x = shieldGeneratorData.shieldGeneratorRangeRef.transform.localScale.x;
                    shieldGeneratorSavingData.shieldScale_y = shieldGeneratorData.shieldGeneratorRangeRef.transform.localScale.y;
                    shieldGeneratorSavingData.shieldScale_z = shieldGeneratorData.shieldGeneratorRangeRef.transform.localScale.z;
                }

                ResourceManager.Instance.shiledGeneratorsList.Remove(this);

                if (shieldGeneratorData.shieldGeneratorRangeRef)
                {
                    Destroy(shieldGeneratorData.shieldGeneratorRangeRef.gameObject);
                }

                GameHendler.Instance.shieldGeneratorSavingData = shieldGeneratorSavingData;

                Destroy(gameObject);








                // DestroyBuilding();



                // DestroyBuilding();
                
                // BuildingMapInfo temp = GetComponent<BuildingMapInfo>();

                // foreach (var mapPoint in temp.mapPoints)
                // {
                //     Debug.Log(mapPoint.name);
                // }
            }
        }
    }
























    public void StartUpgrade()
    {
        StartCoroutine(shieldGeneratorData.UpgradeLogic());
    }

    public void EnableShield()
    {
        shieldGeneratorData.EnableShield();
    }

    public void DisableShield()
    {
        shieldGeneratorData.DisableShield();
    }


    public void InitStatsAfterBaseUpgrade()
    {
        shieldGeneratorData.InitStatsAfterBaseUpgrade();
        
        OnDamageTaken(this);
    }


    public void UpgradeToLvl2()
    {
        shieldGeneratorData.UpgradeToLvl2();

        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl2_ShieldGenerator_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl2_ShieldGenerator_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl2_ShieldGenerator_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl2_ShieldGenerator_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl2_ShieldGenerator_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl2_ShieldGenerator_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl2_ShieldGenerator_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl2_ShieldGenerator_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl2_ShieldGenerator_Base_Lvl_3;
            break;
        }
        UpgradeStats(health, shield, defense);


        OnUpgraded();
        OnDamageTaken(this);
    }

    public void UpgradeToLvl3()
    {
        shieldGeneratorData.UpgradeToLvl3();

        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl3_ShieldGenerator_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl3_ShieldGenerator_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl3_ShieldGenerator_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl3_ShieldGenerator_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl3_ShieldGenerator_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl3_ShieldGenerator_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl3_ShieldGenerator_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl3_ShieldGenerator_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl3_ShieldGenerator_Base_Lvl_3;
            break;
        }
        UpgradeStats(health, shield, defense);


        OnUpgraded();
        OnDamageTaken(this);
    }


    // Reloads sliders if Turret Menu is opened
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

    // Function for creating building
    public void ConstructBuilding(Model model)
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl1_ShieldGenerator_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl1_ShieldGenerator_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl1_ShieldGenerator_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl1_ShieldGenerator_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl1_ShieldGenerator_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl1_ShieldGenerator_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl1_ShieldGenerator_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl1_ShieldGenerator_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl1_ShieldGenerator_Base_Lvl_3;
            break;
        }

        CreateGameUnit(health, shield, defense);

        shieldGeneratorData = new ShieldGeneratorData(this);

        gameObject.name = "SG" + ShiledGeneratorStaticData.shieldGenerator_counter;
        ShiledGeneratorStaticData.shieldGenerator_counter++;





        gameObject.AddComponent<BuildingMapInfo>();////////////////////////////////////////////////////////////////////////////
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[3];
        info.mapPoints[0] = model.BTileZero.transform;
        info.mapPoints[1] = model.BTileOne.transform;
        info.mapPoints[2] = model.BTileTwo.transform;




        shieldGeneratorData.ConstructBuilding(model);

        OnUpgraded += ShiledGeneratorStaticData.shieldGeneratorMenuReference.UpgradeVisuals;
        OnDamageTaken += ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnSGDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;





        ResourceManager.Instance.shiledGeneratorsList.Add(this);
        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    // Function for displaying info
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("ShieldGeneratorMenu");
        
        shieldGeneratorData.Invoke();

        ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadPanel(this);
    }


    public void DestroyBuilding()
    {
        shieldGeneratorData.DestroyBuilding();

        OnSGDestroyed(this);

        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }
















    public void CreateFromFile(ShieldGeneratorSavingData shieldGeneratorSavingData)
    {
        name = shieldGeneratorSavingData.name;

        InitGameUnitFromFile(
        shieldGeneratorSavingData.healthPoints, 
        shieldGeneratorSavingData.maxCurrentHealthPoints,
        shieldGeneratorSavingData.shieldPoints,
        shieldGeneratorSavingData.maxCurrentShieldPoints,
        shieldGeneratorSavingData.deffencePoints,
        shieldGeneratorSavingData.isShieldOn,
        shieldGeneratorSavingData.shieldGeneratorInfluencers);


        
        shieldGeneratorData = new ShieldGeneratorData(this);

        shieldGeneratorData._tileOccupied = GameObject.Find(shieldGeneratorSavingData._tileOccupied_name);        // Reference to real MapTile on which building is set
        shieldGeneratorData._tileOccupied1 = GameObject.Find(shieldGeneratorSavingData._tileOccupied1_name);       // Reference to real MapTile on which building is set
        shieldGeneratorData._tileOccupied2 = GameObject.Find(shieldGeneratorSavingData._tileOccupied2_name);       // Reference to real MapTile on which building is set
        
        shieldGeneratorData.level = shieldGeneratorSavingData.level;
        shieldGeneratorData.isMenuOpened = false;

        shieldGeneratorData.rotation = shieldGeneratorSavingData.rotation;
        shieldGeneratorData.upgradeTimer = shieldGeneratorSavingData.upgradeTimer;














        // 1 - ON 
        // 2 - OFF
        // 3 - Turning ON
        // 4 - Turning OFF
        switch (shieldGeneratorSavingData.shield_state)
        {
            case 1:
            // Create Shield
            shieldGeneratorData.shieldGeneratorRangeRef = GameObject.Instantiate (ShiledGeneratorStaticData.shieldRangePrefab, gameObject.transform.position, Quaternion.identity);
            shieldGeneratorData.shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            shieldGeneratorData.shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.shieldGeneratorRangeLayer;
        
            shieldGeneratorData.shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorData.shieldGeneratorRangeRef.name = "ShieldGeneratorRange";

            switch (shieldGeneratorSavingData.level)
            {
                case 1:
                shieldGeneratorData.shieldGeneratorRangeRef.transform.localScale = ShiledGeneratorStaticData.scaleLevel1;
                break;

                case 2:
                shieldGeneratorData.shieldGeneratorRangeRef.transform.localScale = ShiledGeneratorStaticData.scaleLevel2;
                break;

                case 3:
                shieldGeneratorData.shieldGeneratorRangeRef.transform.localScale = ShiledGeneratorStaticData.scaleLevel3;
                break;
            }

            shieldGeneratorData.isShieldCreationInProgress = false; 
            shieldGeneratorData.isDisablingInProgress = false;     
            break;

            case 2:
            // Dont create shield
            
            shieldGeneratorData.isShieldCreationInProgress = false; 
            shieldGeneratorData.isDisablingInProgress = false; 
            break;
            
            case 3:
            shieldGeneratorData.shieldGeneratorRangeRef = GameObject.Instantiate (ShiledGeneratorStaticData.shieldRangePrefab, gameObject.transform.position, Quaternion.identity);
            shieldGeneratorData.shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            shieldGeneratorData.shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.shieldGeneratorRangeLayer;
        
            shieldGeneratorData.shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorData.shieldGeneratorRangeRef.name = "ShieldGeneratorRange";

            shieldGeneratorData.shieldGeneratorRangeRef.transform.localScale = new Vector3(shieldGeneratorSavingData.shieldScale_x, shieldGeneratorSavingData.shieldScale_y, shieldGeneratorSavingData.shieldScale_z);
            
            
            shieldGeneratorData.isShieldCreationInProgress = true; 
            shieldGeneratorData.isDisablingInProgress = false; 

            
            switch (shieldGeneratorSavingData.level)
            {
                case 1:
                shieldGeneratorData.targetScale = ShiledGeneratorStaticData.scaleLevel1;
                break;

                case 2:
                shieldGeneratorData.targetScale = ShiledGeneratorStaticData.scaleLevel2;
                break;

                case 3:
                shieldGeneratorData.targetScale = ShiledGeneratorStaticData.scaleLevel3;
                break;
            }

            StartCoroutine(shieldGeneratorData.TurningShiledON());
            
            break;

            case 4:
            shieldGeneratorData.shieldGeneratorRangeRef = GameObject.Instantiate (ShiledGeneratorStaticData.shieldRangePrefab, gameObject.transform.position, Quaternion.identity);
            shieldGeneratorData.shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            shieldGeneratorData.shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.shieldGeneratorRangeLayer;
        
            shieldGeneratorData.shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorData.shieldGeneratorRangeRef.name = "ShieldGeneratorRange";

            shieldGeneratorData.shieldGeneratorRangeRef.transform.localScale = new Vector3(shieldGeneratorSavingData.shieldScale_x, shieldGeneratorSavingData.shieldScale_y, shieldGeneratorSavingData.shieldScale_z);
            
            
            shieldGeneratorData.isShieldCreationInProgress = false; 
            shieldGeneratorData.isDisablingInProgress = true;


            
            shieldGeneratorData.targetScale = ShiledGeneratorStaticData.startScale;
            

            StartCoroutine(shieldGeneratorData.TurningShiledOFF());
            
            break;
        }

        if (shieldGeneratorData.upgradeTimer != 0)
        {
            shieldGeneratorData.isUpgradeInProgress = true;
            StartCoroutine(shieldGeneratorData.UpgradeLogic());
        }
        else
        {
            shieldGeneratorData.isUpgradeInProgress = false;
        }













        OnUpgraded += ShiledGeneratorStaticData.shieldGeneratorMenuReference.UpgradeVisuals;
        OnDamageTaken += ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnSGDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;


        ResourceManager.Instance.shiledGeneratorsList.Add(this);
    }



}