using UnityEngine;

public class MineShaft : AliveGameUnit, IBuilding
{
    // public GameUnit gameUnit;
    public MineShaftData mineShaftData;
    public MineShaftSavingData mineShaftSavingData;

    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void Upgraded();
    public event Upgraded OnUpgraded = delegate{};

    public delegate void ShaftDestroy(AliveGameUnit gameUnit);
    public event ShaftDestroy OnShaftDestroyed = delegate{};

    public delegate void UnitManipulated();
    public event UnitManipulated OnUnitManipulated = delegate{};

    





    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (name == "CS1")
            {
                TakeDamage(10);
                // DestroyBuilding();
            }
        }
    }














    public void AddWorkerViaSlider()
    {
        mineShaftData.AddWorkerViaSlider();
        OnUnitManipulated();
    }
 
    public void RemoveWorkerViaSlider()
    {
        mineShaftData.RemoveWorkerViaSlider();
        OnUnitManipulated();
    }

    public void RemoveUnit(Unit unit)
    {
        mineShaftData.RemoveUnit(unit);
        OnUnitManipulated();
    }


    public void StartUpgrade()
    {
        StartCoroutine(mineShaftData.UpgradeLogic());
    }

    public Transform GetUnitDestination()
    {
        return mineShaftData.GetUnitDestination();
    }

    public void UpgradeToLvl2()
    {
        mineShaftData.UpgradeToLvl2();
        UpgradeStats(StatsManager._maxHealth_Lvl2_Shaft, StatsManager._maxShiled_Lvl2_Shaft, StatsManager._defensePoints_Lvl2_Shaft);

        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftExpand(this);

        OnDamageTaken(this);
        OnUpgraded();
    }

    public void UpgradeToLvl3()
    {
        mineShaftData.UpgradeToLvl3();
        UpgradeStats(StatsManager._maxHealth_Lvl3_Shaft, StatsManager._maxShiled_Lvl3_Shaft, StatsManager._defensePoints_Lvl3_Shaft);

        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftExpand(this);

        OnDamageTaken(this);
        OnUpgraded();
    }

    public override void UpgradeStats(int newHealth, int NewShield, int newDefense)
    {
        base.UpgradeStats(newHealth, NewShield, newDefense);
    }

    public void InitStatsAfterBaseUpgrade()
    {
        mineShaftData.UpgradeStatsAfterShtabUpgrade();

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

        OnDamageTaken(this);
    }

    public virtual void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("ShaftMenu");

        mineShaftData.Invoke();
    }

    public virtual void ConstructBuilding(Model model)
    {
        CreateGameUnit(StatsManager._maxHealth_Lvl1_Shaft, StatsManager._maxShiled_Lvl1_Shaft, StatsManager._defensePoints_Lvl1_Shaft);
        mineShaftData = new MineShaftData(this);

        mineShaftData.ConstructBuilding(model);
        mineShaftData.HelperObjectInit();

        OnDamageTaken += MineShaftStaticData.shaftMenuReference.ReloadSlidersHP_SP;

        OnUpgraded += MineShaftStaticData.shaftMenuReference.ReloadShaftLevelVisuals; // update buttons and visuals
        OnUpgraded += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;        // expands slider
        OnUpgraded += mineShaftData.UpdateUI;

        OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        OnUnitManipulated += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;


        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    public virtual void DestroyBuilding()
    {
        mineShaftData.DestroyBuilding();

        OnShaftDestroyed(this);
        OnUnitManipulated(); // Because after destroying - units change
    }















    public void ConstructBuildingFromFile(MineShaftSavingData mineShaftSavingData)
    {
        switch (mineShaftSavingData.type)
        {
            case 1: // Crystal
            CrystalShaft cs = Instantiate(
            CSStaticData.BuildingPrefab, 
            mineShaftSavingData.position + OffsetConstants.buildingOffset, 
            Quaternion.Euler(0f, 0f, (mineShaftSavingData.rotation*60))).GetComponent<CrystalShaft>();

            cs.ConstructShaftFromFile(mineShaftSavingData);
            break;

            case 2: // Iron
            // Instantiate(
            // ISStaticData.BuildingPrefab, 
            // mineShaftSavingData.position + OffsetConstants.buildingOffset, 
            // Quaternion.Euler(0f, 0f, (mineShaftSavingData.rotation*60)));

            // mineShaftData.type = 2;
            break;

            case 3: // Gel
            // Instantiate(
            // GSStaticData.BuildingPrefab, 
            // mineShaftSavingData.position + OffsetConstants.buildingOffset, 
            // Quaternion.Euler(0f, 0f, (mineShaftSavingData.rotation*60)));

            // mineShaftData.type = 3;
            break;
        }

        InitGameUnitFromFile(
        mineShaftSavingData.healthPoints, 
        mineShaftSavingData.maxCurrentHealthPoints,
        mineShaftSavingData.shieldPoints,
        mineShaftSavingData.maxCurrentShieldPoints,
        mineShaftSavingData.deffencePoints,
        mineShaftSavingData.isShieldOn,
        mineShaftSavingData.shieldGeneratorInfluencers);

        mineShaftData = new MineShaftData(this);
        mineShaftData.InitMineShaftDataFromFile(mineShaftSavingData);

        OnDamageTaken += MineShaftStaticData.shaftMenuReference.ReloadSlidersHP_SP;

        OnUpgraded += MineShaftStaticData.shaftMenuReference.ReloadShaftLevelVisuals; // update buttons and visuals
        OnUpgraded += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;        // expands slider
        OnUpgraded += mineShaftData.UpdateUI;

        OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        OnUnitManipulated += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;
    }



    public void CreateRelations()
    {
        for (int i = 0; i < mineShaftData._garageMembersIDs.Length; i++)
        {
            for (int j = 0; j < ResourceManager.Instance.unitsList.Count; j++)
            {
                if (mineShaftData._garageMembersIDs[i] == ResourceManager.Instance.unitsList[j].unitData.ID)
                {
                    mineShaftData.unitsWorkers.Add(ResourceManager.Instance.unitsList[j]);
                    ResourceManager.Instance.unitsList[j].WorkPlace = this;
                    break;
                }
            }
        }
    }
}