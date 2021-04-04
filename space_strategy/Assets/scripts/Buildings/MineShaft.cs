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
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (name == "CS1")
            {
                mineShaftSavingData = new MineShaftSavingData();

                mineShaftSavingData.name = this.name;
                mineShaftSavingData.ID = this.mineShaftData.ID;
                mineShaftSavingData.isShieldOn = this.isShieldOn;
                mineShaftSavingData.shieldPoints = this.shieldPoints;
                mineShaftSavingData.healthPoints = this.healthPoints;
                mineShaftSavingData.maxCurrentHealthPoints = this.maxCurrentHealthPoints;
                mineShaftSavingData.maxCurrentShieldPoints = this.maxCurrentShieldPoints;
                mineShaftSavingData.positionTileName = this.mineShaftData._tileOccupied.name;
                mineShaftSavingData.rotation = mineShaftData.rotation;
                mineShaftSavingData.shieldGeneratorInfluencers = this.shieldGeneratorInfluencers;


                mineShaftSavingData._shaftWorkersIDs = new int[mineShaftData.unitsWorkers.Count];

                for (int i = 0; i < mineShaftData.unitsWorkers.Count; i++)
                {
                    mineShaftSavingData._shaftWorkersIDs[i] = mineShaftData.unitsWorkers[i].unitData.ID;
                }



                mineShaftSavingData.rotation = mineShaftData.rotation;

                mineShaftSavingData.type = mineShaftData.type;
                mineShaftSavingData.capacity = mineShaftData.capacity;
                mineShaftSavingData.level = mineShaftData.level;



                mineShaftSavingData._tileOccupiedName = mineShaftData._tileOccupied.name;
                if(mineShaftData._tileOccupied1 != null)
                {
                    mineShaftSavingData._tileOccupied1Name = mineShaftData._tileOccupied1.name;
                }

                mineShaftSavingData.upgradeTimer = mineShaftData.upgradeTimer;

                GameHendler.Instance.mineShaftSavingData = this.mineShaftSavingData;




                Destroy(gameObject);

                // TakeDamage(10);
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }
















































    public void ConstructBuildingFromFile(MineShaftSavingData mineShaftSavingData)
    {
        name = mineShaftSavingData.name;


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


        //Start timer


        OnDamageTaken += MineShaftStaticData.shaftMenuReference.ReloadSlidersHP_SP;
        OnUpgraded += MineShaftStaticData.shaftMenuReference.ReloadShaftLevelVisuals; // update buttons and visuals
        OnUpgraded += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;        // expands slider
        OnUpgraded += mineShaftData.UpdateUI;
        OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        OnUnitManipulated += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;


        // Add to list of shafts
    }



    public void CreateRelations()
    {
        for (int i = 0; i < mineShaftData._shaftWorkersIDs.Length; i++)
        {
            for (int j = 0; j < ResourceManager.Instance.unitsList.Count; j++)
            {
                if (mineShaftData._shaftWorkersIDs[i] == ResourceManager.Instance.unitsList[j].unitData.ID)
                {
                    Debug.Log("Add unit to shaft!" + this.name + "   " + ResourceManager.Instance.unitsList[j].name);
                    mineShaftData.unitsWorkers.Add(ResourceManager.Instance.unitsList[j]);
                    ResourceManager.Instance.unitsList[j].WorkPlace = this;
                    j = 0;
                    break;
                }
            }
        }
    }
}