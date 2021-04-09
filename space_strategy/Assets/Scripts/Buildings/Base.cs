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
    private float _timerStep = 0.25f;



    public void StartUpgrade()
    {
        StartCoroutine(UpgradeLogic());
    }

    IEnumerator UpgradeLogic()
    {
        while (upgradeTimer < 1)
        {
            upgradeTimer += _timerStep * Time.deltaTime;

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
        Debug.Log(damagePoints);

        base.TakeDamage(damagePoints);

        if (healthPoints <= 0)
        {
            DestroyBuilding();
            return;
        }

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

        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[4];

        GameObject tile1 = GameObject.Find("9.28.-37");
        GameObject tile3 = GameObject.Find("9.29.-38");
        GameObject tile2 = GameObject.Find("10.28.-38");
        GameObject tile4 = GameObject.Find("10.29.-39");

        info.mapPoints[0] = tile1.transform;
        info.mapPoints[1] = tile2.transform;
        info.mapPoints[2] = tile3.transform;
        info.mapPoints[3] = tile4.transform;


        OnDamageTaken += ShtabStaticData.baseMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnUpgraded += ShtabStaticData.baseMenuReference.UpdateUIAfterBaseUpgrade;
        OnUpgraded += ResourceManager.Instance.UpgradeStatisticsAfterBaseUpgrade;


        ResourceManager.Instance.shtabReference = this;
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