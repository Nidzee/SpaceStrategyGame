using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PowerPlant : AliveGameUnit, IBuilding
{
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};
    public delegate void PowerPlantDestroy(AliveGameUnit gameUnit);
    public event PowerPlantDestroy OnPowerPlantDestroyed = delegate{};
    public PowerPlantSavingData powerPlantSavingData;    

    public GameObject _tileOccupied;
    public bool isMenuOpened;
    public int rotation;

    public GameObject canvas;
    public Slider healthBar; 
    public Slider shieldhBar;


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

        // Destroy(gameObject);
    }

    public void ConstructBuildingFromFile(PowerPlantSavingData savingData)
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

        
        rotation = savingData.rotation;
        isMenuOpened = false;
        _tileOccupied = null;
        gameObject.name = savingData.name;


        _tileOccupied = GameObject.Find(savingData._tileOccupiedName);
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;


        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = _tileOccupied.transform;


        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        canvas.SetActive(false);



        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnPowerPlantDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;

        ResourceManager.Instance.powerPlantsList.Add(this);
    }

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

    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("PowerPlantMenu");
        
        PowerPlantStaticData.powerPlantMenuReference.ReloadPanel(this);
    }

    public void ConstructBuilding(Model model)
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

        CreateGameUnit(health, shield, defense);

        rotation = model.rotation;
        isMenuOpened = false;
        _tileOccupied = null;
        
        gameObject.name = "PP" + PowerPlantStaticData.powerPlant_counter;
        PowerPlantStaticData.powerPlant_counter++;



        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = model.BTileZero.transform;


        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        canvas.SetActive(false);


        _tileOccupied = model.BTileZero;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;


        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnPowerPlantDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;


        ResourceManager.Instance.powerPlantsList.Add(this);
        ResourceManager.Instance.CreatePPandAddElectricityWholeCount();
    }

    public void DestroyBuilding()
    {
        ResourceManager.Instance.powerPlantsList.Remove(this);

        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        if (isMenuOpened)
        {
            PowerPlantStaticData.powerPlantMenuReference.ExitMenu();
        }
        
        OnPowerPlantDestroyed(this);
        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyPPandRemoveElectricityWholeCount();
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }

}