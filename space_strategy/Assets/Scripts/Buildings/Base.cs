using UnityEngine;

public class Base : AliveGameUnit, IBuilding
{
    public GameUnit gameUnit;
    public ShtabData shtabData;
    public ShtabSavingData shtabSavingData;
    
    public delegate void DamageTaken(GameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void Upgraded();
    public event Upgraded OnUpgraded = delegate{};

    public delegate void ShtabDestroy(GameUnit gameUnit);
    public event ShtabDestroy OnShtabDestroyed = delegate{};




    public void StartUpgrade()
    {
        shtabData.StartUpgrade(this);
    }

    public Transform GetUnitDestination()
    {
        return shtabData.GetUnitDestination();
    }



    public void Upgrade()
    {
        shtabData.Upgrade(this);

        OnUpgraded();
    }

    public void InitStaticsLevel_2()
    {
        shtabData.UpgradeToLvl2();
        gameUnit.UpgradeStats(StatsManager._maxHealth_Lvl2_Shtab, StatsManager._maxShiled_Lvl2_Shtab, StatsManager._deffencePoints_Lvl2_Shtab);
       
        OnDamageTaken(gameUnit);
    }

    public void InitStaticsLevel_3()
    {
        shtabData.UpgradeToLvl3();
        gameUnit.UpgradeStats(StatsManager._maxHealth_Lvl3_Shtab, StatsManager._maxShiled_Lvl3_Shtab, StatsManager._deffencePoints_Lvl3_Shtab);

        OnDamageTaken(gameUnit);
    }


    public override void TakeDamage(int damagePoints)
    {
        if (!gameUnit.TakeDamage(damagePoints))
        {
            DestroyBuilding();
            return;
        }

        OnDamageTaken(gameUnit);
    }


    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("BaseMenu");
        
        shtabData.Invoke();
    }

    public void ConstructBuilding(Model model)
    {
        gameUnit = new GameUnit(StatsManager._maxHealth_Lvl1_Shtab, StatsManager._maxShiled_Lvl1_Shtab, StatsManager._deffencePoints_Lvl1_Shtab);
        shtabData = new ShtabData();

        shtabData.CreateBuilding(model);
        shtabData.HelperObjectInit(this);

        name = "BASE";
        transform.tag = TagConstants.buildingTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;
        gameUnit.name = this.name;

        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;

        OnUpgraded += ShtabStaticData.baseMenuReference.ReloadBaseLevelVisuals; // update buttons and visuals
        OnUpgraded += ShtabStaticData.baseMenuReference.ReloadSlidersHP_SP;
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
}