using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Base : AliveGameUnit, IBuilding
{
    // Events
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public delegate void Upgraded();
    public delegate void ShtabDestroy(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};
    public event ShtabDestroy OnShtabDestroyed = delegate{};
    public event Upgraded OnUpgraded = delegate{};


    // Shtab data
    public ShtabSavingData shtabSavingData = null;
    public GameObject resourceRef = null;
    public GameObject storageConsumer;      // Init in inspector
    public int level = 0;
    public bool isMenuOpened = false;
    public float upgradeTimer = 0f;
    public GameObject _tileOccupied  = null;
    public GameObject _tileOccupied1 = null; 
    public GameObject _tileOccupied2 = null;
    public GameObject _tileOccupied3 = null;


    // UI
    public Slider healthBar; 
    public Slider shieldhBar;
    public GameObject canvas;


    // Save logic
    public void SaveData()
    {
        shtabSavingData = new ShtabSavingData();

        shtabSavingData.name = name;
        shtabSavingData._tileOccupiedName = _tileOccupied.name;
        shtabSavingData._tileOccupied1Name = _tileOccupied1.name;
        shtabSavingData._tileOccupied2Name = _tileOccupied2.name;
        shtabSavingData._tileOccupied3Name = _tileOccupied3.name;
        shtabSavingData.level = level;
        shtabSavingData.upgradeTimer = upgradeTimer;

        shtabSavingData.healthPoints = healthPoints;
        shtabSavingData.shieldPoints = shieldPoints;
        shtabSavingData.maxCurrentHealthPoints = maxCurrentHealthPoints;
        shtabSavingData.maxCurrentShieldPoints = maxCurrentShieldPoints;
        shtabSavingData.deffencePoints = deffencePoints;
        shtabSavingData.isShieldOn = isShieldOn;
        shtabSavingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;

        GameHendler.Instance.shtabSavingData = shtabSavingData;
    }




    // Upgrade logic functions
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
       

        OnDamageTaken(this); // KOSTUL'
        OnUpgraded();
    }

    public void InitStaticsLevel_3()
    {
        level = 3;
        UpgradeStats(StatsManager._maxHealth_Lvl3_Shtab, StatsManager._maxShiled_Lvl3_Shtab, StatsManager._deffencePoints_Lvl3_Shtab);


        OnDamageTaken(this); // KOSTUL'
        OnUpgraded();
    }





    // Construction and destroying of building
    public void ConstructBuilding(Model model)
    {
        // Data initialization
        CreateGameUnit(StatsManager._maxHealth_Lvl1_Shtab, StatsManager._maxShiled_Lvl1_Shtab, StatsManager._deffencePoints_Lvl1_Shtab);
        level = 1;
        name = "SHTAB";
        // _tileOccupied = GameObject.Find("9.28.-37");
        // _tileOccupied1 = GameObject.Find("9.29.-38");
        // _tileOccupied2 = GameObject.Find("10.28.-38");
        // _tileOccupied3 = GameObject.Find("10.29.-39");



        // Init rest of the information
        // InitEventsBuildingMapInfoResourceManagerReference();
    }

    public void ConstructBuildingFromFile(ShtabSavingData savingData)
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
        _tileOccupied = GameObject.Find(savingData._tileOccupiedName);
        _tileOccupied1 = GameObject.Find(savingData._tileOccupied1Name);
        _tileOccupied2 = GameObject.Find(savingData._tileOccupied2Name);
        _tileOccupied3 = GameObject.Find(savingData._tileOccupied3Name);
        level = savingData.level;
        upgradeTimer = savingData.upgradeTimer;



        // Init other data
        InitEventsBuildingMapInfoResourceManagerReference();



        // Start coroutine if base was in process
        if (upgradeTimer != 0)
        {
            StartCoroutine(UpgradeLogic());
        }
    }

    public void InitEventsBuildingMapInfoResourceManagerReference()
    {
        // UI
        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(false);


        
        // Building map info 
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[4];
        info.mapPoints[0] = _tileOccupied.transform;
        info.mapPoints[1] = _tileOccupied1.transform;
        info.mapPoints[2] = _tileOccupied2.transform;
        info.mapPoints[3] = _tileOccupied3.transform;


        // Events
        OnDamageTaken += ShtabStaticData.baseMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += BuildingsManageMenu.Instance.ReloadHPSP;
        OnUpgraded += ShtabStaticData.baseMenuReference.UpdateUIAfterBaseUpgrade;
        OnUpgraded += ResourceManager.Instance.UpgradeStatisticsAfterBaseUpgrade;


        // Resource manager reference
        ResourceManager.Instance.shtabReference = this;
    }

    public void DestroyBuilding()
    {
        Debug.Log("END OF THE GAME!");
        UIPannelManager.Instance.Loose();
    }





    // Other functions
    public Transform GetUnitDestination()
    {
        return storageConsumer.transform;
    }

    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("BaseMenu");
        
        ShtabStaticData.baseMenuReference.ReloadPanel(ResourceManager.Instance.shtabReference);
    }



    // Not finished YET!
    public void ActivateDefenceMode()
    {
        Debug.Log("Defence MODE!");
    }

    public void ActivateAttackMode()
    {
        Debug.Log("Attack MODE!");
    }



    // Damage logic functions
    private void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
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