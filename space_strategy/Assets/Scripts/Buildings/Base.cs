using UnityEngine;

public class Base : AliveGameUnit, IBuilding
{
    // public GameUnit gameUnit;
    public ShtabData shtabData;
    public ShtabSavingData shtabSavingData;
    
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void Upgraded();
    public event Upgraded OnUpgraded = delegate{};

    public delegate void ShtabDestroy(AliveGameUnit gameUnit);
    public event ShtabDestroy OnShtabDestroyed = delegate{};




    public void StartUpgrade()
    {
        shtabData.StartUpgrade();
    }

    public Transform GetUnitDestination()
    {
        return shtabData.GetUnitDestination();
    }



    public void Upgrade()
    {
        shtabData.Upgrade();

        OnUpgraded();
    }

    public void InitStaticsLevel_2()
    {
        shtabData.UpgradeToLvl2();
        UpgradeStats(StatsManager._maxHealth_Lvl2_Shtab, StatsManager._maxShiled_Lvl2_Shtab, StatsManager._deffencePoints_Lvl2_Shtab);
       
        OnDamageTaken(this);
    }

    public void InitStaticsLevel_3()
    {
        shtabData.UpgradeToLvl3();
        UpgradeStats(StatsManager._maxHealth_Lvl3_Shtab, StatsManager._maxShiled_Lvl3_Shtab, StatsManager._deffencePoints_Lvl3_Shtab);

        OnDamageTaken(this);
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
        
        shtabData.Invoke();
    }

    public void ConstructBuilding(Model model)
    {
        CreateGameUnit(StatsManager._maxHealth_Lvl1_Shtab, StatsManager._maxShiled_Lvl1_Shtab, StatsManager._deffencePoints_Lvl1_Shtab);
        shtabData = new ShtabData(this);

        shtabData.CreateBuilding(model);
        shtabData.HelperObjectInit();

        name = "BASE";
        transform.tag = TagConstants.buildingTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;
        // myName = this.name;


        
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



        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnDamageTaken += ShtabStaticData.baseMenuReference.ReloadSlidersHP_SP;

        OnUpgraded += ShtabStaticData.baseMenuReference.ReloadBaseLevelVisuals; // update buttons and visuals
        OnUpgraded += shtabData.Invoke; // Correct Button cost
        OnUpgraded += ResourceManager.Instance.UpgradeStatisticsAfterBaseUpgrade;


        ResourceManager.Instance.shtabReference = this;
    }

    public void DestroyBuilding()
    {
        shtabData.DestroyBuilding();

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





    
    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }
}