//////////////////////////////////////////////////////////////////////////////

// All functionality is int GameHendler because we can create only 1 instance of Antenne
// But when Antenne dies we dont want to reset buttons timers 

//////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Antenne : AliveGameUnit, IBuilding
{
    // Events
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public delegate void AntenneDestroy(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};
    public event AntenneDestroy OnAntenneDestroyed = delegate{};

    // Antenne data
    public AntenneSavingData antenneSavingData = null;
    public GameObject _tileOccupied = null;
    public GameObject _tileOccupied1 = null;
    public bool isMenuOpened = false;
    public int rotation = 0;


    // UI
    public GameObject canvas; // Init in inspector
    public Slider healthBar;  // Init in inspector
    public Slider shieldhBar; // Init in inspector

    
    // Save data logic
    public void SaveData()
    {
        antenneSavingData = new AntenneSavingData();


        antenneSavingData._tileOccupied_name = _tileOccupied.name;
        antenneSavingData._tileOccupied1_name = _tileOccupied1.name;
        antenneSavingData.rotation = rotation;
        antenneSavingData.name = name;  
        antenneSavingData.healthPoints = healthPoints;
        antenneSavingData.shieldPoints = shieldPoints;
        antenneSavingData.maxCurrentHealthPoints = maxCurrentHealthPoints;
        antenneSavingData.maxCurrentShieldPoints = maxCurrentShieldPoints;
        antenneSavingData.deffencePoints = deffencePoints;
        antenneSavingData.isShieldOn = isShieldOn;
        antenneSavingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;


        GameHendler.Instance.antenneSavingData = antenneSavingData;
    }



    // Other finctions
    public void InitStatsAfterBaseUpgrade()
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_1;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_1;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_2;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_2;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_3;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_3;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_3;
            break;
        }

        UpgradeStats(health, shield, defense);

        OnDamageTaken(this); // KOSTUL'
    }

    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("AntenneMenu");

        AntenneStaticData.antenneMenuReference.ReloadPanel();
    }



    // Constructing and destroying
    public void ConstructBuilding(Model model)
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_1;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_1;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_2;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_2;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_3;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_3;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_3;
            break;
        }

        CreateGameUnit(health, shield, defense);


        gameObject.name = "AN0";
        rotation = model.rotation;
        _tileOccupied = model.BTileZero;
        _tileOccupied1 = model.BTileOne;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;


        // Events
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnAntenneDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;


        // UI
        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(false);


        // Building map info initialization        
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[2];
        info.mapPoints[0] = _tileOccupied.transform;
        info.mapPoints[1] = _tileOccupied1.transform;



        TurnAntenneButtonsON();


        // Resource manager lists maintaining
        ResourceManager.Instance.antenneReference = this;
        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    public void CreateFromFile(AntenneSavingData antenneSavingData)
    {
        name = antenneSavingData.name;

        InitGameUnitFromFile(
        antenneSavingData.healthPoints, 
        antenneSavingData.maxCurrentHealthPoints,
        antenneSavingData.shieldPoints,
        antenneSavingData.maxCurrentShieldPoints,
        antenneSavingData.deffencePoints,
        antenneSavingData.isShieldOn,
        antenneSavingData.shieldGeneratorInfluencers);

        
        // gameObject.AddComponent<BuildingMapInfo>();/////////////////////////////////////////////////////////////////////////////////
        // BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        // info.mapPoints = new Transform[2];
        // info.mapPoints[0] = model.BTileZero.transform;
        // info.mapPoints[1] = model.BTileOne.transform;





        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnAntenneDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;

        _tileOccupied = GameObject.Find(antenneSavingData._tileOccupied_name);
        _tileOccupied1 = GameObject.Find(antenneSavingData._tileOccupied1_name);
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        
        gameObject.AddComponent<BuildingMapInfo>();/////////////////////////////////////////////////////////////////////////////////
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[2];
        info.mapPoints[0] = _tileOccupied.transform;
        info.mapPoints[1] = _tileOccupied1.transform;


        rotation = antenneSavingData.rotation;

        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        canvas.SetActive(false);

        isMenuOpened = false;



        ResourceManager.Instance.antenneReference = this;
    }

    public void DestroyBuilding()
    {
        GameHendler.Instance.resourceDropButton.interactable = false;
        GameHendler.Instance.impusleAttackButton.interactable = false;


        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        if (isMenuOpened)
        {
            AntenneStaticData.antenneMenuReference.ExitMenu();
        }

        OnAntenneDestroyed(this);


        Destroy(gameObject);
        ResourceManager.Instance.antenneReference = null;
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    private void TurnAntenneButtonsON()
    {
        if (!ResourceManager.Instance.isAntenneOnceCreated)
        {
            // Roll animation to open special panel with this buttons
            ResourceManager.Instance.isAntenneOnceCreated = true;

            GameHendler.Instance.antenneButtonsPanel.SetActive(true);
        }

        GameHendler.Instance.resourceDropButton.interactable = ResourceManager.Instance.IsPowerOn();
        GameHendler.Instance.impusleAttackButton.interactable = ResourceManager.Instance.IsPowerOn();
    }



    // Damage logic
    void OnTriggerEnter2D(Collider2D collider)
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
