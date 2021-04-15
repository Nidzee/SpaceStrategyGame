using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PowerPlant : AliveGameUnit, IBuilding
{
    // Events
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public delegate void PowerPlantDestroy(AliveGameUnit gameUnit);
    public event PowerPlantDestroy OnPowerPlantDestroyed = delegate{};
    public event DamageTaken OnDamageTaken = delegate{};

    // Power Plant data
    public PowerPlantSavingData powerPlantSavingData = null;    
    public GameObject _tileOccupied = null;
    public bool isMenuOpened = false;
    public int rotation = 0;

    // UI
    public GameObject canvas; // Init in inspector
    public Slider healthBar;  // Init in inspector
    public Slider shieldhBar; // Init in inspector


    // Save logic
    public void SaveData()
    {
        powerPlantSavingData = new PowerPlantSavingData();

        powerPlantSavingData.healthPoints = healthPoints;
        powerPlantSavingData.shieldPoints = shieldPoints;
        powerPlantSavingData.maxCurrentHealthPoints = maxCurrentHealthPoints;
        powerPlantSavingData.maxCurrentShieldPoints = maxCurrentShieldPoints;
        powerPlantSavingData.deffencePoints = deffencePoints;
        powerPlantSavingData.isShieldOn = isShieldOn;
        powerPlantSavingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;

        powerPlantSavingData.name = name;
        powerPlantSavingData._tileOccupiedName = _tileOccupied.name;
        powerPlantSavingData.rotation = rotation;

        GameHendler.Instance.powerPlantsSaved.Add(powerPlantSavingData);
    }


    // Building constructing and destroying
    public void ConstructBuilding(Model model)
    {
        // Data initialization
        int health = 0;
        int shield = 0;
        int defense = 0;
        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_1;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_1;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_2;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_2;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_3;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_3;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_3;
            break;
        }
        CreateGameUnit(health, shield, defense);
        gameObject.name = "PP" + PowerPlantStaticData.powerPlant_counter;
        PowerPlantStaticData.powerPlant_counter++;
        rotation = model.rotation;
        _tileOccupied = model.BTileZero;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;


        InitUIEventsBuildingMapInfoAndAddToResourceManagerList();


        ResourceManager.Instance.CreatePPandAddElectricityWholeCount();
    }

    public void ConstructBuildingFromFile(PowerPlantSavingData savingData)
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
        name = savingData.name;
        rotation = savingData.rotation;
        _tileOccupied = GameObject.Find(savingData._tileOccupiedName);
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;


        InitUIEventsBuildingMapInfoAndAddToResourceManagerList();
    }

    private void InitUIEventsBuildingMapInfoAndAddToResourceManagerList()
    {
        // Building map info
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = _tileOccupied.transform;


        // UI
        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(false);


        // Events initialization
        OnDamageTaken += BuildingsManageMenu.Instance.ReloadHPSP;
        OnPowerPlantDestroyed += BuildingsManageMenu.Instance.RemoveFromBuildingsMenu;


        // Resource manager lists managing
        ResourceManager.Instance.powerPlantsList.Add(this);
    }

    public void DestroyBuilding()
    {
        if (isMenuOpened)
        {
            PowerPlantStaticData.powerPlantMenuReference.ExitMenu();
        }


        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        
        OnPowerPlantDestroyed(this);


        ResourceManager.Instance.powerPlantsList.Remove(this);
        Destroy(gameObject);
        ResourceManager.Instance.DestroyPPandRemoveElectricityWholeCount();
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }


    // Other functions
    public void InitStatsAfterBaseUpgrade()
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_1;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_1;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_2;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_2;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_3;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_3;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_3;
            break;
        }

        UpgradeStats(health, shield, defense);

        OnDamageTaken(this);
    }

    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("PowerPlantMenu");
        
        PowerPlantStaticData.powerPlantMenuReference.ReloadPanel(this);
    }


    // Damage logic functions
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