using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Base : AliveGameUnit, IBuilding
{
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};
    public delegate void Upgraded();
    public event Upgraded OnUpgraded = delegate{};
    public delegate void ShtabDestroy(AliveGameUnit gameUnit);
    public event ShtabDestroy OnShtabDestroyed = delegate{};
    public ShtabSavingData shtabSavingData;


    public GameObject resourceRef;        // Reference to Unit resource object (for creating copy and consuming)
    public GameObject storageConsumer;    // Place for resource consuming and dissappearing
    public int level;                     // Determin upgrade level of rest buildings
    public bool isMenuOpened = false;
    public float upgradeTimer = 0;
    public GameObject _tileOccupied;              // Reference to real MapTile on which building is set
    public GameObject _tileOccupied1; 
    public GameObject _tileOccupied2;              // Reference to real MapTile on which building is set
    public GameObject _tileOccupied3;

    public Slider healthBar; 
    public Slider shieldhBar;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10);
        }
    }


    public void SaveData()
    {
        shtabSavingData = new ShtabSavingData();

        shtabSavingData.name = name;

        shtabSavingData._tileOccupiedName = _tileOccupied.name;
        shtabSavingData._tileOccupied1Name = _tileOccupied1.name;
        shtabSavingData._tileOccupied2Name = _tileOccupied2.name;
        shtabSavingData._tileOccupied3Name = _tileOccupied3.name;

        
        shtabSavingData.level = level;                     // Determin upgrade level of rest buildings
        shtabSavingData.upgradeTimer = upgradeTimer;

        
        shtabSavingData.healthPoints = healthPoints;
        shtabSavingData.shieldPoints = shieldPoints;
        shtabSavingData.maxCurrentHealthPoints = maxCurrentHealthPoints;
        shtabSavingData.maxCurrentShieldPoints = maxCurrentShieldPoints;
        shtabSavingData.deffencePoints = deffencePoints;
        shtabSavingData.isShieldOn = isShieldOn;
        shtabSavingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;

        GameHendler.Instance.shtabSavingData = shtabSavingData;

        Destroy(gameObject);
    }



    public void StartUpgrade()
    {
        StartCoroutine(UpgradeLogic());
    }

    IEnumerator UpgradeLogic()
    {
        while (upgradeTimer < 1)
        {
            upgradeTimer += ShtabStaticData._timerStep * Time.deltaTime;

            if (isMenuOpened) // Reload fill circles
            {
                switch(level)
                {
                    case 1:
                    ShtabStaticData.baseMenuReference.level2.fillAmount = upgradeTimer;
                    break;

                    case 2:
                    ShtabStaticData.baseMenuReference.level3.fillAmount = upgradeTimer;
                    break;

                    case 3:
                    Debug.LogError("Error! Invalid Circle filling");
                    break;
                }
            }
            yield return null;
        }

        upgradeTimer = 0;

        Upgrade();
    }

    public void Upgrade()
    {
        if (level == 1)
        {
            InitStaticsLevel_2();
            StatsManager.InitCost_ToLvl3___Shtab();
        }
        else if (level == 2)
        {
            InitStaticsLevel_3();
            ShtabStaticData.baseMenuReference._upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
        }
        else
        {
            Debug.LogError("ERROR! - Invalid base level!!!!!");
        }
    }

    public void InitStaticsLevel_2()
    {
        level = 2;

        UpgradeStats(StatsManager._maxHealth_Lvl2_Shtab, StatsManager._maxShiled_Lvl2_Shtab, StatsManager._deffencePoints_Lvl2_Shtab);
       
        OnDamageTaken(this);

        OnUpgraded();
    }

    public void InitStaticsLevel_3()
    {
        level = 3;

        UpgradeStats(StatsManager._maxHealth_Lvl3_Shtab, StatsManager._maxShiled_Lvl3_Shtab, StatsManager._deffencePoints_Lvl3_Shtab);

        OnDamageTaken(this);

        OnUpgraded();
    }



    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);

        if (healthPoints <= 0)
        {
            DestroyBuilding();
            return;
        }

        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;

        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        OnDamageTaken(this);
    }

    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("BaseMenu");
        
        ShtabStaticData.baseMenuReference.ReloadPanel(ResourceManager.Instance.shtabReference);
    }



    public void ConstructBuilding(Model model)
    {
        CreateGameUnit(StatsManager._maxHealth_Lvl1_Shtab, StatsManager._maxShiled_Lvl1_Shtab, StatsManager._deffencePoints_Lvl1_Shtab);

        HelperObjectInit();

        level = 1;
        name = "BASE";
        transform.tag = TagConstants.buildingTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;

        _tileOccupied = GameObject.Find("9.28.-37");
        _tileOccupied1 = GameObject.Find("9.29.-38");
        _tileOccupied2 = GameObject.Find("10.28.-38");
        _tileOccupied3 = GameObject.Find("10.29.-39");


        
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[4];

        info.mapPoints[0] = _tileOccupied.transform;
        info.mapPoints[1] = _tileOccupied1.transform;
        info.mapPoints[2] = _tileOccupied2.transform;
        info.mapPoints[3] = _tileOccupied3.transform;





        OnDamageTaken += ShtabStaticData.baseMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnUpgraded += ShtabStaticData.baseMenuReference.UpdateUIAfterBaseUpgrade;
        OnUpgraded += ResourceManager.Instance.UpgradeStatisticsAfterBaseUpgrade;


        ResourceManager.Instance.shtabReference = this;
    }

    public void ConstructBuildingFromFile(ShtabSavingData savingData)
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

        _tileOccupied = GameObject.Find(savingData._tileOccupiedName);
        _tileOccupied1 = GameObject.Find(savingData._tileOccupied1Name);
        _tileOccupied2 = GameObject.Find(savingData._tileOccupied2Name);
        _tileOccupied3 = GameObject.Find(savingData._tileOccupied3Name);

        
        level = savingData.level;
        upgradeTimer = savingData.upgradeTimer;

        
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[4];

        info.mapPoints[0] = _tileOccupied.transform;
        info.mapPoints[1] = _tileOccupied1.transform;
        info.mapPoints[2] = _tileOccupied2.transform;
        info.mapPoints[3] = _tileOccupied3.transform;


        
        OnDamageTaken += ShtabStaticData.baseMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnUpgraded += ShtabStaticData.baseMenuReference.UpdateUIAfterBaseUpgrade;
        OnUpgraded += ResourceManager.Instance.UpgradeStatisticsAfterBaseUpgrade;

        HelperObjectInit();


        ResourceManager.Instance.shtabReference = this;


        if (upgradeTimer != 0)
        {
            StartCoroutine(UpgradeLogic());
        }
    }





    public Transform GetUnitDestination()
    {
        return storageConsumer.transform;
    }

    public void DestroyBuilding()
    {
        Debug.Log("END OF THE GAME!");
    }

    public void ActivateDefenceMode()
    {
        Debug.Log("Defence MODE!");
    }

    public void ActivateAttackMode()
    {
        Debug.Log("Attack MODE!");
    }

    public void HelperObjectInit()
    {
        if (gameObject.transform.childCount != 0)
        {
            storageConsumer = gameObject.transform.GetChild(0).gameObject;

            storageConsumer.tag = TagConstants.baseStorageTag;
            storageConsumer.gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);            
        }
        else
        {
            Debug.LogError("ERROR!     No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }

    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }
}