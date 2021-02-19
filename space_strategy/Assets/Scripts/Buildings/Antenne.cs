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
    
    private GameObject tileOccupied = null;  // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null; // Reference to real MapTile on which building is set

    public bool isMenuOpened = false;



    public override void TakeDamage(float damagePoints)
    {
        ///////////////////////////////
        ///// Damage logic HERE ///////
        ///////////////////////////////


        // Reloads HP/SP sliders if menu is opened
        if (isMenuOpened)
        {
            antenneMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadAntenneHP_SPAfterDamage();
    }

    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.DoubleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.antennePrefab;
    }

    public void Creation(Model model)
    {
        HealthPoints = 100;
        ShieldPoints = 100;

        this.gameObject.name = "AN1";
        ResourceManager.Instance.antenneReference = this;

        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

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
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        if (isMenuOpened)
        {
            antenneMenuReference.ExitMenu();
        }

        // No need for reloading unit manage menu
        ReloadBuildingsManageMenuInfo();
        
        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
    }


    private void ReloadBuildingsManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadBuildingsManageMenuInfo___AfterAntenneDestroying();
    }
}
