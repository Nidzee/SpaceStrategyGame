using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShieldGenerator : AliveGameUnit, IBuilding
{
    // Events
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public delegate void Upgraded(ShieldGenerator shieldGenerator);
    public delegate void ShieldGeneraorDestroy(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};
    public event Upgraded OnUpgraded = delegate{};
    public event ShieldGeneraorDestroy OnSGDestroyed = delegate{};


    // Shield generator data
    public ShieldGeneratorSavingData shieldGeneratorSavingData;
    public GameObject _tileOccupied = null;        // Reference to real MapTile on which building is set
    public GameObject _tileOccupied1 = null;       // Reference to real MapTile on which building is set
    public GameObject _tileOccupied2 = null;       // Reference to real MapTile on which building is set
    public GameObject shieldGeneratorRangeRef = null;
    public int level = 0;
    public bool isMenuOpened = false;
    public Vector3 targetScale;
    public bool isShieldCreationInProgress = false; // Do not touch - LEGACY CODE
    public bool isDisablingInProgress = false;      // DO not touch - LEGACE CODE
    public bool isUpgradeInProgress = false;        // Do not touch - LEGACY CODE
    public float upgradeTimer = 0f;
    public int rotation = 0;


    // UI
    public GameObject canvas; // Init in inspector
    public Slider healthBar;  // Init in inspector
    public Slider shieldhBar; // Init in inspector



    // Save logic
    public void SaveData()
    {
        shieldGeneratorSavingData = new ShieldGeneratorSavingData();

        shieldGeneratorSavingData.name = name;

        shieldGeneratorSavingData.healthPoints = healthPoints;
        shieldGeneratorSavingData.shieldPoints = shieldPoints;
        shieldGeneratorSavingData.maxCurrentHealthPoints = maxCurrentHealthPoints;
        shieldGeneratorSavingData.maxCurrentShieldPoints = maxCurrentShieldPoints;
        shieldGeneratorSavingData.deffencePoints = deffencePoints;
        shieldGeneratorSavingData.isShieldOn = isShieldOn;
        shieldGeneratorSavingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;


        shieldGeneratorSavingData._tileOccupied_name = _tileOccupied.name;        
        shieldGeneratorSavingData._tileOccupied1_name = _tileOccupied1.name; 
        shieldGeneratorSavingData._tileOccupied2_name = _tileOccupied2.name;

        shieldGeneratorSavingData.rotation = rotation;
        shieldGeneratorSavingData.level = level;

        if (isDisablingInProgress)
        {
            // disabling
            shieldGeneratorSavingData.shield_state = 4;
        }
        else if (isShieldCreationInProgress)
        {
            // creating
            shieldGeneratorSavingData.shield_state = 3;
        }
        else if (!shieldGeneratorRangeRef)
        {
            // not created
            shieldGeneratorSavingData.shield_state = 2;
        }
        else if (shieldGeneratorRangeRef && !isDisablingInProgress && !isShieldCreationInProgress)
        {
            // created
            shieldGeneratorSavingData.shield_state = 1;
        }
        
        shieldGeneratorSavingData.upgradeTimer = upgradeTimer;


        if (shieldGeneratorRangeRef)
        {
            shieldGeneratorSavingData.shieldScale_x = shieldGeneratorRangeRef.transform.localScale.x;
            shieldGeneratorSavingData.shieldScale_y = shieldGeneratorRangeRef.transform.localScale.y;
            shieldGeneratorSavingData.shieldScale_z = shieldGeneratorRangeRef.transform.localScale.z;
        }

        GameHendler.Instance.shieldGeneratorsSaved.Add(shieldGeneratorSavingData);
    }




    // Shield game object manipulations
    public void EnableShield()
    {
        if (!shieldGeneratorRangeRef)
        {
            shieldGeneratorRangeRef = GameObject.Instantiate (ShiledGeneratorStaticData.shieldRangePrefab, gameObject.transform.position, Quaternion.identity);
            shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.shieldGeneratorRangeLayer;
            
            shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorRangeRef.name = "ShieldGeneratorRange";

            // May be useless as we upgrade target scale
            switch (level)
            {
                case 1:
                targetScale = ShiledGeneratorStaticData.scaleLevel1;
                break;

                case 2:
                targetScale = ShiledGeneratorStaticData.scaleLevel2;
                break;

                case 3:
                targetScale = ShiledGeneratorStaticData.scaleLevel3;
                break;
            }

            isShieldCreationInProgress = true;
            StartCoroutine(TurningShiledON());
        }
        else
        {
            Debug.Log("Error! Shield is already On!");
        }
    }
    
    public void DisableShield()
    {
        if (shieldGeneratorRangeRef)
        {
            isDisablingInProgress = true;
            StartCoroutine(TurningShiledOFF());
        }
        else
        {
            Debug.Log("Error! Shield is already Off!");
        }
    }

    public IEnumerator TurningShiledON()
    {
        while (shieldGeneratorRangeRef.transform.localScale != targetScale)
        {
            shieldGeneratorRangeRef.transform.localScale += new Vector3(0.5f,0.5f,0);
            yield return null;
        }

        if (isMenuOpened)
        {
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.OFFbutton.interactable = true;
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.ONbutton.interactable = false;
        }

        isShieldCreationInProgress = false;
    }

    public IEnumerator TurningShiledOFF()
    {
        while (shieldGeneratorRangeRef.transform.localScale != ShiledGeneratorStaticData.startScale)
        {
            shieldGeneratorRangeRef.transform.localScale -= new Vector3(0.5f,0.5f,0);
            yield return null;
        }

        if (isMenuOpened)
        {
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.OFFbutton.interactable = false;
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.ONbutton.interactable = true;
        }
        
        GameObject.Destroy(shieldGeneratorRangeRef);
        isDisablingInProgress = false;
    }




    // Upgrade logic
    public void StartUpgrade()
    {
        StartCoroutine(UpgradeLogic());
    }

    public IEnumerator UpgradeLogic()
    {
        isUpgradeInProgress = true;

        while (upgradeTimer < 1)
        {
            upgradeTimer += ShiledGeneratorStaticData._timerStep * Time.deltaTime;

            if (isMenuOpened) // Reload fill circles
            {
                switch (level)
                {
                    case 1:
                    ShiledGeneratorStaticData.shieldGeneratorMenuReference.level2.fillAmount = upgradeTimer;
                    break;

                    case 2:
                    ShiledGeneratorStaticData.shieldGeneratorMenuReference.level3.fillAmount = upgradeTimer;
                    break;

                    case 3:
                    Debug.Log("Error");
                    break;
                }
            }

            yield return null;
        }

        upgradeTimer = 0;
        isUpgradeInProgress = false;

        ShiledGeneratorUpgrading();
    }

    private void ShiledGeneratorUpgrading()
    {
        Debug.Log("Shield Generator levelUP!");

        if (level == 1)
        {
            UpgradeToLvl2();
        }
        else if (level == 2)
        {
            UpgradeToLvl3();
        }
        else
        {
            Debug.LogError("ERROR! - Invalid ShieldGenerator level!!!!!");
        }

        ModifyShieldRangeIfItIsActive();
    }

    private void ModifyShieldRangeIfItIsActive()
    {
        if (shieldGeneratorRangeRef && !isShieldCreationInProgress && !isDisablingInProgress)
        {
            isShieldCreationInProgress = true;
            StartCoroutine(TurningShiledON());
            if (isMenuOpened)
            {
                ShiledGeneratorStaticData.shieldGeneratorMenuReference.OFFbutton.interactable = false;
                ShiledGeneratorStaticData.shieldGeneratorMenuReference.ONbutton.interactable = false;
            }
        }
    }

    public void UpgradeToLvl2()
    {
        level = 2;
        targetScale = ShiledGeneratorStaticData.scaleLevel2;

        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
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

        OnUpgraded(this);

        OnDamageTaken(this);
    }

    public void UpgradeToLvl3()
    {
        level = 3;
        targetScale = ShiledGeneratorStaticData.scaleLevel3;

        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
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

        OnUpgraded(this);

        OnDamageTaken(this);
    }



    // Other functions
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("ShieldGeneratorMenu");

        ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadPanel(this);
    }

    public void InitStatsAfterBaseUpgrade()
    {
        int newHealth = 0;
        int newShield = 0;
        int newDefense = 0;
        
        switch (level)
        {
            case 1:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl1_ShieldGenerator_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl1_ShieldGenerator_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl1_ShieldGenerator_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl1_ShieldGenerator_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl1_ShieldGenerator_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl1_ShieldGenerator_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl1_ShieldGenerator_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl1_ShieldGenerator_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl1_ShieldGenerator_Base_Lvl_3;
                break;
            }
            break;

            case 2:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl2_ShieldGenerator_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl2_ShieldGenerator_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl2_ShieldGenerator_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl2_ShieldGenerator_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl2_ShieldGenerator_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl2_ShieldGenerator_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl2_ShieldGenerator_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl2_ShieldGenerator_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl2_ShieldGenerator_Base_Lvl_3;
                break;
            }
            break;


            case 3:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl3_ShieldGenerator_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl3_ShieldGenerator_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl3_ShieldGenerator_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl3_ShieldGenerator_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl3_ShieldGenerator_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl3_ShieldGenerator_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl3_ShieldGenerator_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl3_ShieldGenerator_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl3_ShieldGenerator_Base_Lvl_3;
                break;
            }
            break;
        }

        UpgradeStats(newHealth, newShield, newDefense);
        
        OnDamageTaken(this);
    }





    // Constructing and destroying
    public void ConstructBuilding(Model model)
    {
        // Data initialization
        int health = 0;
        int shield = 0;
        int defense = 0;
        switch (ResourceManager.Instance.shtabReference.level)
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
        gameObject.name = "SG" + ShiledGeneratorStaticData.shieldGenerator_counter;
        ShiledGeneratorStaticData.shieldGenerator_counter++;
        _tileOccupied = model.BTileZero;
        _tileOccupied1 = model.BTileOne;
        _tileOccupied2 = model.BTileTwo;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied2.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        rotation = model.rotation;
        level = 1;



        // Events building map info and ResourceManager list initialization
        InitBuildingMapInfo_UI_Events_AndAddToResourceManagerList();



        // Resource manager manipulations
        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    public void CreateFromFile(ShieldGeneratorSavingData shieldGeneratorSavingData)
    {
        // Data initialization
        InitGameUnitFromFile(
        shieldGeneratorSavingData.healthPoints, 
        shieldGeneratorSavingData.maxCurrentHealthPoints,
        shieldGeneratorSavingData.shieldPoints,
        shieldGeneratorSavingData.maxCurrentShieldPoints,
        shieldGeneratorSavingData.deffencePoints,
        shieldGeneratorSavingData.isShieldOn,
        shieldGeneratorSavingData.shieldGeneratorInfluencers);
        name = shieldGeneratorSavingData.name;
        _tileOccupied = GameObject.Find(shieldGeneratorSavingData._tileOccupied_name);        // Reference to real MapTile on which building is set
        _tileOccupied1 = GameObject.Find(shieldGeneratorSavingData._tileOccupied1_name);       // Reference to real MapTile on which building is set
        _tileOccupied2 = GameObject.Find(shieldGeneratorSavingData._tileOccupied2_name);       // Reference to real MapTile on which building is set
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied2.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        level = shieldGeneratorSavingData.level;
        rotation = shieldGeneratorSavingData.rotation;
        upgradeTimer = shieldGeneratorSavingData.upgradeTimer;
        // 1 - ON 
        // 2 - OFF
        // 3 - Turning ON
        // 4 - Turning OFF
        switch (shieldGeneratorSavingData.shield_state)
        {
            case 1:
            // Create Shield
            shieldGeneratorRangeRef = GameObject.Instantiate (ShiledGeneratorStaticData.shieldRangePrefab, gameObject.transform.position, Quaternion.identity);
            shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.shieldGeneratorRangeLayer;
        
            shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorRangeRef.name = "ShieldGeneratorRange";

            switch (shieldGeneratorSavingData.level)
            {
                case 1:
                shieldGeneratorRangeRef.transform.localScale = ShiledGeneratorStaticData.scaleLevel1;
                break;

                case 2:
                shieldGeneratorRangeRef.transform.localScale = ShiledGeneratorStaticData.scaleLevel2;
                break;

                case 3:
                shieldGeneratorRangeRef.transform.localScale = ShiledGeneratorStaticData.scaleLevel3;
                break;
            }

            isShieldCreationInProgress = false; 
            isDisablingInProgress = false;     
            break;

            case 2:
            // Dont create shield
            
            isShieldCreationInProgress = false; 
            isDisablingInProgress = false; 
            break;
            
            case 3:
            shieldGeneratorRangeRef = GameObject.Instantiate (ShiledGeneratorStaticData.shieldRangePrefab, gameObject.transform.position, Quaternion.identity);
            shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.shieldGeneratorRangeLayer;
        
            shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorRangeRef.name = "ShieldGeneratorRange";

            shieldGeneratorRangeRef.transform.localScale = new Vector3(shieldGeneratorSavingData.shieldScale_x, shieldGeneratorSavingData.shieldScale_y, shieldGeneratorSavingData.shieldScale_z);
            
            
            isShieldCreationInProgress = true; 
            isDisablingInProgress = false; 

            
            switch (shieldGeneratorSavingData.level)
            {
                case 1:
                targetScale = ShiledGeneratorStaticData.scaleLevel1;
                break;

                case 2:
                targetScale = ShiledGeneratorStaticData.scaleLevel2;
                break;

                case 3:
                targetScale = ShiledGeneratorStaticData.scaleLevel3;
                break;
            }

            StartCoroutine(TurningShiledON());
            
            break;

            case 4:
            shieldGeneratorRangeRef = GameObject.Instantiate (ShiledGeneratorStaticData.shieldRangePrefab, gameObject.transform.position, Quaternion.identity);
            shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.shieldGeneratorRangeLayer;
        
            shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorRangeRef.name = "ShieldGeneratorRange";

            shieldGeneratorRangeRef.transform.localScale = new Vector3(shieldGeneratorSavingData.shieldScale_x, shieldGeneratorSavingData.shieldScale_y, shieldGeneratorSavingData.shieldScale_z);
            
            
            isShieldCreationInProgress = false; 
            isDisablingInProgress = true;


            
            targetScale = ShiledGeneratorStaticData.startScale;
            

            StartCoroutine(TurningShiledOFF());
            
            break;
        }
        if (upgradeTimer != 0)
        {
            isUpgradeInProgress = true;
            StartCoroutine(UpgradeLogic());
        }



        // rest data initialization
        InitBuildingMapInfo_UI_Events_AndAddToResourceManagerList();
    }

    private void InitBuildingMapInfo_UI_Events_AndAddToResourceManagerList()
    {
        // Building map info initialization
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[3];
        info.mapPoints[0] = _tileOccupied.transform;
        info.mapPoints[1] = _tileOccupied1.transform;
        info.mapPoints[2] = _tileOccupied2.transform;



        // UI
        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(false);


        // Events initialization
        OnUpgraded += ShiledGeneratorStaticData.shieldGeneratorMenuReference.UpdateUIAfterUpgrade;
        OnDamageTaken += ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnSGDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;


        // Resource manager lists maintaining
        ResourceManager.Instance.shiledGeneratorsList.Add(this);
    }

    public void DestroyBuilding()
    {
        // Remove shield range
        if (shieldGeneratorRangeRef)
        {
            GameObject.Destroy(shieldGeneratorRangeRef);
        }


        // Deleting map info
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        _tileOccupied2.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;


        // Close UI menu
        if (isMenuOpened)
        {
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.ExitMenu();
        }


        // Remove manipulations
        ResourceManager.Instance.shiledGeneratorsList.Remove(this);
        OnSGDestroyed(this);
        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }





    // Damage logic functions
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
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

        canvas.SetActive(true);

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
        canvas.SetActive(false);
    }
}