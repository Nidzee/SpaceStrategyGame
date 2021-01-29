﻿using UnityEngine;

public class CrystalShaft : MineShaft
{
    private static int crystalShaft_counter = 0;                            // For understanding which building number is this
    
    public static Tile_Type PlacingTileType {get; private set;}             // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}             // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}             // Static field - Specific prefab for creating building

    private static GameObject crystalShaftResourcePrefab;                   // Static field - specific resource Prefab (from PrefabManager)

    private GameObject tileOccupied = null;                                 // Reference to real MapTile on which building is set


    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.RS1_crystal;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.crystalShaftPrefab;
        
        crystalShaftResourcePrefab = PrefabManager.Instance.crystalResourcePrefab; // Initializing resource prefab
    }


    // Function for creating building
    public void Creation(Model model)
    {
        HealthPoints = 100;
        ShieldPoints = 100;

        ResourceManager.Instance.crystalShaftList.Add(this);

        tileOccupied = model.BTileZero; // grab reference to hex on which model is currently set
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings

        crystalShaft_counter++;
        
        this.gameObject.name = "CrystalShaft" + CrystalShaft.crystalShaft_counter;  

        base.HelperObjectInit(); 
        capacity = 3; 
    }


    // Function for displaying info
    public override void Invoke() 
    {
        base.Invoke();

        shaftMenuReference.ReloadPanel(this);
    }









    public override void DestroyShaft()
    {
        base.DestroyShaft();

        ResourceManager.Instance.crystalShaftList.Remove(this);

        if (isMenuOpened)
        {
            // Close Menu panel if it is opened
            shaftMenuReference.ExitMenu();
        }

        ReloadMenuSlider(); // Here becasue shaft destroys
        
        Destroy(gameObject);
    }
}
