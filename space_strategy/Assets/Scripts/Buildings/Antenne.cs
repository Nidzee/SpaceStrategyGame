using UnityEngine;

public class Antenne :  AliveGameUnit, IBuilding
{
    private static int antenne_counter = 0;   // For understanding which building number is this
    
    public static Tile_Type PlacingTileType {get; private set;} // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;} // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;} // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    
    private GameObject tileOccupied = null;  // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null; // Reference to real MapTile on which building is set

    public bool isMenuOpened = false;



    public override void TakeDamage(float DamagePoints)
    {
        // base.TakeDamage(DamagePoints);

        // if (isMenuOpened)
        // {
        //     //baseMenuReference.ReloadPanel(this);
        // }
    }

    public static void InitStaticFields()    // Static info about building - determins all info about every object of this building class
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.DoubleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.antennePrefab;
    }

    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        antenne_counter++;
        
        this.gameObject.name = "Antenne" + Antenne.antenne_counter;
    }


    public void Invoke() // TODO
    {
        Debug.Log("Selected Antenne - go menu now");
    }
}
