using UnityEngine;

public class MineShaft : MonoBehaviour, IAliveGameUnit, IBuilding
{
    public GameUnit gameUnit;
    public MineShaftData mineShaftData;
    public MineShaftSavingData mineShaftSavingData;

    public delegate void DamageTaken(GameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void Upgraded();
    public event Upgraded OnUpgraded = delegate{};

    public delegate void ShaftDestroy(GameUnit gameUnit);
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
        mineShaftData.AddWorkerViaSlider(this);
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
        StartCoroutine(mineShaftData.UpgradeLogic(this));
    }

    public Transform GetUnitDestination()
    {
        return mineShaftData.GetUnitDestination();
    }

    public void UpgradeToLvl2()
    {
        mineShaftData.UpgradeToLvl2();
        gameUnit.UpgradeStats(StatsManager._maxHealth_Lvl2_Shaft, StatsManager._maxShiled_Lvl2_Shaft, StatsManager._defensePoints_Lvl2_Shaft);

        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftExpand(this);

        OnDamageTaken(gameUnit);
        OnUpgraded();
    }

    public void UpgradeToLvl3()
    {
        mineShaftData.UpgradeToLvl3();
        gameUnit.UpgradeStats(StatsManager._maxHealth_Lvl3_Shaft, StatsManager._maxShiled_Lvl3_Shaft, StatsManager._defensePoints_Lvl3_Shaft);

        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftExpand(this);

        OnDamageTaken(gameUnit);
        OnUpgraded();
    }

    public void UpgradeStats(int newHealth, int NewShield, int newDefense)
    {
        gameUnit.UpgradeStats(newHealth, NewShield, newDefense);
    }

    public void InitStatsAfterBaseUpgrade()
    {
        mineShaftData.UpgradeStatsAfterShtabUpgrade(this);

        OnDamageTaken(gameUnit);
    }

    public void TakeDamage(int damagePoints)
    {
        if (!gameUnit.TakeDamage(damagePoints))
        {
            DestroyBuilding();
            return;
        }

        OnDamageTaken(gameUnit);
    }

    public virtual void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("ShaftMenu");

        mineShaftData.Invoke();
    }

    public virtual void ConstructBuilding(Model model)
    {
        gameUnit = new GameUnit(StatsManager._maxHealth_Lvl1_Shaft, StatsManager._maxShiled_Lvl1_Shaft, StatsManager._defensePoints_Lvl1_Shaft);
        mineShaftData = new MineShaftData();

        mineShaftData.ConstructBuilding(model);
        mineShaftData.HelperObjectInit(this);

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

        OnShaftDestroyed(gameUnit);
        OnUnitManipulated(); // Because after destroying - units change

        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
    }
}