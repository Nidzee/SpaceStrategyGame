using UnityEngine;

public class GelShaft : MineShaft
{
    public static int gelShaft_counter = 0; // For Debug and iterations (OPTIONAL)
    public static GameObject gelShaftResourcePrefab; // resource prefab - got from PrefabManager
    public static Tile_Type placingTileType;
    public static Tile_Type placingTile_Optional;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;
    public GameObject tileOccupied = null; // Tile on which building is set - before setruction - set to TileFree!
    public GameObject tileOccupied1 = null; // Tile on which building is set - before setruction - set to TileFree!


    public static void InitStaticFields() // Do not touch!
    {
        placingTileType = Tile_Type.RS3_gel;
        placingTile_Optional = Tile_Type.FreeTile;
        buildingType = BuildingType.DoubleTileBuilding;
        buildingPrefab = PrefabManager.Instance.gelShaftPrefab;
        gelShaftResourcePrefab = PrefabManager.Instance.gelResourcePrefab;
    }

    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero; // grab reference to hex on which model is currently set
        tileOccupied1 = model.BTileOne; // grab reference to hex on which model is currently set
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;  // make this tile unwalkable for units and buildings
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings

        gelShaft_counter++;
        this.gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;
        this.gameObject.tag = TagConstants.buildingTag;
        this.gameObject.name = "GelShaft" + GelShaft.gelShaft_counter;
    }


    public override void Invoke() 
    {
        Debug.Log("Selected GelShaft - go menu now");
        //UIPannelManager.Instance.ResetPanels((int)InitPannelIndex.shaftPanel);
    }
}
