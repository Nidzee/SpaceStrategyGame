using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShieldGenerator :  AliveGameUnit, IBuilding
{
    public GameUnit gameUnit;
    public ShieldGeneratorData shieldGeneratorData;
    public ShieldGeneratorSavingData shieldGeneratorSavingData;

    public delegate void DamageTaken(GameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void Upgraded();
    public event Upgraded OnUpgraded = delegate{};

    public delegate void ShieldGeneraorDestroy(GameUnit gameUnit);
    public event ShieldGeneraorDestroy OnSGDestroyed = delegate{};

    

    public void StartUpgrade()
    {
        StartCoroutine(shieldGeneratorData.UpgradeLogic(this));
    }

    public void EnableShield()
    {
        shieldGeneratorData.EnableShield(this);
    }

    public void DisableShield()
    {
        shieldGeneratorData.DisableShield(this);
    }


    public void InitStatsAfterBaseUpgrade()
    {
        shieldGeneratorData.InitStatsAfterBaseUpgrade(this);
        
        OnDamageTaken(gameUnit);
    }

    public void UpgradeStats(int newHealth, int NewShield, int newDefense)
    {
        gameUnit.UpgradeStats(newHealth, NewShield, newDefense);
    }


    public void UpgradeToLvl2()
    {
        shieldGeneratorData.UpgradeToLvl2();

        gameUnit.UpgradeStats(
        StatsManager._maxHealth_Lvl2_ShieldGenerator, 
        StatsManager._maxShiled_Lvl2_ShieldGenerator, 
        StatsManager._defensePoints_Lvl2_ShieldGenerator);


        OnUpgraded();
        OnDamageTaken(gameUnit);
    }

    public void UpgradeToLvl3()
    {
        shieldGeneratorData.UpgradeToLvl3();

        gameUnit.UpgradeStats(
        StatsManager._maxHealth_Lvl3_ShieldGenerator, 
        StatsManager._maxShiled_Lvl3_ShieldGenerator, 
        StatsManager._defensePoints_Lvl3_ShieldGenerator);

        OnUpgraded();
        OnDamageTaken(gameUnit);
    }


    // Reloads sliders if Turret Menu is opened
    public override void TakeDamage(int damagePoints)
    {
        if (!gameUnit.TakeDamage(damagePoints))
        {
            DestroyBuilding();
            return;
        }

        OnDamageTaken(gameUnit);
    }

    // Function for creating building
    public void ConstructBuilding(Model model)
    {
        gameUnit = new GameUnit(StatsManager._maxHealth_Lvl1_ShieldGenerator, StatsManager._maxShiled_Lvl1_ShieldGenerator, StatsManager._defensePoints_Lvl1_ShieldGenerator);
        shieldGeneratorData = new ShieldGeneratorData();

        ShiledGeneratorStaticData.shieldGenerator_counter++;
        gameObject.name = "SG" + ShiledGeneratorStaticData.shieldGenerator_counter;

        

        shieldGeneratorData.ConstructBuilding(model);


        // Add events here
        
        // OnDamageTaken += MineShaftStaticData.shaftMenuReference.ReloadSlidersHP_SP;
        // OnUpgraded += MineShaftStaticData.shaftMenuReference.ReloadShaftLevelVisuals; // update buttons and visuals
        // OnUpgraded += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;        // expands slider
        // OnUpgraded += mineShaftData.UpdateUI;
        // OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        // OnUnitManipulated += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;
        // OnShaftDestroyed += GameViewMenu.Instance.unitManageMenuReference.RemoveCrystalScrollItem;
        // OnShaftDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
        // OnUnitManipulated += GameViewMenu.Instance.unitManageMenuReference.ReloadCrystalSlider;
        // OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;



        // Redo into one function - UpgradeVisuals
        OnUpgraded += ShiledGeneratorStaticData.shieldGeneratorMenuReference.UpgradeVisuals;
        OnDamageTaken += ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnSGDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;

        // ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadSlidersHP_SP(gameUnit);
        // ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadLevelManager();


        ResourceManager.Instance.shiledGeneratorsList.Add(this);
        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    // Function for displaying info
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("ShieldGeneratorMenu");
        
        shieldGeneratorData.Invoke();

        ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadPanel(this);
    }


    public void DestroyBuilding()
    {
        shieldGeneratorData.DestroyBuilding(this);


        // Call events here
        OnSGDestroyed(gameUnit);


        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
        AstarPath.active.Scan();
    }

}