﻿using UnityEngine;

public class PowerPlant :  AliveGameUnit, IBuilding
{
    private static PowerPlantMenu powerPlantMenuReference;
    private static int powerPlant_counter = 0; // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}  // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}  // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}  // Static field - Specific prefab for creating building

    private GameObject tileOccupied = null;   // Reference to real MapTile on which building is set

    public bool isMenuOpened = false;



    private static int _crystalNeedForBuilding = 0;
    private static int _ironNeedForBuilding = 0;
    private static int _gelNeedForBuilding = 0;


    public static string GetResourcesNeedToBuildAsText()
    {
        return _crystalNeedForBuilding.ToString() + " " + _ironNeedForBuilding.ToString() +" "+_gelNeedForBuilding.ToString();
    }


    public static void GetResourcesNeedToBuild(out int crystalNeed, out int ironNeed, out int gelNeed)
    {
        crystalNeed = _crystalNeedForBuilding;
        ironNeed = _ironNeedForBuilding;
        gelNeed = _gelNeedForBuilding;
    }










    private static int maxHealth = 50; 

    private static int maxShiled = 50; 

    private static int maxDeffencePoints = 7; 

    private void InitStatics()
    {
        healthPoints = maxHealth;
        maxCurrentHealthPoints = maxHealth;

        shieldPoints = maxShiled;
        maxCurrentShieldPoints = maxShiled;

        deffencePoints = maxDeffencePoints;
    }




    private static int baseUpgradeStep = 25;

    public static void UpgradeStatisticsAfterBaseUpgrade()
    {
        maxHealth += baseUpgradeStep;
        maxShiled += baseUpgradeStep;
    }

    public void InitStatisticsAfterBaseUpgrade()
    {
        healthPoints = ((maxHealth + baseUpgradeStep) * healthPoints) / maxHealth;
        maxCurrentHealthPoints = (maxHealth + baseUpgradeStep);

        shieldPoints = ((maxShiled + baseUpgradeStep) * shieldPoints) / maxShiled;
        maxCurrentShieldPoints = (maxShiled + baseUpgradeStep);

        deffencePoints = maxDeffencePoints; // not changing at all

        // reload everything here
        if (isMenuOpened)
        {
            powerPlantMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadPowerPlantHP_SPAfterDamage(this);
    }






    // Reloads sliders if Turret Menu is opened
    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);
        ///////////////////////////////
        ///// Damage logic HERE ///////
        ///////////////////////////////


        // Reloads HP/SP sliders if menu is opened
        if (isMenuOpened)
        {
            powerPlantMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadPowerPlantHP_SPAfterDamage(this);
    }

    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.powerPlantPrefab;
    }

    // Function for creating building
    public void Creation(Model model)
    {
        InitStatics();

        powerPlant_counter++;
        this.gameObject.name = "PP" + PowerPlant.powerPlant_counter;
        ResourceManager.Instance.powerPlantsList.Add(this);

        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        
        ResourceManager.Instance.CreatePPandAddElectricityWholeCount();
    }

    // Function for displaying info
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("PowerPlantMenu");
        
        if (!powerPlantMenuReference) // executes once
        {
            powerPlantMenuReference = GameObject.Find("PowerPlantMenu").GetComponent<PowerPlantMenu>();
        }

        powerPlantMenuReference.ReloadPanel(this);
    }



    public void DestroyPP()
    {
        ResourceManager.Instance.powerPlantsList.Remove(this);

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        if (isMenuOpened)
        {
            powerPlantMenuReference.ExitMenu();
        }

        // No need for reloading unit manage menu
        ReloadBuildingsManageMenuInfo();

        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyPPandRemoveElectricityWholeCount();
        AstarPath.active.Scan();
    }


    private void ReloadBuildingsManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadBuildingsManageMenuInfo___AfterPowerPlantDestroying(this);
    }

}
