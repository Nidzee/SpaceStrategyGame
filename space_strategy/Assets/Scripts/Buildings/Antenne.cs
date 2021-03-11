using UnityEngine;

//////////////////////////////////////////////////////////////////////////////




// All functionality is int GameHendler because we can create only 1 instance of Antenne
// But when Antenne dies we dont want to reset buttons timers 




//////////////////////////////////////////////////////////////////////////////

public class Antenne :  AliveGameUnit, IBuilding
{
    public static AntenneMenu antenneMenuReference;
    
    public static Tile_Type PlacingTileType {get; private set;} // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;} // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;} // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    
    private GameObject _tileOccupied = null;  // Reference to real MapTile on which building is set
    private GameObject _tileOccupied1 = null; // Reference to real MapTile on which building is set

    public bool isMenuOpened = false;

    private static int _crystalNeedForBuilding;
    private static int _ironNeedForBuilding;
    private static int _gelNeedForBuilding;

    private static int _maxHealth; 
    private static int _maxShiled; 
    private static int _maxDefensePoints;

    private static int _baseUpgradeStep;


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

    private void InitStatics()
    {
        healthPoints = _maxHealth;
        maxCurrentHealthPoints = _maxHealth;

        shieldPoints = _maxShiled;
        maxCurrentShieldPoints = _maxShiled;

        deffencePoints = _maxDefensePoints;
    }

    public static void UpgradeStatisticsAfterBaseUpgrade()
    {
        _maxHealth += _baseUpgradeStep;
        _maxShiled += _baseUpgradeStep;
    }

    public void InitStatsAfterBaseUpgrade()
    {
        healthPoints = ((_maxHealth + _baseUpgradeStep) * healthPoints) / _maxHealth;
        maxCurrentHealthPoints = (_maxHealth + _baseUpgradeStep);

        shieldPoints = ((_maxShiled + _baseUpgradeStep) * shieldPoints) / _maxShiled;
        maxCurrentShieldPoints = (_maxShiled + _baseUpgradeStep);

        deffencePoints = _maxDefensePoints; // not changing at all

        // reload everything here
        if (isMenuOpened)
        {
            antenneMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadAntenneHPSP();
    }


    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);
        ///////////////////////////////
        ///// Damage logic HERE ///////
        ///////////////////////////////


        // Reloads HP/SP sliders if menu is opened
        if (isMenuOpened)
        {
            antenneMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadAntenneHPSP();
    }

    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.DoubleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.antennePrefab;

        _crystalNeedForBuilding = 50;
        _ironNeedForBuilding = 50;
        _gelNeedForBuilding = 50;

        _maxHealth = 200; 
        _maxShiled = 150; 
        _maxDefensePoints = 10;

        _baseUpgradeStep = 25;
    }

    public void Creation(Model model)
    {
        InitStatics();

        this.gameObject.name = "AN1";
        ResourceManager.Instance.antenneReference = this;

        _tileOccupied = model.BTileZero;
        _tileOccupied1 = model.BTileOne;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        TurnAntenneButtonsON();

        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    public void Invoke() // TODO
    {
        UIPannelManager.Instance.ResetPanels("AntenneMenu");

        if (!antenneMenuReference) // executes once
        {
            antenneMenuReference = GameObject.Find("AntenneMenu").GetComponent<AntenneMenu>();
        }

        antenneMenuReference.ReloadPanel();
    }

    private void TurnAntenneButtonsON()
    {
        if (!GameHendler.Instance.isAntenneOnceCreated)
        {
            // Roll animation to open special panel with this buttons
            GameHendler.Instance.isAntenneOnceCreated = true;

            GameHendler.Instance.antenneButtonsPanel.SetActive(true);
        }

        GameHendler.Instance.resourceDropButton.interactable = ResourceManager.Instance.IsPowerOn();
        GameHendler.Instance.impusleAttackButton.interactable = ResourceManager.Instance.IsPowerOn();
    }


    public void DestroyAntenne()
    {
        GameHendler.Instance.resourceDropButton.interactable = false;
        GameHendler.Instance.impusleAttackButton.interactable = false;
        // Remove shield range

        ResourceManager.Instance.antenneReference = null;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        if (isMenuOpened)
        {
            antenneMenuReference.ExitMenu();
        }

        // No need for reloading unit manage menu
        ReloadBuildingsManageMenuInfo();
        
        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
        AstarPath.active.Scan();
    }

    private void ReloadBuildingsManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadBuildingsManageMenuInfo___AfterAntenneDestroying();
    }
}
